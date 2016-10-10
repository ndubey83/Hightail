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

using System;
using System.Collections;
using System.Collections.Generic;

using YouSendIt.Entities;

[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlRootAttribute("item", Namespace="", IsNullable=false)]
public class ItemInfoType {
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("id", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string id { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("create", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string create { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("expiration", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string expiration { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("recipients", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string recipients { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("subject", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string subject { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("message", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string message { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("file", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public FileInfoType[] Files { get; set; }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class DownloadInfoInfoType {
    
    private string emailField;
    
    private string whenField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string email {
        get {
            return this.emailField;
        }
        set {
            this.emailField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string when {
        get {
            return this.whenField;
        }
        set {
            this.whenField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute("items", Namespace = "", IsNullable = true)]
public partial class ItemsInfo
{
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public ItemInfoType[] Item { get; set; }
}
