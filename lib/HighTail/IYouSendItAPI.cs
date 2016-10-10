using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

using YouSendIt.Entities;

namespace YouSendIt
{
    [Guid("694C1820-04B6-4988-928F-FD858B95C880")]
    public interface IYouSendItAPI
    {
       /// <summary>
        /// Gets or sets the API key.
        /// </summary>
        [DispId(1)]
        string APIKey
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets or sets the Auth Token.
        /// </summary>
        [DispId(3)]
        string AuthToken
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the API endpoint.
        /// </summary>
        [DispId(4)]
        string APIEndpoint
        {
            get;
            set;
        }

        /// <summary>
        /// Loggs into YouSendIt via API.
        /// </summary>
        /// <param name="email">
        /// Email address for user's YouSendIt account.
        /// </param>
        /// <param name="password">
        /// Password for user's YouSendIt account.
        /// </param>
        /// <returns>
        /// The user's authentication token.
        /// </returns>
        [DispId(13)]
        String Login(String email, String password);

        /// <summary>
        /// Get guest user authentication token.
        /// </summary>
        /// <param name="email">
        /// Email address of guest user.
        /// </param>
        /// <param name="fullname">
        /// Name of guest user. Can be null since it isn't a required parameter.
        /// </param>
        /// <returns>
        /// If successful, the guest user authentication token is set for the object and it's returned.
        /// </returns>
        [DispId(14)]
        String GuestLogin(String email, String fullname);

        /// <summary>
        /// reates a new user.
        /// </summary>
        /// <param name="email">
        /// Email address for user's YouSendIt account.
        /// </param>
        /// <param name="password">
        /// Password for user's YouSendIt account.
        /// </param>
        /// <param name="autoActivate">
        /// By default new users are sent an email with an activation link they must click to activate their account. 
        /// Set autoActivate to true if you want this user to be automatically activated.
        /// </param>
        /// <param name="firstname">
        /// User's first name (optional).
        /// </param>
        /// <param name="lastname">
        /// User's last name (optional)
        /// </param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        [DispId(15)]
        String CreateNewUser(String email, String password, bool autoActivate, String firstname, String lastname);

        /// <summary>
        /// Get upload URLs for new item.
        /// </summary>
        ///<param name="recipients">
        ///  A comma delimited list of email addresses to send notification email to upon the upload being completed.
        ///</param>
        ///<param name="fileCount"></param>
        ///<param name="subject">
        ///  Subject of the notification email sent to recipients upon the upload being completed. Set to null if there is no subject.
        ///</param>
        ///<param name="message">
        ///  Message body of the notification email sent to recipients upon the upload being completed. Set to null if there is no message body.
        ///</param>
        ///<param name="verifyIdentity">
        ///  true/false. If true, only the intended recipients can download the file(s). Recipient identity will be validated before download.
        ///</param>
        ///<param name="returnReceipt">
        ///  If true, the associated user will receive an email notification when any recipients have downloaded the file(s).
        ///</param>
        ///<param name="password">
        ///  If not null, the password for securely downloading the file(s).
        ///</param>
        ///<returns>
        /// If successful, an XMLBean for the upload, which contains the item ID and upload URLs, is returned.
        /// </returns>
        [DispId(16)]
        PrepareSendType PrepareSend(string recipients, int? fileCount, string subject, string message, bool? verifyIdentity, bool? returnReceipt, string password);

        /// <summary>
        /// Commit item send.
        /// </summary>
        ///<param name="itemID">
        ///  Item identifier obtained from prepareSend.
        ///</param>
        ///<param name="sendEmailNotifications">
        ///  If true, an email notification is sent to the sender and the recipients. If false, no email is sent.
        ///</param>
        ///<param name="expiration">
        ///  The expiration duration in minutes. A value of 0 indicates that the item never expires.
        ///</param>
        ///<returns>
        /// If successful, the URL for the YouSendIt web page to download the item is returned.
        /// </returns>
        [DispId(17)]
        CommitSendInfo CommitSend(string itemID, bool? sendEmailNotifications, int? expiration);

        /// <summary>
        /// Get upload status.
        /// </summary>
        /// <param name="uploadURL">
        /// The URL to use for uploading a file that's obtained from prepareSend.
        /// </param>
        /// <returns>
        /// The representation of the upload status.
        /// </returns>
        [DispId(18)]
        UploadStatusType GetUploadStatus(String uploadURL);

        /// <summary>
        /// Get the item info.
        /// </summary>
        /// <param name="itemID">
        /// Item identifier obtained from getSentItems, getReceivedItems, or prepareSend.
        /// </param>
        /// <returns>
        /// If successful, an XMLBean for the item is returned.
        /// </returns>
        [DispId(23)]
        ItemInfoType GetItemInfo(String itemID);

        /// <summary>
        /// Change item expiration.
        /// </summary>
        /// <param name="itemID">
        ///   Item identifier obtained from getSentItems, getReceivedItems, or prepareSend.
        /// </param>
        /// <param name="expiration">
        ///   Expiration is in minutes with 0 indicating the item never expires.
        /// </param>
        /// <returns>
        /// If successful, the new expiration info is returned.
        /// </returns>
        [DispId(24)]
        ExpirationInfo ChangeItemExpiration(string itemID, int expiration);

        /// <summary>
        /// Download a password protected file from YouSendIt. Requires a valid apiKey and authToken.
        /// </summary>
        /// <param name="downloadURL">
        ///   Download URL is obtained from Items or ItemInfo API.
        /// </param>
        /// <param name="downloadFolder">
        ///   Folder to download the file to.
        /// </param>
        /// <param name="password">
        ///   Password for the file.
        /// </param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="progress"></param>
        /// <returns>
        /// FileInfo object for the downloaded file.
        /// </returns>
        [DispId(26)]
        void DownloadFile(string downloadURL, string downloadFolder, string password, long? offset, long? length, Action<DownloadedFileInfo> progress);

        /// <summary>
        /// Upload a file to YouSendIt. This API call doesn't require any authentication (i.e., apiKey or authToken).
        /// </summary>
        /// <param name="itemID">
        /// Item ID from PrepareSend API.
        /// </param>
        /// <param name="uploadURL">
        /// Upload URL from PrepareSend API.
        /// </param>
        /// <param name="fileToUpload">
        /// Path to file to upload.
        /// </param>
        /// <param name="fileStream">
        /// File stream.
        /// </param>
        [DispId(27)]
        void UploadFile(String itemID, String uploadURL, String fileToUpload, Stream fileStream = null, string convertedFileName = "");

        /// <summary>
        /// Gets or sets the Original User Agent.
        /// </summary>
        [DispId(29)]
        string OriginalUserAgent
        {
            get;
            set;
        }

        /// <summary>
        /// Get Policy info.
        /// </summary>
        /// <returns>
        /// Representation of the Policy info.
        /// </returns>
        [DispId(30)]
        PolicyInfoType GetPolicyInfo();

        /// <summary>
        /// Creates folder on YouSendIt.
        /// </summary>
        /// <param name="folderName">
        /// The folder's name.
        /// </param>
        /// <param name="parentId">
        /// The parent Id.
        /// </param>
        /// <returns>
        /// The created folder representation.
        /// </returns>
        [DispId(31)]
        Folder CreateFolder(string folderName, int? parentId);

        /// <summary>
        /// Get folder info.
        /// </summary>
        /// <param name="folderId">
        /// The folder Id.
        /// </param>
        /// <param name="includeFiles">
        /// true/false, specifies whether to include files in the folder info.
        /// </param>
        /// <param name="includeFolders">
        /// true/false, specifies whether to include folders in the folder info.
        /// </param>
        /// <returns>
        /// The folder representation containing the folder info.
        /// </returns>
        [DispId(32)]
        Folder GetFolderInfo(int folderId, bool? includeFolders, bool? includeFiles);

        /// <summary>
        /// Get File info within a folder. 
        /// </summary>
        /// <param name="fileId">
        /// The file Id.
        /// </param>
        /// <returns>
        /// The representation of the file.
        /// </returns>
        [DispId(32)]
        FileInfoType GetFileInfo(int fileId);

        /// <summary>
        /// Initiates file upload on YouSentIt.
        /// </summary>
        /// <returns>
        /// The file upload info.
        /// </returns>
        [DispId(33)]
        FileUploadInfo InitiateFileUploadToFolder();

        /// <summary>
        /// Commits a file upload to a folder on YouSendIt.
        /// </summary>
        /// <param name="name">
        /// The name of the file to be commited.
        /// </param>
        /// <param name="fileId">
        /// The id of the file to be commited.
        /// </param>
        /// <param name="parentId">
        /// The parent id of the file to be commited.
        /// </param>
        /// <returns>
        /// The file representation of the commited file.
        /// </returns>
        [DispId(34)]
        FileInfoType CommitFileUploadToFolder(string name, string fileId, int parentId);

        /// <summary>
        /// Share the folder with other users.
        /// </summary>
        /// <param name="folderId">
        /// The folder id.
        /// </param>
        /// <param name="email">
        /// Comma separated email addresses of the users to share folders with.
        /// </param>
        /// <param name="message">
        /// The message that will go in the invitation email.
        /// </param>
        /// <param name="permission">
        /// true/false
        /// If set to read, the user cannot update the content of the folder. 
        /// If set to true, the user can update the content of the folder.
        /// </param>
        /// <returns>
        /// Share information.
        /// </returns>
        [DispId(35)]
        ShareInfo ShareFolder(int folderId, string emails, string message, string permission);

        /// <summary>
        ///Unshare a folder.
        /// </summary>
        ///<param name="folderId">
        ///  The folder id.
        ///</param>
        ///<param name="email">
        ///  The email address that needs to be removed from the share.
        ///</param>
        ///<returns>
        /// Share information.
        /// </returns>
        [DispId(36)]
        UnShareInfo UnshareFolder(int folderId, string email);

        /// <summary>
        /// Send a file from the folder to an email address.
        /// </summary>
        /// <param name="fileId">
        /// The file Id.
        /// </param>
        /// <param name="recipients">
        /// Comma separated email addresses of the users to whom the file link will be sent.
        /// </param>
        /// <returns>
        /// A File object.
        /// </returns>
        [DispId(36)]
        SendFileInfo SendFileFromFolder(int fileId, string recipients, string subject, string message, int expiration);

        /// <summary>
        /// Get list of items in user’s Inbox or Sent Items.
        /// </summary>
        /// <param name="sentItems">
        ///   If true, the API returns the number of items in user’s Sent Items only otherwise it returns the number of the items in user’s Inbox and Sent Items.
        /// </param>
        /// <param name="filter">
        ///   all: return all items; expired: returns all expired items; unexpired: returns only unexpired items
        /// </param>
        /// <param name="includeFileInfo">
        ///   If true, it returns detailed file info.
        /// </param>
        /// <param name="includeTracking">
        ///   If true, it returns the tracking information about the item.
        /// </param>
        /// <param name="page">
        ///   The API supports pagination to return information in chunks.
        /// </param>
        /// <param name="pageLength">
        ///   The number of items to return in a page. Maximum value is 100.
        /// </param>
        /// <returns>
        /// Items information.
        /// </returns>
        [DispId(37)]
        ItemsInfo ListItems(bool? sentItems, Filter? filter, bool? includeFileInfo, bool? includeTracking, int? page, int? pageLength);

        /// <summary>
        /// Get the number of items in Inbox or Sent items.
        /// </summary>
        /// <param name="filter">all: return all items; expired: returns all expired items; unexpired: returns only unexpired items
        ///</param>
        /// <param name="sentItems">true/false. If true, the API returns the number of items in user’s 
        /// Sent Items otherwise it returns the number of the items in user’s Inbox and Sent Items.
        /// </param>
        /// <returns>
        /// The Count object containing the number of items
        /// </returns>
        [DispId(38)]
        int GetItemCount(Filter? filter, bool? sentItems);

        /// <summary>
        /// Rename a folder.
        /// </summary>
        /// <param name="folderId">
        ///   The id of the folder.
        /// </param>
        /// <param name="name">
        ///   The new name of the folder.
        /// </param>
        /// <returns>
        /// A renamed Folder object status.
        /// </returns>
        [DispId(39)]
        RenameInfo RenameFolder(int folderId, string name);

        /// <summary>
        /// Move the folder to some other folder.
        /// </summary>
        /// <param name="folderId">
        ///   The id of the folder.
        /// </param>
        /// <param name="parentId">
        ///   The folder ID where user wants to move the folder.
        /// </param>
        /// <returns>
        /// A moved Folder object status.
        /// </returns>
        [DispId(40)]
        MoveInfo MoveFolder(int folderId, string parentId);

        /// <summary>
        /// Delete a folder.
        /// </summary>
        /// <param name="folderId">
        ///   The folder id to delete.
        /// </param>
        /// <returns>
        /// Delete status.
        /// </returns>
        [DispId(41)]
        DeleteInfo DeleteFolder(int folderId);

        /// <summary>
        /// Rename a file in a folder.
        /// </summary>
        /// <param name="fileId">
        ///   The file id.
        /// </param>
        /// <param name="name">
        ///   The new name of the file.
        /// </param>
        /// <returns>
        /// A File object status.
        /// </returns>
        [DispId(42)]
        RenameInfo RenameFile(int fileId, string name);

        /// <summary>
        /// Move a file to another folder.
        /// </summary>
        /// <param name="fileId">
        /// The file id.
        /// </param>
        /// <param name="parentId">
        /// The folderId where user wants to move the file.
        /// </param>
        /// <returns>
        /// A move status.
        /// </returns>
        [DispId(43)]
        MoveInfo MoveFile(int fileId, string parentId);

        /// <summary>
        /// Get file revisions of the specified file.
        /// </summary>
        /// <param name="filedId">
        /// The file id.
        /// </param>
        [DispId(44)]
        FileRevisionInfo GetFileRevisions(int filedId);

        /// <summary>
        /// Get storage revisions.
        /// </summary>
        /// <param name="fromRevision"></param>
        [DispId(45)]
        StorageChanges GetStorageRevisions(long? fromRevision);

        /// <summary>
        /// Download a specific file revision.
        /// </summary>
        /// <param name="fileId">
        /// The file Id.
        /// </param>
        /// <param name="revisionId">
        /// The revision Id.
        /// </param>
        [DispId(46)]
        void DownloadSpecificFileRevision(int fileId, int revisionId);
    }
}
