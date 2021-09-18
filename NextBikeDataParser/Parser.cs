using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NextBikeDataParser
{
    public static class Parser
    {
        private static bool _isOk = true;
        private static void ValidationHandler(object sender, ValidationEventArgs args)
        {
            Console.WriteLine(args.Severity == XmlSeverityType.Warning ? "Warning: {0}\n" : "Error: {0}\n",
                args.Message);
            _isOk = false;
        }

        public static markers ReadNextBikesData(string xmlContent, string xsdPath, string libUrl)
        {
            var settings = new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema
            };

            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;

            settings.Schemas.Add(libUrl, xsdPath);
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationEventHandler += ValidationHandler;

            var byteArray = Encoding.ASCII.GetBytes(xmlContent);
            var ms = new MemoryStream(byteArray);
            var reader = XmlReader.Create(ms, settings);
            while (reader.Read());
            reader.Close();

            if (_isOk)
                return ReadNextBikesData(xmlContent);
            else
                throw new XmlSchemaValidationException("Invalid XML file");
        }

        private static markers ReadNextBikesData(string xmlContent)
        {
            var byteArray = Encoding.ASCII.GetBytes(xmlContent);
            var ms = new MemoryStream(byteArray);
            var xs = new XmlSerializer(typeof(markers));
            var pas = (markers)xs.Deserialize(ms);
            ms.Close();
            
            return pas;
        }
    }
}