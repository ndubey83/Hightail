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
    [System.Xml.Serialization.XmlRootAttribute("file", Namespace = "", IsNullable = false)]
    public class SimpleFileInfo
    {
        [System.Xml.Serialization.XmlAttributeAttribute("id")]
        public string Id
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

        [System.Xml.Serialization.XmlElementAttribute("createdOn", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string CreatedOn
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
        public int Parentid
        {
            get;
            set;
        }

        [System.Xml.Serialization.XmlElementAttribute("size", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int Size
        {
            get;
            set;
        }
    
    }
}
