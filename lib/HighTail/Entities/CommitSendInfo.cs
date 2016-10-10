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
    [System.Xml.Serialization.XmlRootAttribute("commit", Namespace = "", IsNullable = true)]
    public class CommitSendInfo {

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("downloadUrl", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string[] downloadUrl { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("errorStatus", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CommitErrorStatus errorStatus { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class CommitErrorStatus {
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string code { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string message { get; set; }
    }
}