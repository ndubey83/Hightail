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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("policy", Namespace="", IsNullable=false)]
    public class PolicyInfoType {
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("fileExpiration", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string FileExpiration { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("fileExpirationOverwrite", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string FileExpirationOverwrite { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string PasswordProtect { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("passwordProtectOverwrite", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string PasswordProtectOverwrite { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("returnReceipt", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ReturnReceipt { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("returnReceiptOverwrite", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ReturnReceiptOverwrite { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("vri", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Vri { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("vriOverwrite", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string VriOverwrite { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("blackListedDomains", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string BlackListedDomains { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("whiteListedDomains", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string WhiteListedDomains { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("vriOverwriteShare", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string VriOverwriteShare
        {
            get;
            set;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("vriShare", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string VriShare
        {
            get;
            set;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("blackListedDomainsShare", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string BlackListedDomainsShare
        {
            get;
            set;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("whiteListedDomainsShare", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string WhiteListedDomainsShare
        {
            get;
            set;
        }
    }
}
