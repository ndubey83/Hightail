/**********************************************************************************************
 * Copyright 2012 YouSendIt, Inc. All Rights Reserved.
 *
 * Licensed under the YouSendIt API agreement. You may not use this file except in compliance 
 * with the License. 
 *
 * This is a sample file distributed on an "AS IS" basis, WITHOUT WARRANTIES OR CONDITIONS OF 
 * ANY KIND, either expressed or implied. See the API License for the specific language 
 * governing permissions and limitations. 
 *********************************************************************************************/


#define NET35
using System;
using System.IO;
using System.Net;
using System.Web;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.InteropServices;

using YouSendIt.Entities;

namespace YouSendIt
{

    public enum Filter { all, expired, unexpired };

    // Events interface YouSendIt_COMObjectEvents 

    [Guid("47C976E0-C208-4740-AC42-41212D3C34F0"),
        InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface YouSendIt_Events
    {
    }

    [Guid("9E5E5FB2-219D-4ee7-AB27-E4DBED8E123E"),
        ClassInterface(ClassInterfaceType.None),
        ComSourceInterfaces(typeof(YouSendIt_Events))]
    public class YouSendItAPI : IYouSendItAPI
    {
        // public enum Filter { all, expired, unexpired };
        
        private String apiKey;
        private String originalUserAgent = "Version1";
        private String authToken;
        private String apiEndpoint;

        private const int BadRequestStatusCode = 400;
        private const String HttpDelete = "DELETE";
        private const String HttpGet = "GET";
        private const String HttpPost = "POST";
        private const String HttpPut = "PUT";
        private const String BadRequest = "Bad Request";
        private const String Boundary = "-------------------------7d6e211e043e";
        private const String BoundaryEnd = "---------------------------7d6e211e043e--";

        public static String YSIHeaderForSigningRequests = "X-YSI-Signature";
        public static String YSIApiKey = "X-Api-Key";
        public static String YSIOriginalUserAgent = "X-Original-User-Agent";

        public static String SandboxEndpoint = "https://test-api.yousendit.com";
        public static String ProductionEndpoint = "https://developer-api.yousendit.com";
        public Boolean bypass;
        
        /// <summary>
        /// You Send It API constructor.
        /// </summary>
        public YouSendItAPI()
        {
            ServicePointManager.Expect100Continue = false;
        }

        /// <summary>
        /// YouSendItAPI constructor.
        /// </summary>
        /// <param name="apiEndpoint">
        ///   Developer API endpoint to send requests to, e.g., https://api.yousendit.com.
        /// </param>
        /// <param name="apiKey">
        ///   Developer API key used to authenticate the developer.
        /// </param>
        public YouSendItAPI(string apiEndpoint, string apiKey)
        {
            ServicePointManager.Expect100Continue = false;

            this.apiKey = apiKey;
            this.apiEndpoint = apiEndpoint;
        }

        /// <summary>
        /// YouSendItAPI constructor.
        /// </summary>
        /// <param name="apiEndpoint">
        ///   Developer API endpoint to send requests to, e.g., https://api.yousendit.com.
        /// </param>
        /// <param name="apiKey">
        ///   Developer API key used to authenticate the developer.
        /// </param>
        /// <param name="authToken">
        ///   The auth token.
        /// </param>
        public YouSendItAPI(string apiEndpoint, string apiKey, string authToken)
        {
            ServicePointManager.Expect100Continue = false;

            this.apiKey = apiKey;
            this.apiEndpoint = apiEndpoint;
            this.authToken = authToken;
        }
       
        
        /// <summary>
        /// Gets or sets the API key.
        /// </summary>
        public string APIKey
        {
            get
            {
                return apiKey;
            }

            set
            {
                apiKey = value;
            }
        }

        /// <summary>
        /// Gets or sets the API endpoint.
        /// </summary>
        public string APIEndpoint
        {
            get
            {
                return apiEndpoint;
            }

            set
            {
                apiEndpoint = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the Auth Token.
        /// </summary>
        public string AuthToken
        {
            get
            {
                return authToken;
            }

            set
            {
                authToken = value;
            }
        }

        /// <summary>
        /// Gets or sets the Original User Agent.
        /// </summary>
        public string OriginalUserAgent
        {
            get
            {
                return originalUserAgent;
            }

            set
            {
                originalUserAgent = value;
            }
        }

        public override String ToString()
        {
            return "apiEndpoint [" + apiEndpoint + "], apiKey [" + apiKey + "], authToken [" + authToken + "]";
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
        public String Login(String email, String password)
        {
            RequiredCredentials(false);

            RequiredParameter(email, "email");

            RequiredParameter(password, "password");

            Dictionary<String, String> parameters = new Dictionary<String, String>
                { { "email", email }, { "password", password } };

            String responseXML = RemoteOperation("/dpi/v1/auth", HttpPost, parameters);
            checkResponseError(responseXML);
            authToken = GetXqueryText(responseXML, "//authToken[1]");

            return authToken;
        }
       

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
        public String GuestLogin(String email, String fullname)
        {
            RequiredCredentials(false);

            RequiredParameter(email, "email");

            Dictionary<String, String> parameters = new Dictionary<String, String> { { "email", email } };

            if (fullname != null)
                parameters.Add("fullname", fullname);


            String responseXML = RemoteOperation("/dpi/v1/auth/guest", HttpGet, parameters);
            checkResponseError(responseXML);
            return GetXqueryText(responseXML, "//authToken[1]");
        }

        /// <summary>
        /// Gets the user info.
        /// </summary>
        /// <param name="email">
        /// The user's email address.
        /// </param>
        /// <returns>
        /// A representation of the User Info.
        /// </returns>
        public UserInfo GetUserInfo(String email)
        {
            try
            {
                RequiredCredentials(false);

                RequiredParameter(email, "email");

                Dictionary<String, String> parameters = new Dictionary<String, String> { { "email", email } };

                String responseXML = RemoteOperation("/dpi/v2/user", HttpGet, parameters);
                checkResponseError(responseXML);

                XmlSerializer serializer = new XmlSerializer(typeof(UserInfo));
                StringReader reader = new StringReader(responseXML);
                UserInfo userInfo = (UserInfo)serializer.Deserialize(reader);

                return userInfo;
            }
            catch (Exception e)
            {
                throw new APIException(BadRequestStatusCode, BadRequest, e.Message);
            }
	    }

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
        public String CreateNewUser(String email, String password, bool autoActivate = false, String firstname = null, String lastname = null)
        {
            RequiredCredentials(false);

            RequiredParameter(email, "email");

            RequiredParameter(password, "password");

            Dictionary<String, String> parameters = new Dictionary<String, String>
                { { "email", email }, { "password", password } };

            if (autoActivate)
                parameters.Add("autoActivate", "true");

            if (firstname != null)
                parameters.Add("firstname", firstname);

            if (lastname != null)
                parameters.Add("lastname", lastname);

            String responseXML = RemoteOperation("/dpi/v1/user", HttpPost, parameters);
            checkResponseError(responseXML);
            return "OK";
        }

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
        public PrepareSendType PrepareSend(string recipients, int? fileCount, string subject, string message, bool? verifyIdentity, bool? returnReceipt, string password)
        {
            try
            {
                RequiredCredentials(true);

                RequiredParameter(recipients, "recipients");

                Dictionary<String, String> parameters = new Dictionary<String, String> { { "recipients", recipients } };

                if (fileCount.HasValue)
                {
                    parameters.Add("fileCount", fileCount.Value.ToString());
                }

                if (!string.IsNullOrEmpty(subject.Trim()))
                {
                    parameters.Add("subject", subject);
                }

                if (!string.IsNullOrEmpty(message.Trim()))
                {
                    parameters.Add("message", message);
                }

                if (verifyIdentity.HasValue)
                {
                    parameters.Add("verifyIdentity", verifyIdentity.Value.ToString().ToLower());
                }

                if (returnReceipt.HasValue)
                {
                    parameters.Add("returnReceipt", returnReceipt.Value.ToString().ToLower());
                }

                if (!string.IsNullOrEmpty(password.Trim()))
                {
                    parameters.Add("password", password);
                }

                String responseXML = RemoteOperation("/dpi/v1/item/send", HttpPost, parameters);
                checkResponseError(responseXML);


                XmlSerializer serializer = new XmlSerializer(typeof(PrepareSendType));

                StringReader reader = new StringReader(responseXML);

                PrepareSendType i = (PrepareSendType)serializer.Deserialize(reader);

                return i;
            }
            catch (Exception e)
            {
                throw new APIException(BadRequestStatusCode, BadRequest, e.Message);
            }
        }

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
        public CommitSendInfo CommitSend(string itemID, bool? sendEmailNotifications, int? expiration)
        {
            CommitSendInfo commitSendInfo;

            try
            {
                RequiredCredentials(true);

                RequiredParameter(itemID, "itemID");

                Dictionary<String, String> parameters = new Dictionary<String, String>();

                if (sendEmailNotifications.HasValue)
                {
                    parameters.Add("sendEmailNotifications", sendEmailNotifications.Value.ToString().ToLower());
                }

                if (expiration.HasValue)
                {
                    parameters.Add("expiration", expiration.Value.ToString().ToLower());
                }

                String responseXML = RemoteOperation("/dpi/v1/item/commit/" + itemID, HttpPost, parameters);
                checkResponseError(responseXML);

                XmlSerializer serializer = new XmlSerializer(typeof(CommitSendInfo));
                StringReader reader = new StringReader(responseXML);
                commitSendInfo = (CommitSendInfo)serializer.Deserialize(reader);

            }
            catch (Exception exception)
            {
                throw new APIException(BadRequestStatusCode, BadRequest, exception.Message);
            }

            return commitSendInfo;
        }

        /// <summary>
        /// Get upload status.
        /// </summary>
        /// <param name="uploadURL">
        /// The URL to use for uploading a file that's obtained from prepareSend.
        /// </param>
        /// <returns>
        /// The representation of the upload status.
        /// </returns>
        public UploadStatusType GetUploadStatus(String uploadURL)
        {
            try
            {

                RequiredCredentials(true);
                RequiredParameter(uploadURL, "uploadUrl");
                Dictionary<String, String> parameters = new Dictionary<String, String> { { "uploadUrl", uploadURL } };
                String responseXML = RemoteOperation("/dpi/v1/item/status", HttpGet, parameters);
                checkResponseError(responseXML);

                XmlSerializer serializer = new XmlSerializer(typeof(UploadStatusType));
                StringReader reader = new StringReader(responseXML);
                UploadStatusType ust = (UploadStatusType)serializer.Deserialize(reader);

                return ust;
            }
            catch (Exception e)
            {
                throw new APIException(BadRequestStatusCode, BadRequest, e.Message);
            }
        }
        
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
        public int GetItemCount(Filter? filter, bool? sentItems)
        {
            try
            {
                RequiredCredentials(true);

                Dictionary<String, String> parameters = new Dictionary<String, String>();

                if (sentItems.HasValue)
                {
                    parameters.Add("sentItems", sentItems.Value.ToString().ToLower());
                }

                if (filter.HasValue)
                {
                    parameters.Add("filter", filter.Value.ToString());
                }

                String responseXML = RemoteOperation("/dpi/v1/item/count", HttpGet, parameters);
                checkResponseError(responseXML);
                String itemCountString = GetXqueryText(responseXML, "//itemCount[1]");
                int itemCount = int.Parse(itemCountString);
                return itemCount;
            }
            catch (Exception exception)
            {
                throw new APIException(BadRequestStatusCode, BadRequest, exception.Message);
            }
        }

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
        public ItemsInfo ListItems(bool? sentItems, Filter? filter, bool? includeFileInfo, bool? includeTracking, int? page, int? pageLength)
        {
            try
            {

                RequiredCredentials(true);

                Dictionary<String, String> parameters = new Dictionary<String, String>();

                if (sentItems.HasValue)
                {
                    parameters.Add("sentItems", sentItems.Value.ToString().ToLower());
                }

                if (filter.HasValue)
                {
                    parameters.Add("filter", filter.Value.ToString());
                }

                if (includeFileInfo.HasValue)
                {
                    parameters.Add("includeFileInfo", includeFileInfo.Value.ToString().ToLower());
                }

                if (includeTracking.HasValue)
                {
                    parameters.Add("includeTracking", includeTracking.Value.ToString().ToLower());
                }

                if (page.HasValue)
                {
                    parameters.Add("page", page.Value.ToString());
                }

                if (pageLength.HasValue)
                {
                    parameters.Add("pageLength", pageLength.ToString());
                }

                String responseXML = RemoteOperation("/dpi/v1/item/list", HttpGet, parameters);
                checkResponseError(responseXML);
                XmlSerializer serializer = new XmlSerializer(typeof(ItemsInfo));
                StringReader reader = new StringReader(responseXML);
                ItemsInfo itemsInfo = (ItemsInfo)serializer.Deserialize(reader);

                return itemsInfo;
            }
            catch (Exception e)
            {
                throw new APIException(BadRequestStatusCode, BadRequest, e.Message);
            }
        }

        /// <summary>
        /// Get the item info.
        /// </summary>
        /// <param name="itemID">
        /// Item identifier obtained from getSentItems, getReceivedItems, or prepareSend.
        /// </param>
        /// <returns>
        /// If successful, an XMLBean for the item is returned.
        /// </returns>
        public ItemInfoType GetItemInfo(String itemID)
        {
            try
            {
                RequiredCredentials(true);
                RequiredParameter(itemID, "itemID");
                String responseXML = RemoteOperation("/dpi/v1/item/" + itemID, HttpGet, null);
                checkResponseError(responseXML);

                XmlSerializer serializer = new XmlSerializer(typeof(ItemInfoType));
                StringReader reader = new StringReader(responseXML);
                ItemInfoType it = (ItemInfoType)serializer.Deserialize(reader);

                return it;
            }
            catch (Exception e)
            {
                throw new APIException(BadRequestStatusCode, BadRequest, e.Message);
            }
        }
        
        /// <summary>
        /// Get Policy info.
        /// </summary>
        /// <returns>
        /// Representation of the Policy info.
        /// </returns>
        public PolicyInfoType GetPolicyInfo()
        {
            RequiredCredentials(false);

            String responseXML = RemoteOperation("/dpi/v1/policy", HttpGet, null);

            checkResponseError(responseXML);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(PolicyInfoType));
                StringReader reader = new StringReader(responseXML);
                PolicyInfoType i = (PolicyInfoType)serializer.Deserialize(reader);

                return i;
            }
            catch (Exception e)
            {
                throw new APIException(BadRequestStatusCode, BadRequest, e.Message);
            }

            // return responseXML;
        }

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
        public ExpirationInfo ChangeItemExpiration(string itemID, int expiration)
        {
            try
            {

                RequiredCredentials(true);

                RequiredParameter(itemID, "itemID");

                Dictionary<String, String> parameters = new Dictionary<String, String>
                {
                    { "expiration", expiration.ToString() }
                };

                String responseXML = RemoteOperation("/dpi/v1/item/expiration/" + itemID, HttpPost, parameters);
                checkResponseError(responseXML);

                XmlSerializer serializer = new XmlSerializer(typeof(ExpirationInfo));
                StringReader reader = new StringReader(responseXML);
                return (ExpirationInfo)serializer.Deserialize(reader);

            }
            catch (Exception exception)
            {
                throw new APIException(BadRequestStatusCode, BadRequest, exception.Message);
            }
        }

        /// <summary>
        /// Delete item.
        /// </summary>
        /// <param name="itemID">
        ///   Item Id which should be deleted.
        /// </param>
        /// <returns>
        /// A Delete object with status value OK
        /// </returns>
        public DeleteStatus DeleteItem(string itemID)
        {
            try
            {
                RequiredCredentials(true);
                RequiredParameter(itemID, "itemID");
                Dictionary<String, String> parameters = new Dictionary<String, String>();

                String responseXML = RemoteOperation("/dpi/v1/item/" + itemID, HttpDelete, parameters);
                checkResponseError(responseXML);

                XmlSerializer serializer = new XmlSerializer(typeof(DeleteStatus));
                StringReader reader = new StringReader(responseXML);
                DeleteStatus deleteStatus = (DeleteStatus)serializer.Deserialize(reader);

                return deleteStatus;
            }
            catch (Exception exception)
            {
                throw new APIException(BadRequestStatusCode, BadRequest, exception.Message);
            }
        }

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
        public void DownloadFile(string downloadURL, string downloadFolder, string password, long? offset, long? length, Action<DownloadedFileInfo> progress)
        {
            try
            {
                RequiredCredentials(true);
                RequiredParameter(downloadURL, "downloadURL");
                RequiredParameter(downloadFolder, "downloadFolder");

                HttpWebResponse httpResponse;

                Uri uri = new Uri(downloadURL);

                if (!string.IsNullOrEmpty(password.Trim()))
                {
                    uri = new Uri(downloadURL + "?password=" + HttpUtility.UrlEncode(password));
                }

                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(uri);

                Log("Calling YouSendIt Developer API [" + httpRequest.RequestUri + "]");

                httpRequest.Headers.Set(YSIApiKey, apiKey);
                SetServiceId(httpRequest);

                httpRequest.Headers.Set(YSIOriginalUserAgent, originalUserAgent);

                if (authToken != null)
                    httpRequest.Headers.Set("X-Auth-Token", authToken);

                try
                {
                    httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                }
                catch (WebException e)
                {
                    Log("WebException " + e.Message);

                    httpResponse = (HttpWebResponse)e.Response;

                    if (httpResponse == null)
                        throw new APIException(BadRequestStatusCode, BadRequest, e.Message);
                }

                Log("HTTP status " + httpResponse.StatusCode + ", " + httpResponse.StatusDescription);

                string fileName;
                if ((int)httpResponse.StatusCode != 200)
                {
                    // Failed to follow the redirect for some reason? If so, download directly from the file location.

                    if ((int)httpResponse.StatusCode == 301 || (int)httpResponse.StatusCode == 302)
                    {

                        String fileLocation = httpResponse.Headers.Get("Location");

                        // Get file name from URL of the file location.

                        Uri fileURL = new Uri(fileLocation);

                        fileName = fileURL.AbsolutePath.Substring(fileURL.AbsolutePath.LastIndexOf("/") + 1);
                        fileName = string.Format("{0}\\{1}", downloadFolder, fileName);

                        if (fileLocation == null)
                            throw new APIException((int)httpResponse.StatusCode, httpResponse.StatusDescription, "No file location");

                        Log("Downloading " + fileName + " from " + fileLocation);

                        uri = new Uri(fileLocation);

                        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);
                        if (offset.HasValue && length.HasValue)
                        {
                            webRequest.AddRange(offset.Value, offset.Value + length.Value);
                        }

                        webRequest.BeginGetResponse((asynchronousResult) =>
                        {
                            HttpWebResponse response = (HttpWebResponse)webRequest.EndGetResponse(asynchronousResult);

                            byte[] buffer = new byte[1024];
                            int bytesProcessed = 0;
                            Stream responseStream = response.GetResponseStream();

                            using (FileStream fileStream = File.Create(fileName))
                            {
                                int bytesRead;
                                do
                                {
                                    bytesRead = responseStream.Read(buffer, 0, buffer.Length);
                                    fileStream.Write(buffer, 0, bytesRead);
                                    bytesProcessed += bytesRead;

                                    progress(new DownloadedFileInfo
                                    {
                                        BytesReceived = bytesProcessed,
                                        Progress = (double)(bytesProcessed * 100) / httpResponse.ContentLength
                                    });
                                }
                                while (bytesRead > 0);

                                 progress(new DownloadedFileInfo
                                        {
                                            File = new FileInfo(fileName)
                                        });
                                
                            }
                        }, null);
                        
                    }
                    else
                    {
                        string apiResponse = GetText(httpResponse.GetResponseStream()); 
                        throw new APIException((int)httpResponse.StatusCode, httpResponse.StatusDescription, apiResponse);
                    }
                }
                else
                {
                    Uri responseUri = httpResponse.ResponseUri;
                    fileName = responseUri.AbsolutePath.Substring(responseUri.AbsolutePath.LastIndexOf("/") + 1);
                    fileName = string.Format("{0}\\{1}", downloadFolder, fileName);
                    
                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(responseUri);
                    if(offset.HasValue && length.HasValue)
                    {
                        webRequest.AddRange(offset.Value, offset.Value + length.Value);
                    }

                    webRequest.BeginGetResponse((asynchronousResult) =>
                    {
                        HttpWebResponse response = (HttpWebResponse)webRequest.EndGetResponse(asynchronousResult);

                        byte[] buffer = new byte[1024];
                        int bytesProcessed = 0;
                        Stream responseStream = response.GetResponseStream();

                        using (FileStream fileStream = File.Create(fileName))
                        {
                            int bytesRead;
                            do
                            {
                                bytesRead = responseStream.Read(buffer, 0, buffer.Length);
                                fileStream.Write(buffer, 0, bytesRead);
                                bytesProcessed += bytesRead;

                                progress(new DownloadedFileInfo
                                {
                                    BytesReceived = bytesProcessed,
                                    Progress = (double)(bytesProcessed * 100) / httpResponse.ContentLength
                                });
                            }
                            while (bytesRead > 0);

                            progress(new DownloadedFileInfo
                            {
                                File = new FileInfo(fileName)
                            });

                        }
                    }, null);
                }
            }
            catch (Exception e)
            {
                throw new APIException(BadRequestStatusCode, BadRequest, e.Message);
            }
        }
        
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
        public void UploadFile(String itemID, String uploadURL, String fileToUpload, Stream fileStream = null, string convertedFileName = "")
        {
            RequiredParameter(itemID, "itemID");

            RequiredParameter(uploadURL, "uploadURL");

            RequiredParameter(fileToUpload, "fileToUpload");

            if (File.Exists(fileToUpload) == false)
                throw new APIException(BadRequestStatusCode, BadRequest, "File to upload doesn't exist");

            MemoryStream contentPreamble = new MemoryStream();

            long contentLength = GetContentLength(itemID, fileToUpload, contentPreamble);

            Log("Content length is = " + contentLength);

            Uri uri = new Uri(uploadURL);

            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(uri);

            httpRequest.Method = HttpPost;
            httpRequest.Accept = "text/xml";
            httpRequest.ContentType = "multipart/form-data; boundary=" + Boundary;
            //httpRequest.ContentLength = contentLength;
            httpRequest.AllowAutoRedirect = false;

            // Enable streaming of a HTTP request body without internal buffering, when the content length is known in advance.
            // This MUST be set in order to enable uploading large files when the memory allocated to the JVM is small.	
            httpRequest.AllowWriteStreamBuffering = true;

            // Getting "System.Net.WebException: The request was aborted: The request was canceled." when uploading 100MB file.
            // This is a problem others have experienced, and it's recommended that the timeout values be adjusted.

            httpRequest.Timeout = -1;   // infinite - also req.Timeout works
            httpRequest.ReadWriteTimeout = 10 * 60 * 1000;

            // Get the request stream and write the post data in.
            Stream requestStream = httpRequest.GetRequestStream();

            //MemoryStream requestStream = new MemoryStream();

            // Send content preamble, i.e., everything prior to sending the file.
            contentPreamble.WriteTo(requestStream);

            SendFile(fileToUpload, requestStream, fileStream, convertedFileName);

            MemoryStream coda = new MemoryStream();

            coda.Write(Encoding.UTF8.GetBytes(BoundaryEnd + "\r\n"), 0, BoundaryEnd.Length + 2);

            coda.WriteTo(requestStream);

            requestStream.Flush();

            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();

            string apiResponse = GetText(httpResponse.GetResponseStream());

            Log("Remote response is:\n" + apiResponse);

            if ((int)httpResponse.StatusCode != 200)
                throw new APIException((int)httpResponse.StatusCode, httpResponse.StatusDescription, "Upload failed");

            // Parse response from storage server and confirm that upload succeeded.
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(UploadResponseType));
                StringReader reader = new StringReader(apiResponse);
                UploadResponseType urt = (UploadResponseType)serializer.Deserialize(reader);

                if (urt.uploadstatus.Equals("failed"))
                    throw new Exception("Upload failed: error code " + urt.ysierror.errorcode + ", " + urt.ysierror.errormessage);
            }
            catch (Exception e)
            {
                throw new APIException(BadRequestStatusCode, BadRequest, e.Message);
            }
        }

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
        public Folder CreateFolder(string folderName, int? parentId)
        {
            Folder folder;
            try
            {

                RequiredCredentials(true);

                RequiredParameter(folderName, "name");

                Dictionary<String, String> parameters = new Dictionary<String, String>
                {
                    { "name", folderName }
                };

                if (parentId.HasValue)
                {
                    parameters.Add("parentId", parentId.Value.ToString());
                }

                String responseXML = RemoteOperation("/dpi/v1/folder", HttpPost, parameters);
                checkResponseError(responseXML);


                XmlSerializer serializer = new XmlSerializer(typeof(Folder));
                StringReader reader = new StringReader(responseXML);
                folder = (Folder)serializer.Deserialize(reader);
            }
            catch (Exception e)
            {
                throw new APIException(BadRequestStatusCode, BadRequest, e.Message);
            }

            return folder;
        }

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
        public Folder GetFolderInfo(int folderId,  bool? includeFolders, bool? includeFiles)
        {
            Folder folder;

            try
            {

                RequiredCredentials(true);

                Dictionary<String, String> parameters = new Dictionary<String, String>();

                if(includeFiles.HasValue)
                {
                    parameters.Add("includeFiles", Convert.ToInt32(includeFiles).ToString());
                }

                if(includeFolders.HasValue)
                {
                    parameters.Add("includeFolders", Convert.ToInt32(includeFolders).ToString());
                }
                
                String responseXML = RemoteOperation(string.Format("/dpi/v1/folder/{0}", folderId), HttpGet, parameters);
                checkResponseError(responseXML);

                XmlSerializer serializer = new XmlSerializer(typeof(Folder));
                StringReader reader = new StringReader(responseXML);
                folder = (Folder)serializer.Deserialize(reader);
            }
            catch (Exception e)
            {
                throw new APIException(BadRequestStatusCode, BadRequest, e.Message);
            }

            return folder;
        }

        /// <summary>
        /// Get File info within a folder. 
        /// </summary>
        /// <param name="fileId">
        /// The file Id.
        /// </param>
        /// <returns>
        /// The representation of the file.
        /// </returns>
        public FileInfoType GetFileInfo(int fileId)
        {
            FileInfoType fileInfoType;
            try
            {
                RequiredCredentials(true);

                String responseXML = RemoteOperation(string.Format("/dpi/v1/folder/file/{0}", fileId), HttpGet, null);
                checkResponseError(responseXML);

                XmlSerializer serializer = new XmlSerializer(typeof(FileInfoType));
                StringReader reader = new StringReader(responseXML);
                fileInfoType = (FileInfoType)serializer.Deserialize(reader);
            }
            catch (Exception exception)
            {
                throw new APIException(BadRequestStatusCode, BadRequest, exception.Message);
            }

            return fileInfoType;
        }

        /// <summary>
        /// Download a specific file revision.
        /// </summary>
        /// <param name="fileId">
        /// The file Id.
        /// </param>
        /// <param name="revisionId">
        /// The revision Id.
        /// </param>
        public void DownloadSpecificFileRevision(int fileId, int revisionId)
        {
            try
            {
                RequiredCredentials(true);
                RequiredParameter(fileId, "fileId");
                RequiredParameter(revisionId, "revisionId");

                String responseXML = RemoteOperation(string.Format("/dpi/v1/file/download/{0}/revisions/{1}", fileId, revisionId), HttpGet, null);
                checkResponseError(responseXML);

                XmlSerializer serializer = new XmlSerializer(typeof(FileInfoType));
                StringReader reader = new StringReader(responseXML);
                //fileInfoType = (FileInfoType)serializer.Deserialize(reader);
            }
            catch (Exception exception)
            {
                throw new APIException(BadRequestStatusCode, BadRequest, exception.Message);
            }

            //return fileInfoType;
        }

        /// <summary>
        /// Initiates file upload on YouSentIt.
        /// </summary>
        /// <returns>
        /// The file upload info.
        /// </returns>
        public FileUploadInfo InitiateFileUploadToFolder()
        {
            FileUploadInfo prepareSendType;

            try
            {
                RequiredCredentials(true);
                String responseXML = RemoteOperation(string.Format("/dpi/v1/folder/file/initUpload"), HttpPost, null);
                checkResponseError(responseXML);

                XmlSerializer serializer = new XmlSerializer(typeof(FileUploadInfo));
                StringReader reader = new StringReader(responseXML);
                prepareSendType = (FileUploadInfo)serializer.Deserialize(reader);
            }
            catch (Exception exception)
            {
                throw new APIException(BadRequestStatusCode, BadRequest, exception.Message);
            }

            return prepareSendType;
        }

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
        public FileInfoType CommitFileUploadToFolder(string name, string fileId, int parentId)
        {
            FileInfoType fileInfo;

            try
            {
                RequiredCredentials(true);

                Dictionary<String, String> parameters = new Dictionary<String, String>
                {
                    { "name", name },
                    { "fileId", fileId },
                    { "parentId", parentId.ToString() }
                };

                String responseXML = RemoteOperation(string.Format("/dpi/v1/folder/file/commitUpload"), HttpPost, parameters);
                checkResponseError(responseXML);

                XmlSerializer serializer = new XmlSerializer(typeof(FileInfoType));
                StringReader reader = new StringReader(responseXML);
                fileInfo = (FileInfoType)serializer.Deserialize(reader);
            }
            catch (Exception exception)
            {
                throw new APIException(BadRequestStatusCode, BadRequest, exception.Message);
            }

            return fileInfo;
        }

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
        public ShareInfo ShareFolder(int folderId, string email, string message, string permission)
        {
            ShareInfo shareInfo;
            try
            {
                RequiredCredentials(true);
                RequiredParameter(email, "email");

                Dictionary<String, String> parameters = new Dictionary<String, String>
                {
                    { "email", email },
                    { "messasge", message },
                    { "permission", permission}
                };

                String responseXML = RemoteOperation(string.Format("/dpi/v1/folder/{0}/share", folderId), HttpPost, parameters);
                checkResponseError(responseXML);

                XmlSerializer serializer = new XmlSerializer(typeof(ShareInfo));
                StringReader reader = new StringReader(responseXML);
                shareInfo = (ShareInfo)serializer.Deserialize(reader);
            }
            catch (Exception exception)
            {
                throw new APIException(BadRequestStatusCode, BadRequest, exception.Message);
            }

            return shareInfo;
        }

        /// <summary>
        /// Get the share info of a folder.
        /// </summary>
        /// <param name="folderId">
        /// The folder id.
        /// </param>
        /// <returns>
        /// A Share object.
        /// </returns>
        public ShareMember GetShareMemberInfo(int folderId)
        {
            ShareMember shareMemberInfo;
            try
            {
                RequiredCredentials(true);

                String responseXML = RemoteOperation(string.Format("/dpi/v1/folder/{0}/share", folderId), HttpGet, null);
                checkResponseError(responseXML);

                XmlSerializer serializer = new XmlSerializer(typeof(ShareMember));
                StringReader reader = new StringReader(responseXML);
                shareMemberInfo = (ShareMember)serializer.Deserialize(reader);
            }
            catch (Exception exception)
            {
                throw new APIException(BadRequestStatusCode, BadRequest, exception.Message);
            }

            return shareMemberInfo;
        }

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
        public UnShareInfo UnshareFolder(int folderId, string email)
        {
            UnShareInfo shareInfo;
            try
            {
                RequiredCredentials(true);
                RequiredParameter(email, "email");

                String responseXML = MakeHttpPutRequest(string.Format("/dpi/v1/folder/{0}/unshare?email={1}", folderId, email), null);
                checkResponseError(responseXML);

                XmlSerializer serializer = new XmlSerializer(typeof(UnShareInfo));
                StringReader reader = new StringReader(responseXML);
                shareInfo = (UnShareInfo)serializer.Deserialize(reader);
            }
            catch (Exception exception)
            {
                throw new APIException(BadRequestStatusCode, BadRequest, exception.Message);
            }

            return shareInfo;
        }

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
        public SendFileInfo SendFileFromFolder(int fileId, string recipients, string subject, string message,int expiration)
        {
            SendFileInfo sendFileInfo;
            try
            {
                RequiredParameter(recipients, "recipients");

                Dictionary<String, String> parameters = new Dictionary<String, String>
                {
                    { "recipients", recipients },
                    {"verifyIdentity","true"},
                    {"subject",subject},
                    {"message",message},
                    {"expiration",expiration.ToString()}

                };

                String responseXML = RemoteOperation(string.Format("/dpi/v1/folder/file/{0}/send", fileId), HttpPost, parameters);
                checkResponseError(responseXML);

                XmlSerializer serializer = new XmlSerializer(typeof(SendFileInfo));
                StringReader reader = new StringReader(responseXML);
                sendFileInfo = (SendFileInfo)serializer.Deserialize(reader);
            }
            catch (Exception exception)
            {
                throw new APIException(BadRequestStatusCode, BadRequest, exception.Message);
            }

            return sendFileInfo;
        }

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
        public RenameInfo RenameFolder(int folderId, string name)
        {
            try
            {
                RequiredCredentials(true);

                RequiredParameter(folderId, "folderId");
                RequiredParameter(name, "name");

                String responseXML = MakeHttpPutRequest(string.Format("/dpi/v1/folder/{0}/rename?name={1}", folderId, name), null);
                checkResponseError(responseXML);

                XmlSerializer serializer = new XmlSerializer(typeof(RenameInfo));
                StringReader reader = new StringReader(responseXML);
                return (RenameInfo)serializer.Deserialize(reader);
            }
            catch (Exception exception)
            {
                throw new APIException(BadRequestStatusCode, BadRequest, exception.Message);
            }
        }

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
        public MoveInfo MoveFolder(int folderId, string parentId)
        {
            try
            {
                RequiredCredentials(true);

                RequiredParameter(folderId, "folderId");
                RequiredParameter(parentId, "parentId");

                String responseXML = MakeHttpPutRequest(string.Format("/dpi/v1/folder/{0}/move?parentId={1}", folderId, parentId), null);
                checkResponseError(responseXML);

                XmlSerializer serializer = new XmlSerializer(typeof(MoveInfo));
                StringReader reader = new StringReader(responseXML);
                return (MoveInfo)serializer.Deserialize(reader);
            }
            catch (Exception exception)
            {
                throw new APIException(BadRequestStatusCode, BadRequest, exception.Message);
            }
        }

        /// <summary>
        /// Delete a folder.
        /// </summary>
        /// <param name="folderId">
        ///   The folder id to delete.
        /// </param>
        /// <returns>
        /// Delete status.
        /// </returns>
        public DeleteInfo DeleteFolder(int folderId)
        {
            try
            {
                RequiredCredentials(true);
                RequiredParameter(folderId, "folderId");

                String responseXML = RemoteOperation("/dpi/v1/folder/" + folderId, HttpDelete, null);
                checkResponseError(responseXML);

                XmlSerializer serializer = new XmlSerializer(typeof(DeleteInfo));
                StringReader reader = new StringReader(responseXML);
                return (DeleteInfo)serializer.Deserialize(reader);
            }
            catch (Exception exception)
            {
                throw new APIException(BadRequestStatusCode, BadRequest, exception.Message);
            }
        }

        /// <summary>
        /// Delete a file from a folder.
        /// </summary>
        /// <param name="fileId">
        ///   The file id to delete.
        /// </param>
        /// <returns>
        /// Delete status.
        /// </returns>
        public DeleteInfo DeleteFile(int fileId)
        {
            try
            {
                RequiredCredentials(true);

                RequiredParameter(fileId, "fileId");

                String responseXML = RemoteOperation("/dpi/v1/folder/file/" + fileId, HttpDelete, null);
                checkResponseError(responseXML);

                XmlSerializer serializer = new XmlSerializer(typeof(DeleteInfo));
                StringReader reader = new StringReader(responseXML);
                return (DeleteInfo)serializer.Deserialize(reader);
            }
            catch (Exception exception)
            {
                throw new APIException(BadRequestStatusCode, BadRequest, exception.Message);
            }
        }

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
        public RenameInfo RenameFile(int fileId, string name)
        {
            try
            {
                RequiredCredentials(true);

                RequiredParameter(fileId, "fileId");
                RequiredParameter(name, "name");

                String responseXML = MakeHttpPutRequest(string.Format("/dpi/v1/folder/file/{0}/rename?name={1}", fileId, name), null);
                checkResponseError(responseXML);

                XmlSerializer serializer = new XmlSerializer(typeof(RenameInfo));
                StringReader reader = new StringReader(responseXML);
                return (RenameInfo)serializer.Deserialize(reader);
            }
            catch (Exception exception)
            {
                throw new APIException(BadRequestStatusCode, BadRequest, exception.Message);
            }
        }

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
        public MoveInfo MoveFile(int fileId, string parentId)
        {
            try
            {
                RequiredCredentials(true);

                RequiredParameter(fileId, "fileId");
                RequiredParameter(parentId, "parentId");

                String responseXML = MakeHttpPutRequest(string.Format("/dpi/v1/folder/file/{0}/move?parentId={1}", fileId, parentId), null);
                checkResponseError(responseXML);

                XmlSerializer serializer = new XmlSerializer(typeof(MoveInfo));
                StringReader reader = new StringReader(responseXML);
                return (MoveInfo)serializer.Deserialize(reader);
            }
            catch (Exception exception)
            {
                throw new APIException(BadRequestStatusCode, BadRequest, exception.Message);
            }
        }

        /// <summary>
        /// Get file revisions of the specified file.
        /// </summary>
        /// <param name="filedId">
        /// The file id.
        /// </param>
        /// <returns>
        /// A file revision info.
        /// </returns>
        public FileRevisionInfo GetFileRevisions(int filedId)
        {
            try
            {
                RequiredCredentials(true);

                String responseXML = RemoteOperation(string.Format("/dpi/v1/folder/file/{0}/revisions", filedId), HttpGet, null);
                checkResponseError(responseXML);

                XmlSerializer serializer = new XmlSerializer(typeof(FileRevisionInfo));
                StringReader reader = new StringReader(responseXML);
                return (FileRevisionInfo)serializer.Deserialize(reader);
            }
            catch (Exception exception)
            {
                throw new APIException(BadRequestStatusCode, BadRequest, exception.Message);
            }
        }

        /// <summary>
        /// Get storage revisions.
        /// </summary>
        /// <param name="fromRevision">
        ///   From revision.
        /// </param>
        /// <returns>
        /// Storage changes.
        /// </returns>
        public StorageChanges GetStorageRevisions(long? fromRevision)
        {
            try
            {
                RequiredCredentials(true);

                Dictionary<String, String> parameters = null;

                if (fromRevision.HasValue)
                {
                    parameters = new Dictionary<String, String>
                {
                    { "fromRevision", fromRevision.ToString() },
                };
                }

                String responseXML = RemoteOperation("/dpi/v1/storage/revisions", HttpGet, parameters);
                checkResponseError(responseXML);

                XmlSerializer serializer = new XmlSerializer(typeof(StorageChanges));
                StringReader reader = new StringReader(responseXML);
                return (StorageChanges)serializer.Deserialize(reader);
            }
            catch (Exception exception)
            {
                throw new APIException(BadRequestStatusCode, BadRequest, exception.Message);
            }
        }

        private string MakeHttpPutRequest(string resource, string parameter)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(apiEndpoint + resource);
            httpWebRequest.Method = HttpPut;
            httpWebRequest.Headers.Set(YSIOriginalUserAgent, originalUserAgent);
            httpWebRequest.ContentType = "text/xml";

            if (authToken != null)
            {
                httpWebRequest.Headers.Set("X-Auth-Token", authToken);
            }

            if(APIKey != null)
            {
                httpWebRequest.Headers.Set("X-Api-Key", APIKey);
            }

            if (parameter != null)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(parameter);

                // Set the content length of the string being posted. Must be done BEFORE getting the request stream.
                httpWebRequest.ContentLength = bytes.Length;

                // Get the request stream and write the post data in.
                Stream requestStream = httpWebRequest.GetRequestStream();

                requestStream.Write(bytes, 0, bytes.Length);
            }

            HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string response = GetText(httpResponse.GetResponseStream());
            httpResponse.Close();

            return response;
        }

        private String RemoteOperation(String resource, String httpVerb, Dictionary<String, String> parameters)
        {
            String response;

            HttpWebRequest httpRequest;

            HttpWebResponse httpResponse;

            try
            {
                DateTime date = DateTime.UtcNow;

                String httpDate = date.ToString("R");
#if !(NET4)
                if (parameters == null)
                    parameters = new Dictionary<String, String>();

                parameters.Add("date", httpDate);
#endif
                if (httpVerb.Equals(HttpGet) || httpVerb.Equals(HttpDelete))
                {
                    resource = AddEncodedParameters(resource, parameters);
                }

                httpRequest = (HttpWebRequest)WebRequest.Create(apiEndpoint + resource);

                Log("Calling YouSendIt Developer API [" + httpRequest.RequestUri + "]");
#if (NET4)
				httpRequest.Date = date;
#endif
                // httpRequest.Headers.Set(YSIHeaderForSigningRequests, getSignature(httpVerb, httpDate, httpRequest.RequestUri.AbsolutePath));
                httpRequest.Headers.Set(YSIApiKey, apiKey);
                SetServiceId(httpRequest);

                httpRequest.Headers.Set(YSIOriginalUserAgent, originalUserAgent);
                if (authToken != null)
                    httpRequest.Headers.Set("X-Auth-Token", authToken);

                if (httpVerb.Equals(HttpPost))
                {
                    DoPost(httpRequest, parameters);
                }

                if(httpVerb.Equals(HttpPut))
                {
                  WriteHttpPutRequest(httpRequest, parameters);   
                }

                if (httpVerb.Equals(HttpDelete))
                {
                    httpRequest.Method = HttpDelete;
                }

                httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                response = GetText(httpResponse.GetResponseStream());

                if (httpResponse.StatusCode == HttpStatusCode.OK || httpResponse.StatusCode == HttpStatusCode.Created)
                    Log("Dev API response is:\n" + response);
                else
                    throw new Exception(); // Throw exception which transfers control to the exception handler.
            }
            catch (WebException e)
            {
                httpResponse = (HttpWebResponse)e.Response;

                if (httpResponse == null)
                {
                    throw new APIException(BadRequestStatusCode, BadRequest, e.Message);
                }

                response = GetText(httpResponse.GetResponseStream());
                Log("Dev API response is:\n" + response);
                throw new APIException((int)httpResponse.StatusCode, httpResponse.StatusDescription, response);
            }
            catch (Exception e)
            {
                throw new APIException(BadRequestStatusCode, BadRequest, e.Message);
            }

            return response;
        }

        private String AddEncodedParameters(String restAPI, Dictionary<String, String> parameters) // throws Exception
        {
            if (parameters == null)
                return restAPI;

            bool firstParameter = true;

            // C# StringBuilder is the equivalent to Java's StringBuffer.

            StringBuilder strbuf = new StringBuilder(restAPI);

            // C# has the "foreach" statement which appears to be equivalent to Java but with different syntax.

            foreach (String parameter in parameters.Keys)
            {
                if (firstParameter)
                {
                    strbuf.Append("?");
                    firstParameter = false;
                }
                else
                    strbuf.Append("&");

                // C# has System.Web.HttpUtility to handle URL encoding, however "System.Web" had to be explicity added to the project references.

                strbuf.Append(parameter).Append("=").Append(HttpUtility.UrlEncode(parameters[parameter]));
            }

            return strbuf.ToString();
        }

        private void WriteHttpPutRequest(HttpWebRequest httpWebRequest, Dictionary<String, String> parameters)
        {
            httpWebRequest.Method = HttpPut;

            // Set the content type of the data being posted.
            httpWebRequest.ContentType = "text/xml";

            if (parameters == null)
                return;

            try
            {
                int i = 0;

                StringBuilder content = new StringBuilder();

                foreach (String parameter in parameters.Keys)
                {
                    if (i > 0)
                        content.Append("&");

                    content.Append(parameter).Append("=").Append(HttpUtility.UrlEncode(parameters[parameter]));

                    i++;
                }

                // Get the bytes for the request; should be pre-escaped.
                byte[] bytes = Encoding.UTF8.GetBytes(content.ToString());

                // Set the content length of the string being posted. Must be done BEFORE getting the request stream.
                httpWebRequest.ContentLength = bytes.Length;

                // Get the request stream and write the post data in.
                Stream requestStream = httpWebRequest.GetRequestStream();

                requestStream.Write(bytes, 0, bytes.Length);
            }
            catch (Exception e)
            {
                Log("Exception: " + e + "\nStack trace: " + e.StackTrace);
                throw new APIException(BadRequestStatusCode, BadRequest, e.Message);
            }
        }

        private void DoPost(HttpWebRequest httpRequest, Dictionary<String, String> parameters)
        {
            httpRequest.Method = HttpPost;

            // Set the content type of the data being posted.
            httpRequest.ContentType = "application/x-www-form-urlencoded";

            if (parameters == null)
                return;

            try
            {
                int i = 0;

                StringBuilder content = new StringBuilder();

                foreach (String parameter in parameters.Keys)
                {
                    if (i > 0)
                        content.Append("&");

                    content.Append(parameter).Append("=").Append(HttpUtility.UrlEncode(parameters[parameter]));

                    i++;
                }

                // Get the bytes for the request; should be pre-escaped.
                byte[] bytes = Encoding.UTF8.GetBytes(content.ToString());

                // Set the content length of the string being posted. Must be done BEFORE getting the request stream.
                httpRequest.ContentLength = bytes.Length;

                // Get the request stream and write the post data in.
                Stream requestStream = httpRequest.GetRequestStream();

                requestStream.Write(bytes, 0, bytes.Length);
            }
            catch (Exception e)
            {
                Log("Exception: " + e + "\nStack trace: " + e.StackTrace);
                throw new APIException(BadRequestStatusCode, BadRequest, e.Message);
            }
        }


        // This is an equivalent for CopyTo (which is only available for .NET 4.0 and above) which came from Microsoft's web site.
        private static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[32768];

            while (true)
            {
                int read = input.Read(buffer, 0, buffer.Length);
                if (read <= 0) return;
                output.Write(buffer, 0, read);
            }
        }

        private String GetText(Stream datastream)
        {
            String response = "";

            try
            {
                // Open the stream using a StreamReader for easy access.
                using (StreamReader reader = new StreamReader(datastream))
                {
                    response = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Log("Exception: " + e + "\nStack trace: " + e.StackTrace);
            }

            return response.Trim(); // Remove trailing newline.
        }

        private long GetFileContentLength(String filename) // throws Exception
        {
            const string type = "application/octet-stream";

            String field = "--" + Boundary + "\r\n";

            field = field + "Content-Disposition: form-data; name=\"fname\"; filename=\"" + filename + "\"\r\n";

            field = field + "Content-Type: " + type + "\r\n";

            field = field + "\r\n";

            long fileContentLength = Encoding.UTF8.GetBytes(field).Length;

            // This where the contents of the file would be sent.

            FileInfo file = new FileInfo(filename);

            Log("File size is = " + file.Length);

            fileContentLength += file.Length;

            fileContentLength += Encoding.UTF8.GetBytes("\r\n").Length;

            return fileContentLength;
        }

        private long GetContentLength(String itemID, String fileToUpload, MemoryStream contentPreamble) // throws Exception
        {
            // Leave recipients blank since PrepareSend is where recipients are specified.
            String requestBody = CreateField("rcpt", "");

            contentPreamble.Write(Encoding.UTF8.GetBytes(requestBody), 0, requestBody.Length);

            String bidStr = CreateField("bid", itemID);

            contentPreamble.Write(Encoding.UTF8.GetBytes(bidStr), 0, bidStr.Length);

            String pdfStr = CreateField("pdf_mark", "false");

            contentPreamble.Write(Encoding.UTF8.GetBytes(pdfStr), 0, pdfStr.Length);

            // Include boundary end in content length calculation.
            requestBody = requestBody + bidStr + pdfStr + BoundaryEnd + "\r\n";

            return requestBody.Length + GetFileContentLength(fileToUpload);
        }

        private void SendFile(String filename, Stream datastream, Stream fs = null, string convertedFileName = "") // throws Exception
        {
            String type = "application/octet-stream";

            String field = "--" + Boundary + "\r\n";

            convertedFileName = string.IsNullOrWhiteSpace(convertedFileName) ? filename : convertedFileName;

            field = field + "Content-Disposition: form-data; name=\"fname\"; filename=\"" + convertedFileName + "\"\r\n";

            field = field + "Content-Type: " + type + "\r\n";

            field = field + "\r\n";

            datastream.Write(Encoding.UTF8.GetBytes(field), 0, field.Length);

            if (fs == null)
            {
                fs = File.OpenRead(filename);
            }
            fs.Seek(0, SeekOrigin.Begin);
#if (NET4)
				fs.CopyTo(datastream);
#else
            CopyStream(fs, datastream);
#endif
            fs.Close();
            datastream.Write(Encoding.UTF8.GetBytes("\r\n"), 0, 2);
        }

        private String CreateField(String fieldname, String fieldvalue)
        {
            String field = "--" + Boundary + "\r\n";

            field = field + "Content-Disposition: form-data; name=\"" + fieldname + "\"\r\n";

            field = field + "\r\n" + fieldvalue + "\r\n";

            return field;
        }

        private void Log(String logMessage)
        {
#if (SHOW_LOG_MESSAGES)
				Console.WriteLine(logMessage);
#endif
        }

        private static void RequiredParameter(object parameter, String parameterName)
        {
            if (parameter == null || parameter.ToString().Trim().Length == 0)
                throw new APIException(BadRequestStatusCode, BadRequest, "Missing " + parameterName);
        }

        private void RequiredCredentials(bool authTokenRequired)
        {
            RequiredParameter(apiKey, "apiKey");

            if (authTokenRequired)
                RequiredParameter(authToken, "authToken");
        }

        private void checkResponseError(String xmlstring)
        {

            XmlNode errorStatus = GetXquery(xmlstring, "//errorStatus[1]");
            if (errorStatus != null)
            {
                XmlNode errorCode = GetXquery(xmlstring, "//errorStatus/code[1]");
                XmlNode errorMessage = GetXquery(xmlstring, "//errorStatus/message[1]");
                throw new APIException(BadRequestStatusCode, errorCode.InnerText, errorMessage.InnerText);
                // BadRequestStatusCode, BadRequest, e.Message);
            }

        }

        private String GetXqueryText(String xmlstring, String xquery)
        {

            XmlNode node = GetXquery(xmlstring, xquery);
            String value = node.InnerText;
            return value;
        }

        private XmlNode GetXquery(String xmlstring, String xquery)
        {

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlstring);

            XmlElement root = xml.DocumentElement;
            if (root != null)
            {
                XmlNode node = root.SelectSingleNode(xquery);
                return node;
            }

            return null;
        }

        private void SetServiceId(HttpWebRequest httpRequest)
        {
            if (!bypass)
            {
                return;
            }

            const string serviceId = "serviceId1";
            const string YSIServiceId = "X-Service-Id";
            httpRequest.Headers.Set(YSIServiceId, serviceId);
        }
        
    }
}
