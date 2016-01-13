using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ILvYou.Zero.Config
{
    [Serializable]
    [GeneratedCode("xsd", "4.0.30319.1"), DesignerCategory("code"), DebuggerStepThrough,
    XmlRoot("zero-scheduling-data", Namespace = "http://ilvyou.sourceforge.net/ZeroSchedulingData", IsNullable = false),
    XmlType(AnonymousType = true, Namespace = "http://ilvyou.sourceforge.net/ZeroSchedulingData")]
    public class ZeroXml
    {
        [XmlAttribute]
        public string version { get; set; }

        [XmlElement("exchange")]
        public ZeroExchange[] exchanges { get; set; }
    }

    [Serializable]
    [GeneratedCode("xsd", "4.0.30319.1"), DesignerCategory("code"), DebuggerStepThrough,
    XmlType(AnonymousType = true, Namespace = "http://ilvyou.sourceforge.net/ZeroSchedulingData")]
    public class ZeroExchange
    {
        [XmlElement("routing")]
        public ZeroRoute[] routings { get; set; }
    }
}
