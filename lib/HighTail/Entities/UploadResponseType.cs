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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute("ysi-response", Namespace="", IsNullable=false)]
    public class UploadResponseType {
    
        private string ufidField;
    
        private string uploadstatusField;
    
        private ysierror ysierrorField;
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="NCName")]
        public string ufid {
            get {
                return this.ufidField;
            }
            set {
                this.ufidField = value;
            }
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("upload-status", DataType="NCName")]
        public string uploadstatus {
            get {
                return this.uploadstatusField;
            }
            set {
                this.uploadstatusField = value;
            }
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ysi-error")]
        public ysierror ysierror {
            get {
                return this.ysierrorField;
            }
            set {
                this.ysierrorField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute("ysi-error", Namespace="", IsNullable=false)]
    public partial class ysierror {
    
        private string errorcodeField;
    
        private string errormessageField;
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("error-code", DataType="NCName")]
        public string errorcode {
            get {
                return this.errorcodeField;
            }
            set {
                this.errorcodeField = value;
            }
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("error-message", DataType="NCName")]
        public string errormessage {
            get {
                return this.errormessageField;
            }
            set {
                this.errormessageField = value;
            }
        }
    }
}