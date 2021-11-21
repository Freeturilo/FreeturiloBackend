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
        private static readonly string xsdPath = @"../../../../NextBikeDataParser/markers.xsd";
        private const string libUrl = @"http://example.org/mr/nextbikesdata";
        private static void ValidationHandler(object sender, ValidationEventArgs args)
        {
            Console.WriteLine("Error: {0}\n", args.Message);
            _isOk = false;
        }

        public static markers ReadNextBikesData(string xmlContent)
        {
            _isOk = true;

            var xmlContentSplit = xmlContent.Split(new string[] { "markers" }, StringSplitOptions.None);
            if(xmlContentSplit.Length < 3) throw new XmlSchemaValidationException("Invalid XML file");
            var xmlContentJoined = xmlContentSplit[0] + $"markers xmlns=\"{libUrl}\"" + xmlContentSplit[1] +
                                    "markers" +
                                    xmlContentSplit[2];

            var settings = new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema
            };

            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;

            settings.Schemas.Add(libUrl, xsdPath);
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationEventHandler += ValidationHandler;

            var byteArray = Encoding.ASCII.GetBytes(xmlContentJoined);
            var ms = new MemoryStream(byteArray);
            var reader = XmlReader.Create(ms, settings);
            while (reader.Read());
            reader.Close();

            if (_isOk)
                return ReadNextBikesValidatedData(xmlContentJoined);
            else
                throw new XmlSchemaValidationException("Invalid XML file");
        }

        private static markers ReadNextBikesValidatedData(string xmlContent)
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