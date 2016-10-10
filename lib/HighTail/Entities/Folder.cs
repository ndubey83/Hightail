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
using System.Xml.Serialization;

namespace YouSendIt.Entities
{
    [System.Xml.Serialization.XmlRootAttribute("folder", Namespace = "", IsNullable = true)]
    public class Folder {

        [System.Xml.Serialization.XmlElementAttribute("createdOn", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string CreatedOn
        {
            get; 
            set;
        }

        [System.Xml.Serialization.XmlElementAttribute("fileCount", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string FileCount
        {
            get; 
            set;
        }

        [System.Xml.Serialization.XmlElementAttribute("folderCount", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string FolderCount
        {
            get;
            set;
        }

        [System.Xml.Serialization.XmlElementAttribute("name", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Name
        {
            get;
            set;
        }

        [System.Xml.Serialization.XmlElementAttribute("parentid", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Parentid
        {
            get;
            set;
        }

        [System.Xml.Serialization.XmlElementAttribute("readable", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Readable
        {
            get;
            set;
        }

        [System.Xml.Serialization.XmlElementAttribute("size", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Size
        {
            get;
            set;
        }

        [System.Xml.Serialization.XmlElementAttribute("type", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Type
        {
            get;
            set;
        }

        [System.Xml.Serialization.XmlElementAttribute("updatedOn", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string UpdatedOn
        {
            get;
            set;
        }

        [System.Xml.Serialization.XmlElementAttribute("writeable", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Writeable
        {
            get;
            set;
        }

        [System.Xml.Serialization.XmlAttributeAttribute("revision")]
        public string Revision
        {
            get;
            set;
        }

        [System.Xml.Serialization.XmlAttributeAttribute("id")]
        public int Id
        {
            get;
            set;
        }

        [System.Xml.Serialization.XmlElementAttribute("files", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public FileBase Files
        {
            get;
            set;
        }

        [System.Xml.Serialization.XmlElementAttribute("folders", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public FolderBase Folders
        {
            get;
            set;
        }
    }

    public class FileBase
    {
        [XmlElement("file", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SimpleFileInfo[] Items
        {
            get;
            set;
        }
    }

    public class FolderBase
    {
        [XmlElement("folder", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public Folder[] Items
        {
            get;
            set;
        }
    }
}