using NUnit.Framework;
using NextBikeDataParser;
using System.IO;
using System.Xml.Schema;
using System;

namespace NextBikeDataParser.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ValidFile()
        {
            var xmlContent = File.ReadAllText("../../../TestFile.xml");
            var markers = Parser.ReadNextBikesData(xmlContent);

            foreach (var property in typeof(markers).GetProperties())
            {
                Assert.IsNotNull(typeof(markers).GetProperty(property.Name).GetValue(markers));
            }

            foreach (var property in typeof(markersCountry).GetProperties())
            {
                Assert.IsNotNull(typeof(markersCountry).GetProperty(property.Name).GetValue(markers.country));
            }

            foreach (var property in typeof(markersCountryCity).GetProperties())
            {
                Assert.IsNotNull(typeof(markersCountryCity).GetProperty(property.Name).GetValue(markers.country.city));
            }

            foreach (var property in typeof(markersCountryCityPlace).GetProperties())
            {
                Assert.IsNotNull(typeof(markersCountryCityPlace).GetProperty(property.Name).GetValue(markers.country.city.place[0]));
            }

            foreach (var property in typeof(markersCountryCityPlaceBike).GetProperties())
            {
                Assert.IsNotNull(typeof(markersCountryCityPlaceBike).GetProperty(property.Name).GetValue(markers.country.city.place[0].bike[0]));
            }
        }

        [Test]
        public void InvalidFile()
        {
            Assert.Catch<XmlSchemaValidationException>(() =>
            {
                var markers = Parser.ReadNextBikesData("invalid content");
                Assert.IsNotNull(markers);
            });

            Assert.Catch<XmlSchemaValidationException>(() =>
            {
                var markers = Parser.ReadNextBikesData("<?xml version=\"1.0\" encoding=\"utf-8\"?><markers></markers>");
                Assert.IsNotNull(markers);
            });
        }
    }
}