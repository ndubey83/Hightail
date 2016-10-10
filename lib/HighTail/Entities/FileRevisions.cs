using System.Xml.Serialization;

namespace YouSendIt.Entities
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute("revision", Namespace = "", IsNullable = false)]
    public class FileRevision {

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("current", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Current { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("downloadUrl", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string DownloadUrl { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("externalId", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ExternalId { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("size", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Size { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("protocolVersion")]
        public string ProtocolVersion { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("id")]
        public string Id { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute("file", Namespace="", IsNullable=true)]
    public class FileRevisionInfo {

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("status", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Status { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("createdOn", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string CreatedOn { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("name", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Name { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ownedByStorage", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string OwnedByStorage { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("parentid", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ParentId { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("size", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Size { get; set; }

       [System.Xml.Serialization.XmlElementAttribute("revisions", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public FileRevisionBase Revisions
        {
            get;
            set;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("revision")]
        public string Revision { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("id")]
        public string Id { get; set; }
    }

    
    public class FileRevisionBase
    {
        [XmlElement("revision", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public FileRevision[] Revisions
        {
            get;
            set;
        }
    }
    
}