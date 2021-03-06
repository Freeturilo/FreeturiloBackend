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
        private const string libUrl = @"http://example.org/mr/nextbikesdata";
        private const string _xsdPath = @"../NextBikeDataParser/markers.xsd";
        /// <summary>
        /// Validation error handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void ValidationHandler(object sender, ValidationEventArgs args)
        {
            Console.WriteLine("Error: {0}\n", args.Message);
            _isOk = false;
        }
        /// <summary>
        /// Method checks if xml content matcg xsd validation
        /// </summary>
        /// <param name="xmlContent"></param>
        /// <param name="xsdPath"></param>
        /// <returns></returns>
        public static markers ReadNextBikesData(string xmlContent, string xsdPath)
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

            settings.Schemas.Add(libUrl, xsdPath ?? _xsdPath);
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
        /// <summary>
        /// Method to deserialize validated data
        /// </summary>
        /// <param name="xmlContent"></param>
        /// <returns></returns>
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