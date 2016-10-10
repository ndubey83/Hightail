using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using log4net;
using YouSendIt;
using YouSendIt.Entities;
using GenericParsing;
using ClosedXML;
using ClosedXML.Excel;
using System.Data;
using hems.HighTail.Models;
using System.Globalization;
using System.Configuration;

namespace hems.HighTail.Services {
    public interface IFiles {
        /// <summary>
        /// Upload File
        /// </summary>
        /// <param name="fileToUpload"></param>
        /// <param name="folderId"></param>
        /// <param name="convertToExcel"></param>
        /// <param name="convertedFileName"></param>
        /// <returns></returns>
        FileInfoType UploadFile(string fileToUpload, int? folderId, bool convertToExcel = false, string convertedFileName = "");

        /// <summary>
        /// Share Data File
        /// </summary>
        /// <param name="dataFile"></param>
        /// <param name="onFileSharing"></param>
        /// <returns></returns>
        SendFileInfo ShareFile(DataFileComponent dataFile, Action<DataFileComponent> onFileSharing = null);

        /// <summary>
        /// Get File details
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        FileInfoType GetFileInfo(int fileId);
    }

    public class Files : IFiles {
        private readonly IYouSendItAPI _highTailApi;
        private readonly ILog _log;
        private readonly IApiHelper _apiHelper;
        protected int _maxTryCount;
        protected readonly IShare _share;
        protected readonly int _fileExpirationDays;
        private const int _maxItemsPerCall = 100;
        private List<ItemInfoType> _unexpiredFileInfo;

        public Files(ApiHelper apiHelper
            ,IShare share) {
            _apiHelper = apiHelper;
            _log = apiHelper.Log;
            _highTailApi = apiHelper.HighTailApi;
            _maxTryCount = apiHelper.MaxTryCount;
            _share = share;
            _fileExpirationDays = Convert.ToInt32(ConfigurationManager.AppSettings["FileExpirationDays"]);
        }


        public FileInfoType GetFileInfo(int fileId) {
            FileInfoType fit = new FileInfoType();
            var response = _apiHelper.MakeApiCall<FileInfoType>(new Func<int, FileInfoType>(_highTailApi.GetFileInfo), fileId);
            if (!response.Error) {
                fit = response.Response;
            }
            return fit;
        }

        public bool CheckIfFileExistInHighTail(int fileId) {
            var response = _apiHelper.MakeApiCall<FileInfoType>(new Func<int, FileInfoType>(_highTailApi.GetFileInfo), fileId);
            if (response.Error) {
                return false;
            } else {
                return true;
            }
        }

        public SendFileInfo ShareFile(DataFileComponent dataFile, Action<DataFileComponent> onFileSharing = null) {
            int fileExpiration = dataFile.ExpirationDay.HasValue ? dataFile.ExpirationDay.Value : _fileExpirationDays;
            var shareFileInfo = _share.ShareFile(
                    Convert.ToInt32(dataFile.File.Id),
                    dataFile.Email,
                    string.Format("File for you"),
                    string.Format("Please check/download the latest file for. The link will expire in {0} days.",  fileExpiration),
                    fileExpiration * 60 * 24);
            if (onFileSharing != null) {
                onFileSharing(dataFile);
            }
            return shareFileInfo;
        }



        public List<ItemInfoType> GetUnexpiredFileInfo() {
            if (_unexpiredFileInfo == null) {

                _unexpiredFileInfo = _apiHelper.PagedApiCalls<ItemInfoType>(
                    _maxItemsPerCall,
                    (offset, pageSize) => {
                        int pageNum = (int)Math.Ceiling((double)offset / pageSize) + 1;
                        return _apiHelper.MakeApiCall<ItemsInfo>(new Func<bool?, Filter?, bool?, bool?, int?, int?,
                            ItemsInfo>(_highTailApi.ListItems),
                            true,
                            Filter.unexpired,
                            true,
                            true,
                            pageNum,
                            _maxItemsPerCall).Response.Item.ToList();
                    });
            }

            return _unexpiredFileInfo;
        }

        public FileInfoType UploadFile(string fileToUpload, int? folderId, bool convertToExcel = false, string saveFileNameAs = "") {
            _log.InfoFormat("UploadFile");
            if(!folderId.HasValue){
                folderId = 0;
            }
            var fileUploadInit = _apiHelper
                .MakeApiCall<FileUploadInfo>(new Func<FileUploadInfo>(_highTailApi.InitiateFileUploadToFolder))
                .Response;

            _log.DebugFormat("Uploading File: FileId:{0}, UploadUrl:{1}, fileToUpload: {2}", fileUploadInit.fileId, fileUploadInit.uploadUrl, fileToUpload);

            saveFileNameAs = string.IsNullOrWhiteSpace(saveFileNameAs) ? Path.GetFileNameWithoutExtension(fileToUpload) : saveFileNameAs;
            saveFileNameAs = string.Format("{0}{1}", saveFileNameAs, convertToExcel ? ".xlsx" : Path.GetExtension(fileToUpload));

            if (!convertToExcel) {
                _apiHelper
                    .MakeApiCall(
                    new Action<string, string, string, Stream, string>(_highTailApi.UploadFile)
                    , fileUploadInit.fileId
                    , fileUploadInit.uploadUrl
                    , fileToUpload
                    , null
                    , saveFileNameAs);
            }
            else {
                var stream = ConvertFlatFileToExcel(fileToUpload);
                _apiHelper
                    .MakeApiCall(
                    new Action<string, string, string, Stream, string>(_highTailApi.UploadFile)
                    , fileUploadInit.fileId
                    , fileUploadInit.uploadUrl
                    , fileToUpload
                    , stream
                    , saveFileNameAs);
            }
            var commitSend = _apiHelper.MakeApiCall<FileInfoType>(new Func<string, string, int, FileInfoType>(_highTailApi.CommitFileUploadToFolder), saveFileNameAs, fileUploadInit.fileId, folderId.Value);

            if (commitSend.Error) {
                _log.DebugFormat("File upload failed, Error:{0}", commitSend.ErrorMessage);
                EmailService.SendErrorMail(string.Format("File upload failed, Error:{0}", commitSend.ErrorMessage));
                return new FileInfoType();
            }
            else {
                _log.DebugFormat("Uploaded Filed ID {0}", commitSend.Response.Id);
            }
            return commitSend.Response;
        }

        private Stream ConvertFlatFileToExcel(string pathOfFileTobeConverted) {
            _log.DebugFormat(@"ConvertFlatFileToExcel");
            Stream memoryStream = new MemoryStream();
            XLWorkbook workBook = new XLWorkbook();
            var ds = FlatFileToDataSet(pathOfFileTobeConverted);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Columns.Count>0) {
                workBook.Worksheets.Add(ds);
            }
            else {
                var worksheet = workBook.Worksheets.Add("No Leads");
                worksheet.Cell("A1").Value = "No Leads";
            }
            workBook.SaveAs(memoryStream);
            return memoryStream;
        }


        private DataSet FlatFileToDataSet(string filePath) {
            _log.DebugFormat(@"FlatFileToDataSet");
            DataSet result = new DataSet();
            using (GenericParserAdapter parser = new GenericParserAdapter()) {
                parser.SetDataSource(filePath);
                parser.ColumnDelimiter = '\t';
                parser.FirstRowHasHeader = true;
                parser.SkipEmptyRows = false;
                result = parser.GetDataSet();
            }
            return result;
        }
    }
}
