using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Parser.Tests
{
    [TestClass]
    public class MediaParser : TestBase 
    {

        MediaParser mediaParser = new MediaParser();
        /*
        [TestMethod()]
        public void ParseMediaData()
        {
            var path = $@"{Root}data\gobos\clay_paky\catalogue_0813.xml";
            Carallon.MLibrary.Fixture.MediaRange mediaRange = mediaParser.p(path);

            Assert.AreEqual(mediaRange.Media.Skip(1).First().Name, "02");

            Assert.AreEqual(mediaRange.Media.Find(m => m.cId == "13").ImageName, "00590113.png");

        }
        */
    }
}
