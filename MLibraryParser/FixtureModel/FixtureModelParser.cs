using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using Carallon.MLibrary.Fixture;
using Carallon.MLibrary.Fixture.Enums;
using Carallon.MLibrary.Fixture.Enums.ColourMixing;
using Carallon.Helpers;
using Carallon.MLibrary.Models.Misc;
using Carallon.MLibrary.Models.Dmx;
using Carallon.MLibrary.Models;
using Carallon.MLibrary.Models.Physical.Enums;
using Carallon.MLibrary.Models.Physical;
using Carallon.MLibrary.Models.Header;
using Carallon.Parsers;
using LiteMic.FixtureModel.Header.PhysicalProperties;
using LiteMic.FixtureModel;

namespace Carallon.Parsers
{
    // \data\fixtures\generic\conventional_8_bit\conventional_8_bit.xml
    // \data\fixtures\clay_paky\goldenscan_3_8_ch\goldenscan_3_8_ch.xml
    // \data\fixtures\varilite\vl5_m3\vl5_m3.xml
    public class FixtureParser : BaseParser
    {

        /// Fixture Personalities have the following key areas that need parsing: 
        /// - Identifier Elements 
        ///     - ModelName
        ///     - Mode 
        ///     - ModeName 
        ///     - etc..
        /// - PhysicalProperties 
        /// - UserNotes
        /// - KnownIssues
        /// - TestingInfo
        /// - RevisionHistory 
        /// - DmxSpecification
        ///     - PatchGroups
        ///         - DMXMacros
        ///         - DMXChannels



        public FixtureModel ParseFixture(string filename, bool removeTestData = true)
        {
            XDocument doc = XDocument.Load(new StreamReader(filename));
            var root = doc.Element("FixtureModel");

            if (root != null)
            {
                FixtureModel fixture = ParseFixtureModel(root);
                fixture.Header.Revisions = ParseRevisions(root);

                return fixture;
            }
            else
            {
                throw new Exception($"XML file not a valid FixtureModel | {filename}");
            }
        }

        private FixtureModel ParseFixtureModel(XElement fixtureElement)
        {
            ParseResult error = new ParseResult(fixtureElement.Name.LocalName);
            FixtureModel fixture = new FixtureModel();
            FixtureHeader header = new FixtureHeader();

            string val;

            TryExecute(() =>
                {
                    val = ParseAttribute<string>(fixtureElement, "ManuId");
                    Validate(() => Validator.HasValue(val));
                    Validate(() => Validator.IsLengthBetween(3, 3, val));

                    fixture.ManuId = int.Parse(val);
                }, "ManuId", error);

            TryExecute(() =>
                {
                    val = ParseAttribute<string>(fixtureElement, "ModelId");
                    Validate(() => Validator.HasValue(val));
                    Validate(() => Validator.IsLengthBetween(3, 3, val));

                    fixture.ModelId = int.Parse(val);
                }, "ModelId", error);
            
            TryExecute(() =>
                {
                    val = ParseAttribute<string>(fixtureElement, "ModeId");
                    Validate(() => Validator.HasValue(val));
                    Validate(() => Validator.IsLengthBetween(2, 2, val));

                    fixture.ModeId = int.Parse(val);
                }, "ModeId", error);

            TryExecute(() =>
                {
                    val = ParseAttribute<string>(fixtureElement, "ModeId_2");
                    Validate(() => Validator.HasValue(val));
                    Validate(() => Validator.IsLengthBetween(2, 2, val));

                    fixture.ModeId2 = int.Parse(val);
                }, "ModeId_2", error);

            TryExecute(() =>
                {
                    val = ParseElement<string>(fixtureElement, "ModelName");
                    Validate(() => Validator.HasValue(val));                    

                    header.ModelName = val;
                }, "ModelName", error);

            TryExecute(() =>
            {
                val = ParseElement<string>(fixtureElement, "PersonalityUUID");
                Validate(() => Validator.HasValue(val));

                fixture.PersonalityUUID = val;
            }, "PersonalityUUID", error);

            TryExecute(() =>
            {
                val = ParseElement<string>(fixtureElement, "ModeName");
                Validate(() => Validator.HasValue(val));

                header.ModeName = val;
            }, "ModeName", error);

            TryExecute(() =>
            {
                val = ParseElement<string>(fixtureElement, "PersonalityUUID");
                Validate(() => Validator.HasValue(val));

                fixture.PersonalityUUID = val;
            }, "PersonalityUUID", error);

            TryExecute(() =>
            {
                val = ParseAttribute<string>(fixtureElement.GetElement("ModelName", false), "DiscontinuedModel");
                Validate(() => Validator.HasValue(val));

                if (val == "yes")
                    fixture.Discontinued = true;
                else
                    fixture.Discontinued = false;

            }, "DiscontinuedModel", error);

            TryExecute(() =>
            {
                val = ParseAttribute<string>(fixtureElement.GetElement("ModeName", false), "DefaultMode");
                Validate(() => Validator.HasValue(val));

                if (val == "yes")
                    fixture.DefaultMode = true;
                else
                    fixture.DefaultMode = false;

            }, "DiscontinuedModel", error);

            TryExecute(() =>
                {
                    val = ParseElement<string>(fixtureElement, "PersonalityName_8");
                    Validate(() => Validator.HasValue(val));
                    Validate(() => Validator.IsLengthBetween(1, 8, val));

                    header.PersonalityName = val;
                }, "PersonalityName_8", error);

            TryExecute(() =>
                {
                    val = ParseElement<string>(fixtureElement, "DocumentationVersion");
                    Validate(() => Validator.HasValue(val));

                    header.DocumentationVersion = val;
                }, "DocumentationVersion", error);

            TryExecute(() =>
            {
                val = ParseElement<string>(fixtureElement, "KnownIssues");
                header.KnownIssues = ParseList(val);
            }, "KnownIssues", error);

            TryExecute(() =>
            {
                val = ParseElement<string>(fixtureElement, "UserNotes");
                header.UserNotes = ParseList(val);
            }, "UserNotes", error);
                       

            //Parse Element Structure 
            var elementStructureParser = new ElementStructureParser();
            header.ElementStructure = elementStructureParser.Parse(fixtureElement, error);
            

            //Parse Physical Properties
            var physicalPropertiesParser = new PhyicalPropertiesParser();       
            header.PhysicalProperties = physicalPropertiesParser.Parse(fixtureElement, error);

            //Parse Testing Info
            header.TestingInfo = ParseTestingInfo(fixtureElement, error);

            //Parse RDM Spec
            fixture.RdmSpefication = ParseRDMSpecification(fixtureElement, error);

            fixture.Header = header;


            //DMX Spec!
            var dmxSpecificationParser = new DmxSpecificationParser();
            fixture.DmxSpecification = dmxSpecificationParser.Parse(fixtureElement, error);
            
            
            return fixture;
        }

        private TestingInfo ParseTestingInfo(XElement source, ParseResult result)
        {
            XElement tinfoElement = source.Element("TestingInfo");

            var testInfo = new TestingInfo();

            if (tinfoElement != null)
            {
                ParseResult error = new ParseResult(tinfoElement.Name.LocalName);
                string val;
                XElement node;

                TryExecute(
                    () =>
                    {
                        val = ParseElement<string>(tinfoElement, "DmxStatus");
                        Validate(() => Validator.HasValue(val));
                        testInfo.DmxStatus = ParseEnum<DmxStatus>(val);
                    }, "DmxStatus", error);

                TryExecute(
                    () =>
                    {
                        val = ParseElement<string>(tinfoElement, "RdmStatus");
                        Validate(() => Validator.HasValue(val));
                        testInfo.RdmStatus = ParseEnum<RdmStatus>(val);
                    }, "RdmStatus", error);

                TryExecute(
                    () =>
                    {
                        val = ParseElement<string>(tinfoElement, "FirmwareVersionTested");
                        Validate(() => Validator.HasValue(val));
                        testInfo.FirmwareVersionTested = val;
                    }, "FirmwareVersionTested", error);

                TryExecute(
                    () =>
                    {
                        val = ParseElement<string>(tinfoElement, "ColourMixStatus");
                        Validate(() => Validator.HasValue(val));
                        testInfo.ColourMixStatus = ParseEnum<ColourMixStatus>(val);
                    }, "ColourMixStatus", error);

                TryExecute(
                    () =>
                    {
                        val = ParseElement<string>(tinfoElement, "TestingNotes");
                        testInfo.TestingNotes = ParseList(val);
                    }, "TestingNotes", error);

                if (error.HasError) result.ErrorList.Add(error);
            }
            else
            {
                result.FieldsWithError["TestingInfo"] = "Undefined";
            }

            return testInfo;
        }

        private RdmSpecification ParseRDMSpecification(XElement source, ParseResult result)
        {
            XElement rdmSpecElement = source.Element("RdmSpecification");
            var rdmSpec = new RdmSpecification();

            if (rdmSpecElement != null)
            {
                ParseResult error = new ParseResult(rdmSpecElement.Name.LocalName);
                string val;
                XElement node;

                TryExecute(
                    () =>
                    {
                        val = ParseAttribute<string>(rdmSpecElement, "ESTAId");
                        Validate(() => Validator.HasValue(val));
                        rdmSpec.EstaId = int.Parse(val);
                    }, "ESTAId", error);

                TryExecute(
                    () =>
                    {
                        val = ParseAttribute<string>(rdmSpecElement, "DeviceModelId");
                        Validate(() => Validator.HasValue(val));
                        rdmSpec.DeviceModelId = int.Parse(val);
                    }, "DeviceModelId", error);               

              
                if (error.HasError) result.ErrorList.Add(error);
            }
            else
            {
                result.FieldsWithError["RdmSpecification"] = "Undefined";
            }

            return rdmSpec;
        }



    
    }
}
