﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=4.6.1055.0.
// 

namespace XmlData
{

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public class Messages {
    
    private IList<MessageData> items;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("MessageData", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public IList<MessageData> Items {
        get {
            return this.items;
        }
        set {
            this.items = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public class MessageData {
    
    private string id;
    
    private string type;
    
    private string description;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string Id {
        get {
            return this.id;
        }
        set {
            this.id = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string Type {
        get {
            return this.type;
        }
        set {
            this.type = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string Description {
        get {
            return this.description;
        }
        set {
            this.description = value;
        }
    }
}
}