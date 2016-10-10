using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using hems.HighTail.Models;
using log4net;
using YouSendIt;
using YouSendIt.Entities;

namespace hems.HighTail.Services {
    public interface IShare {
        /// <summary>
        /// Share's folder as well as all subfolder
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="email"></param>
        /// <param name="message"></param>
        /// <param name="writePermission"></param>
        /// <returns></returns>
        ShareInfo ShareFolder(int folderId, string email, string message, bool writePermission);
        /// <summary>
        /// Share file to a recipient and send a mail to the recipient
        /// </summary>
        /// <param name="fileId">File Id</param>
        /// <param name="recipients">Comma seperated emails</param>
        /// <param name="subject">Max 200 characters </param>
        /// <param name="message"></param>
        /// <param name="expirationMinutes">time in minutes</param>
        /// <returns></returns>
        SendFileInfo ShareFile(int fileId, string recipients, string subject, string message, int expirationMinutes);
    }

    public class Share : IShare {

        private readonly IYouSendItAPI _highTailApi;
        private readonly ILog _log;
        private readonly IApiHelper _apiHelper;
        protected int _maxTryCount;

        public Share(ApiHelper apiHelper) {
            _apiHelper = apiHelper;
            _log = apiHelper.Log;
            _highTailApi = apiHelper.HighTailApi;
            _maxTryCount = apiHelper.MaxTryCount;
        }

        public ShareInfo ShareFolder(int folderId, string email, string message, bool writePermission) {

            //really weird way of setting permission by hightail
            string permission = "read";
            if (writePermission) {
                permission = "true";
            }
            var result = _apiHelper.MakeApiCall<ShareInfo>(
                new Func<int, string, string, string, ShareInfo>(_highTailApi.ShareFolder)
                , folderId
                , email
                , message
                , permission);

            _log.DebugFormat("Folder: {0}  Shared:{1}", folderId, result.Response.Status);
            return result.Response;
        }


        public SendFileInfo ShareFile(int fileId, string recipients, string subject, string message, int expirationMinutes) {
            // api bug eventhough subject is less than 200 characters it gives error ( error starts if subject is >100).
            // so making sure subject is <=100 characters
            
            if (subject.Length>100) {
                subject = subject.Replace("-", "").Trim();
                subject = subject.Length <= 100 ? subject : subject.Substring(0, 99);
            }


            var result = _apiHelper.MakeApiCall<SendFileInfo>(
                new Func<int,string,string,string,int,SendFileInfo>(_highTailApi.SendFileFromFolder)
                ,fileId
                ,recipients
                ,subject
                ,message
                ,expirationMinutes);
            _log.DebugFormat("File :{0} Shared:{1}", fileId, result.Response.status);
            return result.Response;
        }

        public UnShareInfo UnshareFolder(int folderId, string email) {
            var result = _apiHelper.MakeApiCall<UnShareInfo>(
                new Func<int, string, UnShareInfo>(_highTailApi.UnshareFolder),
                folderId,
                email);
            _log.DebugFormat("Folder :{0} Shared:{1}", folderId, result.Response.Status);
            return result.Response;
        }
    }
}
