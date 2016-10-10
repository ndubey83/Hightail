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

namespace YouSendIt.Entities
{
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlRootAttribute("upload", Namespace = "", IsNullable = true)]
    public class FileUploadInfo
    {

        [System.Xml.Serialization.XmlElementAttribute("fileId", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string fileId
        {
            get;
            set;
        }

        [System.Xml.Serialization.XmlElementAttribute("uploadUrl", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string uploadUrl
        {
            get;
            set;
        }
    }
}