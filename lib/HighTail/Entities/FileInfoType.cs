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
    [System.Xml.Serialization.XmlRootAttribute("file", Namespace = "", IsNullable = true)]
    public class FileInfoType {

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("id")]
        public string Id { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("revision")]
        public string Revision
        {
            get;
            set;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("clickableDownloadUrl", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ClickableDownloadUrl { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("parentid", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ParentId
        {
            get;
            set;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ownedByStorage", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string OwnedByStorage
        {
            get;
            set;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("createdOn", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string CreatedOn
        {
            get;
            set;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("name", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Name { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("size", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int Size { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("passwordProtect", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string PasswordProtect { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("verifyIdentity", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string VerifyIdentity { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("returnReceipt", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ReturnReceipt { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("downloads", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int Downloads { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool downloadsSpecified { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("downloadUrl", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string DownloadUrl { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("tracking", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public DownloadInfoInfoType[] Tracking { get; set; }
    }
}