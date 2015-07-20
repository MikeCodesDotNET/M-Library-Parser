using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Parser.Tests
{
    [TestClass()]
    public class ParseManufacturers
    {
        [TestMethod()]
        public void ParseManufacturerData()
        {
            var path = @"..\..\..\data\manufacturers.xml";
            LiteMic.Core.Fixture.Manufacturers manufacturers = LiteMic.Parsers.Parser.ParseManufacturers(path);

            Assert.AreEqual(manufacturers.ManufacturerList.Find(x => x.ManuId == 2).ManufacturerName, "AC Lighting");
            
        }
    }
}
