using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Parser.Tests
{
    [TestClass()]
    public class ParseManufacturers : TestBase
    {
        [TestMethod()]
        public void ParseManufacturerData()
        {
            var path = $@"{Root}data\manufacturers.xml";
            Carallon.MLibrary.Fixture.Manufacturers manufacturers = Carallon.Parsers.Parser.ParseManufacturers(path);

            Assert.AreEqual(manufacturers.ManufacturerList.Find(x => x.ManuId == 2).ManufacturerName, "AC Lighting");
            
        }
    }
}
