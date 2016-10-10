
namespace YouSendIt.Entities
{
    [System.Xml.Serialization.XmlRootAttribute("file", Namespace = "", IsNullable = true)]
    public class SendFileInfo {

        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string status { get; set; }
    }
}
