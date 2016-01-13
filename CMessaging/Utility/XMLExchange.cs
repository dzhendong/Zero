using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using ILvYou.Zero.Config;

namespace ILvYou.Zero.Utility
{
    public class XMLExchange
    {
        #region Ctor
        private readonly List<ZeroRoute> configs = new List<ZeroRoute>();

        private static XMLExchange Scheduling = new XMLExchange();

        public static XMLExchange GetInstance()
        {
            return XMLExchange.Scheduling;
        }

        private XMLExchange()
        {
        }
        #endregion

        #region APIs
        public List<ZeroRoute> Configs
        {
            get
            {
                if (configs == null || configs.Count == 0)
                {
                    this.ProcessFile("~/zero_jobs.xml", "zero_jobs.xml");
                }

                return configs;
            }
        }

        public void LoadMapperXml()
        {
            this.ProcessFile("~/zero_jobs.xml", "zero_jobs.xml");
        }

        protected virtual void ProcessFile(string fileName, string systemId)
        {
            fileName = FileUtil.ResolveFile(fileName);
           
            using (StreamReader streamReader = new StreamReader(fileName))
            {
                this.ProcessInternal(streamReader.ReadToEnd());
            }
        }

        protected virtual void PrepForProcessing()
        {
        }

        private void ValidateXml(string xml)
        {
            try
            {
                using (StringReader stringReader = new StringReader(xml))
                {
                    XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
                    xmlReaderSettings.ValidationType = ValidationType.Schema;
                    xmlReaderSettings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
                    xmlReaderSettings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
                    xmlReaderSettings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
                    System.IO.Stream manifestResourceStream = base.GetType().Assembly.GetManifestResourceStream("zero-scheduling-data.xsd");
                    XmlSchema schema = XmlSchema.Read(manifestResourceStream, new ValidationEventHandler(this.XmlValidationCallBack));
                    xmlReaderSettings.Schemas.Add(schema);
                    xmlReaderSettings.ValidationEventHandler += new ValidationEventHandler(this.XmlValidationCallBack);
                    XmlReader xmlReader = XmlReader.Create(stringReader, xmlReaderSettings);
                    while (xmlReader.Read())
                    {
                    }
                    xmlReader.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected virtual void ProcessInternal(string xml)
        {
            //this.PrepForProcessing();
            //this.ValidateXml(xml);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ZeroXml));
            ZeroXml xmlConfiguration = (ZeroXml)xmlSerializer.Deserialize(new StringReader(xml));
            if (xmlConfiguration == null)
            {
                throw new Exception("definition data from XML was null after deserialization");
            }

            if (xmlConfiguration.exchanges != null
                && xmlConfiguration.exchanges.Length > 0
                && xmlConfiguration.exchanges[0].routings != null
                )
            {
                configs.AddRange(xmlConfiguration.exchanges[0].routings);
            }
        }

        private void XmlValidationCallBack(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Error)
            {
                return;
            }
        }
        #endregion
    }
}
