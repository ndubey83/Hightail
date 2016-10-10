using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YouSendIt;
using log4net;
using System.Configuration;
using YouSendIt.Entities;
using hems.HighTail.Models;
using System.Data.SqlClient;
using System.Data;
using System.Threading;

namespace hems.HighTail.Services {
    public interface IStorage {
        /// <summary>
        /// Search sub-folder by name on folder level specified by baseParentFolderId. By default it searches at top most level
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="baseParentFolderId"></param>
        /// <returns></returns>
        Folder SearchFolderByName(string folderName, int? baseParentFolderId);

        /// <summary>
        /// Create new Folder
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        Folder CreateFolder(string folderName, int? parentId);

        /// <summary>
        /// Get folder details
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        Folder GetFolderDetail(int folderId);

        /// <summary>
        /// Share Folder
        /// </summary>
        /// <param name="dataFileComponent"></param>
        /// <param name="onFolderSharing"></param>
        /// <returns></returns>
        ShareInfo ShareFolder(DataFileComponent dataFileComponent, Action<DataFileComponent> onFolderSharing = null);
    }

    public class Storage : IStorage {
        private readonly IYouSendItAPI _highTailApi;
        private readonly ILog _log;
        private readonly IApiHelper _apiHelper;
        protected int _maxTryCount;
        protected readonly IShare _share;


        public Storage(ApiHelper apiHelper
            ,IShare share) {
            _apiHelper = apiHelper;
            _log = apiHelper.Log;
            _highTailApi = apiHelper.HighTailApi;
            _maxTryCount = apiHelper.MaxTryCount;
            _share = share;
        }

        public Folder CreateFolder(string folderName, int? parentId) {
            _log.DebugFormat("CreateFolder :{0}", folderName);
            var response = _apiHelper.MakeApiCall<Folder>(new Func<string, int?, Folder>(_highTailApi.CreateFolder),folderName,parentId);
            _log.DebugFormat("Created FolderId:{0}", response.Response.Id);
            return response.Response;
        }

        public ShareInfo ShareFolder(DataFileComponent dataFileComponent, Action<DataFileComponent> onFolderSharing = null) {
            
           var shareFolderInfo =  _share.ShareFolder(dataFileComponent.Folder.Id,
                    dataFileComponent.Email,
                     "File Folder",
                    false);
           if (onFolderSharing != null) {
               onFolderSharing(dataFileComponent);
           }
           return shareFolderInfo;
        }

        public Folder GetFolderDetail(int folderId) {
            var fldr = _apiHelper.MakeApiCall<Folder>(new Func<int, bool?, bool?, Folder>(_highTailApi.GetFolderInfo), folderId, true, true);
            return fldr.Response;
        }
       
        public Folder SearchFolderByName(string folderName, int? baseParentFolderId) {
            Folder folder = new Folder();
            _log.DebugFormat("SearchFolderByName :{0}", folderName);
            if (!baseParentFolderId.HasValue) {
                baseParentFolderId = 0;
            }
            var baseFolders = _apiHelper.MakeApiCall<Folder>(new Func<int, bool?, bool?, Folder>(_highTailApi.GetFolderInfo), baseParentFolderId.Value, true, false);
            if (!baseFolders.Error && Convert.ToInt32(baseFolders.Response.FolderCount)>0) {
                folder = baseFolders.Response.Folders.Items.FirstOrDefault(f => f.Name == folderName);
            }
            return folder;
        }

    }
}

