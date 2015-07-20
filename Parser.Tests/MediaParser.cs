using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Parser.Tests
{
    [TestClass]
    public class MediaParser
    {
        [TestMethod()]
        public void ParseMediaData()
        {
            var path = @"..\..\..\data\gobos\clay_paky\catalogue_0813.xml";
            LiteMic.Core.Fixture.MediaRange mediaRange = LiteMic.Parsers.Parser.ParseMediaRange(path);

            Assert.AreEqual(mediaRange.Media.Skip(1).First().Name, "02");

            Assert.AreEqual(mediaRange.Media.Find(m => m.Id == "13").ImageName, "00590113.png");

        }
    }
}
