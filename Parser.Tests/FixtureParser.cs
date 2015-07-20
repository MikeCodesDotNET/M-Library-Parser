using System;
using System.Linq;
using LiteMic.Core.Fixture.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Parser.Tests
{
    [TestClass()]
    public class ParseFixtureModels
    {
        #region data files for test
        // clay_paky - All tests complete
        // \data\fixtures\clay_paky\goldenscan_3_6_ch\goldenscan_3_6_ch.xml
        // \data\fixtures\clay_paky\goldenscan_3_8_ch\goldenscan_3_8_ch.xml
        // \data\fixtures\clay_paky\miniscan_hpe\miniscan_hpe.xml

        // coemar - All tests complete
        // \data\fixtures\coemar\infinity_spot_xl_8_bit_gobo\infinity_spot_xl_8_bit_gobo.xml
        // \data\fixtures\coemar\infinity_spot_xl_16_bit_gobo\infinity_spot_xl_16_bit_gobo.xml

        // chroma_q - all tests complete
        // \data\fixtures\chroma_q\colorweb_125\colorweb_125.xml

        // etc - all tests passed
        // Sample Data\data\fixtures\etc\pearl_21\pearl_21.xml
        // Sample Data\data\fixtures\etc\pearl_42\pearl_42.xml

        // generic
        // \data\fixtures\generic\conventional_8_bit\conventional_8_bit.xml
        // \data\fixtures\generic\conventional_16_bit\conventional_16_bit.xml
        // \data\fixtures\generic\led_rgba_8_bit\led_rgba_8_bit.xml
        // \data\fixtures\generic\led_rgba_16_bit\led_rgba_16_bit.xml
        // \data\fixtures\generic\led_rgbaw_16_bit\led_rgbaw_16_bit.xml
        // \data\fixtures\generic\led_rgbaw_8_bit\led_rgbaw_8_bit.xml

        // varilite
        // \data\fixtures\varilite\vl5_m3\vl5_m3.xml

        #endregion
        
        [TestMethod()]
        public void ParseFixtureTest()
        {
            
        }

        #region clay_paky

        [TestMethod]
        public void ParseFixtureClayPakyGoldenScan6ChannelTest()
        {
            // passed
            var path = @"..\..\..\data\fixtures\clay_paky\goldenscan_3_6_ch\goldenscan_3_6_ch.xml";
            LiteMic.Core.Fixture.FixtureModel fixtureModel = LiteMic.Parsers.Parser.ParseFixture(path);

            Assert.AreEqual(
                fixtureModel.DmxSpecification.PatchGroups.SelectMany(pg => pg.Channels)
                    .First(c => c.DominantFeatureGroup == FeatureGroup.Gobo_Index_Rotate)
                    .Ranges.Skip(2)
                    .First()
                    .Range.End, 222);
        }

        [TestMethod]
        public void ParseFixtureClayPakyGoldenScan8ChannelTest()
        {
            // passed
            var path = @"..\..\..\data\fixtures\clay_paky\goldenscan_3_8_ch\goldenscan_3_8_ch.xml";
            LiteMic.Core.Fixture.FixtureModel fixtureModel = LiteMic.Parsers.Parser.ParseFixture(path);

            Assert.AreEqual(
                fixtureModel.DmxSpecification.PatchGroups.SelectMany(pg => pg.Channels)
                    .First(c => c.DominantFeatureGroup == FeatureGroup.Colour_Wheel)
                    .Ranges.Skip(2)
                    .First()
                    .Range.End, 41);
        }

        [TestMethod]
        public void ParseFixtureMiniscanHpeTest()
        {
            // passed
            var path = @"..\..\..\data\fixtures\clay_paky\miniscan_hpe\miniscan_hpe.xml";
            LiteMic.Core.Fixture.FixtureModel fixtureModel = LiteMic.Parsers.Parser.ParseFixture(path);

            Assert.AreEqual(fixtureModel.Physical.FixtureMass, 16f);

            Assert.AreEqual(fixtureModel.Physical.BeamAngle.Angle, 16.6f);

            Assert.AreEqual(fixtureModel.Physical.Dimension.X, 238f);

            Assert.AreEqual(
                fixtureModel.Physical.SlotMaps.First(f => f.WheelType.HasValue && f.WheelType.Value == WheelType.Static)
                    .Slots.First(s => s.Number == 4)
                    .MediaRangeInfo.Manufacturer, "generic");


            Assert.AreEqual(
                fixtureModel.DmxSpecification.PatchGroups.First(pg => pg.PatchFootprint == 7)
                    .Macros.First()
                    .TimingSets.Last()
                    .HoldTime, 35);
        }

        #endregion        

        #region coemar

        [TestMethod]
        public void ParseFixtureInfinitySpot8BitTest()
        {
            // passed
            var path = @"..\..\..\data\fixtures\coemar\infinity_spot_xl_8_bit_gobo\infinity_spot_xl_8_bit_gobo.xml";
            LiteMic.Core.Fixture.FixtureModel fixtureModel = LiteMic.Parsers.Parser.ParseFixture(path);

            Assert.AreEqual(
                fixtureModel.DmxSpecification.PatchGroups.SelectMany(pg => pg.Macros)
                    .First(m => m.FeatureRanges.Any(f => f.FeatureName == FeatureName.Position_Reset))
                    .TimingSets.Select(ts => ts.ChannelSetting)
                    .ElementAt(1)
                    .DmxValueRange.End, 65);

            Assert.AreEqual(fixtureModel.Physical.SlotMaps.First().Slots.First(s => s.Number == 5).MediaRangeInfo.Manufacturer, "coemar");

            Assert.AreEqual(fixtureModel.Physical.ValueMaps.ElementAt(3).FeatureName, FeatureName.Lamp_Power);
            
            Assert.AreEqual(
                fixtureModel.DmxSpecification.PatchGroups.SelectMany(pg => pg.Channels)
                    .First(m => m.DominantFeatureGroup == FeatureGroup.Shutter_Strobe)
                    .Ranges.ElementAt(1)
                    .FeatureRange.First().FeatureName, FeatureName.Strobe);

            Assert.AreEqual(fixtureModel.DmxSpecification.PatchGroups.SelectMany(pg => pg.Channels).First(c => c.ChannelNum.Contains(8)).Ranges.ElementAt(3).Range.End, 189);


            Assert.AreEqual(
                fixtureModel.DmxSpecification.PatchGroups.SelectMany(pg => pg.Channels)
                    .First(c => c.ChannelNum.Contains(14))
                    .Ranges.First(r => r.Range.End == 40)
                    .FeatureRange.First()
                    .FeatureName, FeatureName.Gobo_Wheel_Select);
        }

        [TestMethod]
        public void ParseFixtureInfinitySpot16BitTest()
        {
            // passed
            var path = @"..\..\..\data\fixtures\coemar\infinity_spot_xl_16_bit_gobo\infinity_spot_xl_16_bit_gobo.xml";
            LiteMic.Core.Fixture.FixtureModel fixtureModel = LiteMic.Parsers.Parser.ParseFixture(path);

            Assert.AreEqual(
                fixtureModel.Physical.SlotMaps.First(f => f.WheelType.HasValue && f.WheelType == WheelType.Rotating)
                    .Slots.First(s => s.Number == 6)
                    .MediaName, "G0201");

            Assert.AreEqual(
                fixtureModel.Physical.ValueMaps.First(f => f.UnitName.HasValue && f.UnitName == UnitType.Watts)
                    .Specs.Last()
                    .UnitValue, 1500);

            Assert.AreEqual(
                fixtureModel.DmxSpecification.PatchGroups.SelectMany(pg => pg.Macros)
                    .ElementAt(3)
                    .TimingSets.First(t=> t.HoldTime == 30)
                    .ChannelSetting.DmxValueRange.Start, 101);

            Assert.AreEqual(
                fixtureModel.DmxSpecification.PatchGroups.SelectMany(pg => pg.Channels)
                    .First(m => m.DominantFeatureGroup == FeatureGroup.Tilt)
                    .Ranges.ElementAt(0)
                    .FeatureRange.First().FeatureName, FeatureName.Tilt);

            Assert.AreEqual(
                fixtureModel.DmxSpecification.PatchGroups.SelectMany(pg => pg.Channels)
                    .First(c => c.ChannelNum.Contains(5))
                    .Ranges.ElementAt(2)
                    .FeatureRange.First()
                    .FeatureName, FeatureName.Position_MSpeed_Track);


            Assert.AreEqual(
                fixtureModel.DmxSpecification.PatchGroups.SelectMany(pg => pg.Channels)
                    .First(c => c.ChannelNum.Contains(11))
                    .Ranges.First(r => r.Range.End == 70).ConditionalRangeSet.RangeSetCondition.ChannelNum
                , 29);
        }

        #endregion

        #region chroma_q

        [TestMethod]
        public void ParseFixtureChromaQColorWeb125Test()
        {
            // passed 
            var path = @"..\..\..\data\fixtures\chroma_q\colorweb_125\colorweb_125.xml";
            LiteMic.Core.Fixture.FixtureModel fixtureModel = LiteMic.Parsers.Parser.ParseFixture(path);

            Assert.AreEqual(fixtureModel.DmxSpecification.ModuleDefinitions.SelectMany(md => md.Channels).First(c => c.DominantFeatureGroup == FeatureGroup.Green).Ranges.First().FeatureRange.First().FeatureName, FeatureName.Green);

            Assert.AreEqual(fixtureModel.DmxSpecification.PatchGroups.First().Channels.First(c => c.ElementNumber == 5).Modules.First().Name, "RGB module");
        }

        #endregion

        #region etc

        [TestMethod]
        public void ParseFixtureEtcPearl21LedTest()
        {
            // passed
            var path = @"..\..\..\data\fixtures\etc\pearl_21\pearl_21.xml";
            LiteMic.Core.Fixture.FixtureModel fixtureModel = LiteMic.Parsers.Parser.ParseFixture(path);

            Assert.AreEqual(fixtureModel.Physical.CompoundStructure.GeometryYCount, 1);

            Assert.AreEqual(fixtureModel.DmxSpecification.ModuleDefinitions.SelectMany(md => md.Channels).First(c => c.ChannelNum.Contains(2)).Ranges.First().FeatureRange.First().FeatureName, FeatureName.Cool_White);

            Assert.AreEqual(fixtureModel.DmxSpecification.PatchGroups.First(pg => pg.PatchFootprint == 3).Channels.First().Modules.First().Name, "WW-CW-I Cluster");
        }

        [TestMethod]
        public void ParseFixtureEtcPearl42LedTest()
        {
            // passed
            var path = @"..\..\..\data\fixtures\etc\pearl_42\pearl_42.xml";
            LiteMic.Core.Fixture.FixtureModel fixtureModel = LiteMic.Parsers.Parser.ParseFixture(path);

            Assert.AreEqual(fixtureModel.Physical.CompoundStructure.CellSizeX, 270);

            Assert.AreEqual(fixtureModel.DmxSpecification.ModuleDefinitions.SelectMany(md => md.Channels).First(c => c.ChannelNum.Contains(1)).Ranges.First().FeatureRange.First().FeatureName, FeatureName.Warm_White);

            Assert.AreEqual(fixtureModel.DmxSpecification.PatchGroups.First(pg => pg.PatchGroupLabel == "Part#2").Channels.First().Modules.First().Name, "WW-CW-I Cluster");
        }

        #endregion

        #region varilite

        [TestMethod]
        public void ParseFixtureVariliteVl5Mode3Test()
        {
            var path = @"..\..\..\data\fixtures\varilite\vl5_m3\vl5_m3.xml";
            LiteMic.Core.Fixture.FixtureModel fixtureModel = LiteMic.Parsers.Parser.ParseFixture(path);

            // TODO - write test case
        }

        #endregion        

        #region generic

        [TestMethod]
        public void ParseFixtureGeneric8BitDimmerTest()
        {
            // passed
            var path = @"..\..\..\data\fixtures\generic\conventional_8_bit\conventional_8_bit.xml";
            LiteMic.Core.Fixture.FixtureModel fixtureModel = LiteMic.Parsers.Parser.ParseFixture(path);

            Assert.AreEqual(
                fixtureModel.DmxSpecification.PatchGroups.SelectMany(pg => pg.Channels.SelectMany(c => c.Ranges))
                    .First()
                    .Range.End, 255);
        }

        [TestMethod]
        public void ParseFixtureGeneric16BitDimmerTest()
        {
            // passed
            var path = @"..\..\..\data\fixtures\generic\conventional_16_bit\conventional_16_bit.xml";
            LiteMic.Core.Fixture.FixtureModel fixtureModel = LiteMic.Parsers.Parser.ParseFixture(path);

            Assert.AreEqual(
                fixtureModel.DmxSpecification.PatchGroups.SelectMany(pg => pg.Channels.SelectMany(c => c.Ranges))
                    .First()
                    .Range.End, 65535);
        }

        [TestMethod]
        public void ParseFixtureGenericLedRgba8BitDimmerTest()
        {
            // passed
            var path = @"..\..\..\data\fixtures\generic\led_rgba_8_bit\led_rgba_8_bit.xml";
            LiteMic.Core.Fixture.FixtureModel fixtureModel = LiteMic.Parsers.Parser.ParseFixture(path);

            Assert.AreEqual(
                fixtureModel.DmxSpecification.ModuleDefinitions.First(md => md.ModuleFootprint == 4)
                    .Channels.Last()
                    .DominantFeatureGroup, FeatureGroup.Amber);

            Assert.AreEqual(
                fixtureModel.DmxSpecification.PatchGroups.SelectMany(pg => pg.Channels.SelectMany(c => c.Modules))
                    .First().Name, "RGBA Cluster");
        }

        [TestMethod]
        public void ParseFixtureGenericLedRgbs16BitDimmerTest()
        {
            // passed
            var path = @"..\..\..\data\fixtures\generic\led_rgba_16_bit\led_rgba_16_bit.xml";
            LiteMic.Core.Fixture.FixtureModel fixtureModel = LiteMic.Parsers.Parser.ParseFixture(path);

            Assert.AreEqual(
                fixtureModel.DmxSpecification.ModuleDefinitions.First(md => md.ModuleFootprint == 8)
                    .Channels.Skip(2).First().DominantFeatureGroup, FeatureGroup.Blue);

            Assert.AreEqual(
                fixtureModel.DmxSpecification.PatchGroups.SelectMany(pg => pg.Channels.SelectMany(c => c.Modules))
                    .First().Name, "RGBA Cluster");
        }


        [TestMethod]
        public void ParseFixtureGenericLedRgbaw8BitTest()
        {
            // passed
            var path = @"..\..\..\data\fixtures\generic\led_rgbaw_8_bit\led_rgbaw_8_bit.xml";
            LiteMic.Core.Fixture.FixtureModel fixtureModel = LiteMic.Parsers.Parser.ParseFixture(path);

            Assert.AreEqual(fixtureModel.DmxSpecification.ModuleDefinitions.SelectMany(md => md.Channels).First(c => c.ChannelNum.Contains(3)).Ranges.First().FeatureRange.First().FeatureName,FeatureName.Blue);

            Assert.AreEqual(fixtureModel.DmxSpecification.PatchGroups.First().Channels.First().Modules.First().Name, "RGBAW Cluster");
        }

        [TestMethod]
        public void ParseFixtureGenericLedRgbaw16BitTest()
        {
            // passed
            var path = @"..\..\..\data\fixtures\generic\led_rgbaw_16_bit\led_rgbaw_16_bit.xml";
            LiteMic.Core.Fixture.FixtureModel fixtureModel = LiteMic.Parsers.Parser.ParseFixture(path);
            
            Assert.AreEqual(fixtureModel.DmxSpecification.ModuleDefinitions.SelectMany(md => md.Channels).First(c => c.DominantFeatureGroup == FeatureGroup.Green).Ranges.First().FeatureRange.First().FeatureName, FeatureName.Green);

            Assert.AreEqual(fixtureModel.DmxSpecification.PatchGroups.First().Channels.First().Modules.First().Name, "RGBAW Cluster");
        }        
        
        #endregion                

        
    }
}
