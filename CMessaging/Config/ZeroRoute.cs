using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ILvYou.Zero.Config
{
    /// <summary>
    /// 路由规则
    /// </summary>
    [Serializable]
    [GeneratedCode("xsd", "4.0.30319.1"), DesignerCategory("code"), DebuggerStepThrough,
    XmlType(Namespace = "http://ilvyou.sourceforge.net/ZeroSchedulingData")]
    public class ZeroRoute
    {
        #region Attr
        [XmlElement("name")]
        public string ExchangeName { get; set; }

        [XmlElement("appid")]
        public int AppId { get; set; }

        [XmlElement("userid")]
        public int UserId { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("maxmessagesize")]
        public int MaxMessageSize { get; set; }

        [XmlElement("sendtimeout")]
        public int SendTimeout { get; set; }

        [XmlElement("ClientID")]
        public string ClientID { get; set; }

        [XmlElement("zerotype")]
        public string ZeroType { get; set; }

        [XmlElement("identifier")]
        public string Identifier { get; set; }

        [XmlElement("clientip")]
        public string ClientIp { get; set; }

        [XmlElement("bindendpoints")]
        public string BindEndPoints { get; set; }
        #endregion

        #region Function
        public IList<string> ConnectEndPoints()
        {
            return new List<string>(
                BindEndPoints.Split(new string[] { ", " }, 
                StringSplitOptions.RemoveEmptyEntries)
            );
        }

        public List<string> SubscriptionPrefixes()
        {
            return null;
        }
        #endregion
    }
}
