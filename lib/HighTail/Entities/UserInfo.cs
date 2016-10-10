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
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute("user", Namespace="", IsNullable=false)]
    public class UserInfo 
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("email", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Email { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("firstname", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string FirstName { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("lastname", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string LastName { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("type", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Type { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("version", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Version { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("account", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public UserAccount[] Account { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("storage", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public UserStorage[] Storage { get; set; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlRootAttribute("userAccount", Namespace = "", IsNullable = false)]
    public class UserAccount {
    
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("knowledgeBase", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string KnowledgeBase { get; set;}
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("maxDownloadBWpermonth", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MaxDownloadBWPerMonth
        {
            get;
            set;
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("maxFileDownloads", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MaxFileDownloads
        {
            get;
            set;
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("maxFileSize", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MaxFileSize {get; set;}
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("passwordProtect", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string PasswordProtect
        {
            get;
            set;
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("returnReceipt", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ReturnReceipt
        {
            get;
            set;
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("verifyRecipientIdentity", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string VerifyRecipientIdentity
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class UserStorage {
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("currentUsage", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string CurrentUsage { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("storageQuota", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string StorageQuota { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("revision", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Revision { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("id", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
    }
}