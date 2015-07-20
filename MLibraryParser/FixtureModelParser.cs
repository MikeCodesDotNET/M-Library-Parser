using LiteMic.Core.Fixture;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using LiteMic.Helpers;
using System.Globalization;
using LiteMic.Core.Fixture.Enums;
using LiteMic.Core.Fixture.Enums.ColourMixing;

namespace LiteMic.Parsers
{
    // \data\fixtures\generic\conventional_8_bit\conventional_8_bit.xml
    // \data\fixtures\clay_paky\goldenscan_3_8_ch\goldenscan_3_8_ch.xml
    // \data\fixtures\varilite\vl5_m3\vl5_m3.xml
    public partial class Parser
    {
        public static FixtureModel ParseFixture(string filename, bool removeTestData = true)
        {
            XDocument doc = XDocument.Load(new StreamReader(filename));
            var root = doc.Element("FixtureModel");

            FixtureModel fixture = ParseFixtureModel(root);
            ParseRevisions(root, fixture);

            return fixture;
        }

        private static FixtureModel ParseFixtureModel(XElement fixtureElement)
        {
            ParseResult error = new ParseResult(fixtureElement.Name.LocalName);
            FixtureModel fixture = new FixtureModel();

            string val;

            TryExecute(
                () =>
                {
                    val = ParseAttribute<string>(fixtureElement, "ManuId");
                    Validate(() => Validator.HasValue(val));
                    Validate(() => Validator.IsLengthBetween(3, 3, val));

                    fixture.ManuId = int.Parse(val);
                }, "ManuId", error);

            TryExecute(
                () =>
                {
                    val = ParseAttribute<string>(fixtureElement, "ModelId");
                    Validate(() => Validator.HasValue(val));
                    Validate(() => Validator.IsLengthBetween(3, 3, val));

                    fixture.ModelId = int.Parse(val);
                }, "ModelId", error);

            
            TryExecute(
                () =>
                {
                    val = ParseAttribute<string>(fixtureElement, "ModeId");
                    Validate(() => Validator.HasValue(val));
                    Validate(() => Validator.IsLengthBetween(2, 2, val));

                    fixture.ModeId = int.Parse(val);
                }, "ModeId", error);

            TryExecute(
                () =>
                {
                    val = ParseAttribute<string>(fixtureElement, "ModeId_2");
                    Validate(() => Validator.HasValue(val));
                    Validate(() => Validator.IsLengthBetween(2, 2, val));

                    fixture.ModeId2 = int.Parse(val);
                }, "ModeId_2", error);

            TryExecute(
                () =>
                {
                    val = ParseElement<string>(fixtureElement, "ModelName");
                    Validate(() => Validator.HasValue(val));                    

                    fixture.Name = val;
                }, "ModelName", error);
            
            TryExecute(
                () =>
                {
                    val = ParseElement<string>(fixtureElement, "ModeName");
                    Validate(() => Validator.HasValue(val));

                    fixture.ModeName = val;
                }, "ModeName", error);

            TryExecute(
                () =>
                {
                    val = ParseElement<string>(fixtureElement, "PersonalityName_8");
                    Validate(() => Validator.HasValue(val));
                    Validate(() => Validator.IsLengthBetween(1, 8, val));

                    fixture.PersonalityName = val;
                }, "PersonalityName_8", error);

            TryExecute(
                () =>
                {
                    val = ParseElement<string>(fixtureElement, "DocumentationVersion");
                    Validate(() => Validator.HasValue(val));

                    fixture.DocumentationVersion = val;
                }, "DocumentationVersion", error);

            TryExecute(
                () =>
                {
                    val = ParseElement<string>(fixtureElement, "UserNotes");
                    fixture.UserNotes = val;
                }, "UserNotes", error);

            TryExecute(
                () =>
                {
                    val = ParseElement<string>(fixtureElement, "KnownIssues");
                    fixture.KnownIssues = ParseList(val);
                }, "KnownIssues", error);

            fixture.Physical = ParsePhysicalProperties(fixtureElement, error);
            fixture.TestingInfo = ParseTestingInfo(fixtureElement, error);

            fixture.DmxSpecification = ParseDmxSpecification(fixtureElement, error);
            fixture.RdmSpefication = ParseRdmSpecification(fixtureElement, error);
            
            fixture.Error = error;

            return fixture;
        }

        private static PhysicalProperties ParsePhysicalProperties(XElement source, ParseResult result)
        {
            XElement propertiesElement = source.Element("PhysicalProperties");

            var properties = new PhysicalProperties();

            if (propertiesElement != null)
            {
                ParseResult error = new ParseResult(propertiesElement.Name.LocalName);
                string val;
                XElement node;

                TryExecute(
                    () =>
                    {
                        val = ParseElement<string>(propertiesElement, "PhysicalType");
                        Validate(() => Validator.HasValue(val));

                        properties.Type = ParseEnum<PhysicalType>(val);
                    }, "PhysicalType", error);

                TryExecute(
                    () =>
                    {
                        val = ParseElement<string>(propertiesElement, "LightGeneration");
                        Validate(() => Validator.HasValue(val));

                        properties.LightGeneration = ParseEnum<LightGeneration>(val);
                    }, "LightGeneration", error);

                TryExecute(
                    () =>
                    {
                        val = ParseElement<string>(propertiesElement, "MovementType");
                        Validate(() => Validator.HasValue(val));

                        properties.MovementType = ParseEnum<MovementType>(val);
                    }, "MovementType", error);

                TryExecute(
                    () =>
                    {
                        val = ParseElement<string>(propertiesElement, "BeamType");
                        Validate(() => Validator.HasValue(val));

                        properties.BeamType = ParseEnum<BeamType>(val);
                    }, "BeamType", error);

                TryExecute(
                    () =>
                    {
                        if (!propertiesElement.HaveElement("ColourMixing")) return;

                        node = propertiesElement.GetElement("ColourMixing", false);

                        TryExecute(
                            () =>
                            {
                                val = ParseAttribute<string>(node, "MixingType");
                                Validate(() => Validator.HasValue(val));

                                properties.ColourMixingType = ParseEnum<MixingType>(val);
                            }, "MixingType", error);
                    }, "ColourMixing", error);

                TryExecute(
                    () =>
                    {
                        if (propertiesElement.HaveElement("CompoundStructure"))
                            properties.CompoundStructure = ParseCompoundSturcture(propertiesElement, error);
                    }, "CompoundStructure", error);

                TryExecute(
                    () =>
                    {
                        if (propertiesElement.HaveElement("FixtureDimensions"))
                            properties.Dimension = ParserFixtureDimension(propertiesElement, error);
                    }, "FixtureDimensions", error);

                TryExecute(
                    () =>
                    {
                        if (propertiesElement.HaveElement("FixtureMass"))
                            properties.FixtureMass = ParseValue<float>(propertiesElement.GetElementValue("FixtureMass"));
                    }, "FixtureMass", error);

                TryExecute(
                    () =>
                    {
                        if (propertiesElement.HaveElement("FixtureBeamAngle"))
                            properties.BeamAngle = ParserFixtureBeamAngle(propertiesElement, error);
                    }, "FixtureBeamAngle", error);

                node = propertiesElement.GetElement("ValueMap", false);
                if (node != null)
                    properties.ValueMaps = ParseFeature(node, error);

                node = propertiesElement.GetElement("SlotMap", false);
                if (node != null)
                    properties.SlotMaps = ParseFeature(node, error);

                if(error.HaveError) result.ErrorList.Add(error);

            }
            else
            {
                result.FieldsWithError["PhysicalProperties"] = "Undefined";
            }

            return properties;
        }

        private static CompoundStructure ParseCompoundSturcture(XElement source, IParseResult result)
        {
            XElement structureElement = source.Element("CompoundStructure");

            if (structureElement == null) return null;

            ParseResult error = new ParseResult(structureElement.Name.LocalName);

            CompoundStructure structure = new CompoundStructure();

            TryExecute(
                () =>
                {
                    XElement node;
                    string val;

                    if(structureElement.HaveElement("CellGeometry"))
                    {
                        node = structureElement.GetElement("CellGeometry", false);
                        TryExecute(
                            () =>
                            {
                                val = ParseAttribute<string>(node, "XCount");
                                Validate(() => Validator.HasValue(val));
                                structure.GeometryXCount = ParseValue<int>(val);
                            }, "XCount", error);

                        TryExecute(
                            () =>
                            {
                                val = ParseAttribute<string>(node, "YCount");
                                Validate(() => Validator.HasValue(val));
                                structure.GeometryYCount = ParseValue<int>(val);
                            }, "YCount", error);
                    }

                    if (structureElement.HaveElement("CellSize"))
                    {
                        node = structureElement.GetElement("CellSize", false);
                        TryExecute(
                            () =>
                            {
                                val = ParseAttribute<string>(node, "XSize");
                                Validate(() => Validator.HasValue(val));
                                structure.CellSizeX = ParseValue<int>(val);
                            }, "XSize", error);

                        TryExecute(
                            () =>
                            {
                                val = ParseAttribute<string>(node, "YSize");
                                Validate(() => Validator.HasValue(val));
                                structure.CellSizeY = ParseValue<int>(val);
                            }, "YSize", error);
                    }

                    if (structureElement.HaveElement("CellShape"))
                    {
                        TryExecute(
                            () =>
                            {
                                val = ParseElement<string>(structureElement, "CellShape");
                                Validate(() => Validator.HasValue(val));
                                structure.CellShape = ParseEnum<CellType>(val);
                            }, "CellShape", error);
                    }

                }, "CompoundStructure", error);

            if (error.HaveError) result.ErrorList.Add(error);

            return structure;
        }

        private static TestingInfo ParseTestingInfo(XElement source, ParseResult result)
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

                if (error.HaveError) result.ErrorList.Add(error);
            }
            else
            {
                result.FieldsWithError["TestingInfo"] = "Undefined";
            }

            return testInfo;
        }

        private static DmxSpecification ParseDmxSpecification(XElement source, IParseResult result)
        {
            XElement dmxSpecElement = source.Element("DmxSpecification");

            var dmxSpec = new DmxSpecification();

            if (dmxSpecElement != null)
            {
                ParseResult error = new ParseResult(dmxSpecElement.Name.LocalName);
                string val;
                
                TryExecute(
                    () =>
                    {
                        val = ParseAttribute<string>(dmxSpecElement, "DmxFootprint");
                        Validate(() => Validator.HasValue(val));
                        dmxSpec.DmxFootprint = ParseValue<int>(val);
                    }, "DmxFootprint", error);

                TryExecute(
                    () =>
                    {
                        if (dmxSpecElement.HaveElement("DmxPatchgroup"))
                            dmxSpec.PatchGroups = ParseDmxPatchgroup(dmxSpecElement, error);
                    }, "DmxPatchgroup", error);

                TryExecute(
                    () =>
                    {
                        if (dmxSpecElement.HaveElement("DmxModuleDefinitions"))
                            dmxSpec.ModuleDefinitions = ParseDmxModuleDefinition(dmxSpecElement.Element("DmxModuleDefinitions"), error);
                    }, "DmxModuleDefinitions", error);

                if(error.HaveError) result.ErrorList.Add(error);
            }

            return dmxSpec;
        }

        private static List<DmxModuleDefinition> ParseDmxModuleDefinition(XElement source, IParseResult result)
        {
            List<DmxModuleDefinition> moduleDefinitions = new List<DmxModuleDefinition>();

            var moduleElements = source.Elements("DmxModuleDefinition");

            if (moduleElements.Any())
            {
                ParseResult error;
                string val;
                foreach (XElement moduleElement in moduleElements)
                {
                    error = new ParseResult(moduleElement.Name.LocalName);
                    var module = new DmxModuleDefinition();

                    TryExecute(
                        () =>
                        {
                            val = ParseAttribute<string>(moduleElement, "DmxModuleName");
                            Validate(() => Validator.HasValue(val));
                            module.ModuleName = val;
                        }, "DmxModuleName", error);

                    TryExecute(
                        () =>
                        {
                            val = ParseAttribute<string>(moduleElement, "DmxModuleFootprint");
                            Validate(() => Validator.HasValue(val));
                            module.ModuleFootprint = ParseValue<int>(val);
                        }, "DmxModuleFootprint", error);

                    TryExecute(
                        () =>
                        {
                            module.Channels = ParseDmxChannel(moduleElement, error);
                        }, "DmxChannel", error);

                    if (error.FieldsWithError.Count == 0)
                        moduleDefinitions.Add(module);

                    if (error.HaveError) result.ErrorList.Add(error);
                }
            }

            return moduleDefinitions;
        }

        private static List<DmxPatchGroup> ParseDmxPatchgroup(XElement source, IParseResult result)
        {
            var patchGroups = source.Elements("DmxPatchgroup");

            List<DmxPatchGroup> groups = new List<DmxPatchGroup>();

            if (patchGroups.Any())
            {
                foreach (XElement dmxpgroupElement in patchGroups)
                {
                    var dmxpgroup = new DmxPatchGroup();

                    ParseResult error = new ParseResult(dmxpgroupElement.Name.LocalName);
                    string val;
                    
                    TryExecute(
                        () =>
                        {
                            val = ParseAttribute<string>(dmxpgroupElement, "PatchFootprint");
                            Validate(() => Validator.HasValue(val));
                            dmxpgroup.PatchFootprint = ParseValue<int>(val);
                        }, "PatchFootprint", error);

                    TryExecute(
                        () =>
                        {
                            val = ParseAttribute<string>(dmxpgroupElement, "PatchGroupLabel");                            
                            dmxpgroup.PatchGroupLabel = val;
                        }, "PatchGroupLabel", error);

                    TryExecute(
                        () =>
                        {
                            dmxpgroup.Channels = ParseDmxChannel(dmxpgroupElement, error);
                        }, "DmxChannel", error);

                    TryExecute(
                        () =>
                        {
                            var node = dmxpgroupElement.GetElement("DmxMacros", false);
                            if (node != null)
                                dmxpgroup.Macros = ParseDmxMacro(node, error);
                        }, "DmxMacros", error);

                    if(error.FieldsWithError.Count == 0)
                        groups.Add(dmxpgroup);

                    if (error.HaveError) result.ErrorList.Add(error);
                }
            }

            return groups;
        }

        private static List<DmxMacro> ParseDmxMacro(XElement source, IParseResult result)
        {
            List<DmxMacro> macros = new List<DmxMacro>();

            var mocroElements = source.Elements("DmxMacro");

            if (mocroElements.Any())
            {
                foreach (XElement macroElement in mocroElements)
                {
                    var macro = new DmxMacro();
                    ParseResult error = new ParseResult(macroElement.Name.LocalName);

                    TryExecute(
                        () =>
                        {
                            macro.FeatureRanges = ParseFeatureRange(macroElement, error);
                        }, "FeatureRange", error);

                    TryExecute(
                        () =>
                        {
                            macro.TimingSets = ParseDmxTimingSet(macroElement, error);
                        }, "DmxTimingSet", error);

                    if (error.FieldsWithError.Count == 0)
                        macros.Add(macro);

                    if (error.HaveError) result.ErrorList.Add(error);
                }
            }

            return macros;
        }

        private static List<DmxChannel> ParseDmxChannel(XElement source, IParseResult result)
        {
            List<DmxChannel> channels = new List<DmxChannel>();

            var channelElements = source.Elements("DmxChannel");

            if (channelElements.Any())
            {
                foreach (XElement dmxChannelElement in channelElements)
                {
                    var dmxChannel = new DmxChannel();

                    ParseResult error = new ParseResult(dmxChannelElement.Name.LocalName);
                    string val;
                    
                    TryExecute(
                        () =>
                        {
                            val = ParseAttribute<string>(dmxChannelElement, "ChannelNum");
                            Validate(() => Validator.HasValue(val));

                            if (HaveRangeValue(val))
                            {
                                dmxChannel.ChanelNumRange = ParseRangeValue(val);                                
                            }
                            else
                            {
                                dmxChannel.ChannelNum = ParseDelimited(val, ",");

                                if (dmxChannel.ChannelNum.Length == 0)
                                    dmxChannel.ChannelNum = new[] { ParseValue<int>(val) };
                            }
                            
                        }, "ChannelNum", error);

                    TryExecute(
                        () =>
                        {
                            val = ParseAttribute<string>(dmxChannelElement, "DominantFeatureGroup");
                            if (!string.IsNullOrWhiteSpace(val))
                                dmxChannel.DominantFeatureGroup = ParseEnum<FeatureGroup>(val);
                        }, "DominantFeatureGroup", error);

                    TryExecute(
                        () =>
                        {
                            val = ParseAttribute<string>(dmxChannelElement, "ElementNumber");
                            if (!string.IsNullOrWhiteSpace(val))
                                dmxChannel.ElementNumber = ParseValue<int>(val);
                        }, "ElementNumber", error);
                    
                    TryExecute(
                        () =>
                        {
                            val = ParseElement<string>(dmxChannelElement, "Default");
                            if (!string.IsNullOrWhiteSpace(val))
                                dmxChannel.Default = ParseValue<int>(val);
                        }, "Default", error);

                    TryExecute(
                        () =>
                        {
                            val = ParseElement<string>(dmxChannelElement, "DmxChannelLabel");
                            dmxChannel.DmxChannelLabel = val;
                        }, "DmxChannelLabel", error);

                    TryExecute(
                        () =>
                        {
                            dmxChannel.Ranges = ParseDmxRange(dmxChannelElement, null, error);
                        }, "DmxRange", error);

                    TryExecute(
                        () =>
                        {
                            dmxChannel.Modules = ParseDmxModule(dmxChannelElement, error);
                        }, "DmxModule", error);

                    var conditionalRangeSets = ParseConditionalDmxRangeSet(dmxChannelElement, error);

                    if (conditionalRangeSets.Any())
                        dmxChannel.Ranges.AddRange(conditionalRangeSets.SelectMany(rs => rs.Value));

                    if (error.FieldsWithError.Count == 0)
                        channels.Add(dmxChannel);

                    if (error.HaveError) result.ErrorList.Add(error);
                }
            }

            return channels;
        }

        private static Dictionary<ConditionalDmxRangeSet,List<DmxRange>> ParseConditionalDmxRangeSet(XElement source, IParseResult result)
        {
            Dictionary<ConditionalDmxRangeSet,List<DmxRange>> rangeSets = new Dictionary<ConditionalDmxRangeSet, List<DmxRange>>();

            var rangeSetElements = source.Elements("ConditionalDmxRangeSet");

            if (rangeSetElements.Any())
            {
                string val;
                foreach (XElement rangeSetElement in rangeSetElements)
                {
                    var rangeSet = new ConditionalDmxRangeSet();
                    ParseResult error = new ParseResult(rangeSetElement.Name.LocalName);

                    TryExecute(
                        () =>
                        {
                            rangeSet.RangeSetCondition = ParseDmxRangeSetCondition(rangeSetElement, error).FirstOrDefault();
                        }, "DmxRangeSetCondition", error);
                    
                    if (error.FieldsWithError.Count == 0)
                    {
                        rangeSets[rangeSet] = ParseDmxRange(rangeSetElement, rangeSet, error);
                    }

                    if (error.HaveError) result.ErrorList.Add(error);
                }
            }

            return rangeSets;
        }

        private static List<DmxRangeSetCondition> ParseDmxRangeSetCondition(XElement source, IParseResult result)
        {
            List<DmxRangeSetCondition> setConditions = new List<DmxRangeSetCondition>();

            var conditionElements = source.Elements("DmxRangeSetCondition");

            if (conditionElements.Any())
            {
                string val;
                foreach (XElement conditionElement in conditionElements)
                {
                    var condition = new DmxRangeSetCondition();
                    ParseResult error = new ParseResult(conditionElement.Name.LocalName);

                    TryExecute(
                        () =>
                        {
                            val = ParseAttribute<string>(conditionElement, "ChannelNum");
                            Validate(() => Validator.HasValue(val));
                            Validate(() => Validator.IsMatch("\\d+", val));
                            condition.ChannelNum = ParseValue<int>(val);
                        }, "ChannelNum", error);


                    TryExecute(
                        () =>
                        {
                            var node = conditionElement.GetElement("DmxRange", true);

                            val = ParseAttribute<string>(node, "Range");
                            Validate(() => Validator.HasValue(val));
                            Validate(() => Validator.IsMatch("\\d+.{3}\\d+", val));
                            condition.DmxRange = ParseRangeValue(val);
                        }, "DmxRange", error);

                    if(error.FieldsWithError.Count == 0)
                        setConditions.Add(condition);

                    if (error.HaveError) result.ErrorList.Add(error);                        
                }
            }

            return setConditions;
        }

        private static List<DmxModule> ParseDmxModule(XElement source, IParseResult result)
        {
            List<DmxModule> dmxRanges = new List<DmxModule>();

            var moduleElements = source.Elements("DmxModule");

            if (moduleElements.Any())
            {
                foreach (var moduleElement in moduleElements)
                {
                    var module = new DmxModule();

                    ParseResult error = new ParseResult(moduleElement.Name.LocalName);
                    string val;

                    TryExecute(
                        () =>
                        {
                            val = ParseAttribute<string>(moduleElement, "DmxModuleName");
                            Validate(() => Validator.HasValue(val));
                            module.Name = val;
                        }, "DmxModuleName", error);

                    TryExecute(
                        () =>
                        {
                            val = ParseAttribute<string>(moduleElement, "ModuleInstance");
                            Validate(() => Validator.HasValue(val));
                            module.Instance = ParseValue<int>(val);
                        }, "ModuleInstance", error);

                    if (moduleElement.HaveAttribute("TempElementModuleInstance"))
                    {
                        TryExecute(
                        () =>
                        {
                            val = ParseAttribute<string>(moduleElement, "TempElementModuleInstance");
                            Validate(() => Validator.HasValue(val));
                            module.TempElementModuleInstance = ParseValue<int>(val);
                        }, "TempElementModuleInstance", error);
                    }                    

                    if (error.FieldsWithError.Count == 0)
                        dmxRanges.Add(module);

                    if (error.HaveError) result.ErrorList.Add(error);
                }
            }

            return dmxRanges;
        }

        private static List<DmxRange> ParseDmxRange(XElement source, ConditionalDmxRangeSet rangeSet, IParseResult result)
        {
            List<DmxRange> dmxRanges = new List<DmxRange>();

            var rangeElements = source.Elements("DmxRange");

            if (rangeElements.Any())
            {
                foreach (var rangeElement in rangeElements)
                {
                    var range = new DmxRange();

                    ParseResult error = new ParseResult(rangeElement.Name.LocalName);
                    string val;
                    
                    TryExecute(
                        () =>
                        {
                            val = ParseAttribute<string>(rangeElement, "Range");
                            Validate(() => Validator.HasValue(val));
                            Validate(() => Validator.IsMatch("\\d+.{3}\\d+", val));
                            range.Range = ParseRangeValue(val);
                        }, "Range", error);

                    TryExecute(
                        () =>
                        {
                            if (rangeElement.HaveElement("DmxRangeLabel"))
                            {
                                range.RangeLabel = rangeElement.GetElementValue("DmxRangeLabel");
                            }
                        }, "DmxRangeLabel", error);
                    
                    TryExecute(
                        () =>
                        {
                            range.FeatureRange = ParseFeatureRange(rangeElement, error);
                        }, "FeatureRange", error);                    
                    
                    range.ConditionalRangeSet = rangeSet;

                    if (error.FieldsWithError.Count == 0)
                        dmxRanges.Add(range);

                    if (error.HaveError) result.ErrorList.Add(error);
                }
            }

            return dmxRanges;
        }        

        private static List<FeatureRange> ParseFeatureRange(XElement source, IParseResult result)
        {
            List<FeatureRange> ranges = new List<FeatureRange>();

            var rangeElements = source.Elements("FeatureRange");

            if (rangeElements.Any())
            {
                foreach (var rangeElement in rangeElements)
                {
                    var range = new FeatureRange();

                    ParseResult error = new ParseResult(rangeElement.Name.LocalName);
                    string val;
                    XElement node;

                    TryExecute(
                        () =>
                        {
                            val = ParseAttribute<string>(rangeElement, "FeatureName");
                            Validate(() => Validator.HasValue(val));
                            range.FeatureName = ParseEnum<FeatureName>(val);
                        }, "FeatureName", error);

                    TryExecute(
                        () =>
                        {
                            node = rangeElement.GetElement("Value", false);

                            if(node == null) return;
                            range.Value = node.Value;

                            TryExecute(
                                () =>
                                {
                                    val = ParseAttribute<string>(node, "UnitName");
                                    Validate(() => Validator.HasValue(val));
                                    range.UnitName = ParseEnum<UnitType>(val);
                                }, "UnitName", error);                            

                        }, "Value", error);

                    if (error.FieldsWithError.Count == 0)
                        ranges.Add(range);

                    if (error.HaveError) result.ErrorList.Add(error);
                }
            }

            return ranges;
        }

        private static List<Feature> ParseFeature(XElement source, IParseResult result)
        {
            List<Feature> features = new List<Feature>();

            var featureElements = source.Elements("Feature");

            if (featureElements.Any())
            {
                foreach (var featureElement in featureElements)
                {
                    var feature = new Feature();

                    ParseResult error = new ParseResult(featureElement.Name.LocalName);
                    string val;
                    XElement node;

                    TryExecute(
                        () =>
                        {
                            val = ParseAttribute<string>(featureElement, "FeatureName");
                            Validate(() => Validator.HasValue(val));                            
                            feature.FeatureName = ParseEnum<FeatureName>(val);
                        }, "FeatureName", error);

                    TryExecute(
                        () =>
                        {
                            val = ParseAttribute<string>(featureElement, "UnitName");
                            if (!string.IsNullOrWhiteSpace(val))
                                feature.UnitName = ParseEnum<UnitType>(val);
                        }, "UnitName", error);

                    TryExecute(
                        () =>
                        {
                            val = ParseAttribute<string>(featureElement, "WheelType");
                            if (!string.IsNullOrWhiteSpace(val))
                                feature.WheelType = ParseEnum<WheelType>(val);
                        }, "WheelType", error);

                    TryExecute(
                        () =>
                        {
                            if (featureElement.HaveElement("ResponseTime"))
                                feature.ResponseTime = ParseResponseTime(featureElement, error).FirstOrDefault();
                        }, "ResponseTime", error);

                    feature.Specs = ParseSpecValue(featureElement, error);
                    feature.Slots = new List<Slot>();

                    var nodes = featureElement.Elements("Slot");
                    if (nodes.Any())
                    {
                        feature.Slots = nodes.SelectMany(n => ParseSlot(n, null, error)).ToList();                        
                    }

                    var mediaSlots = ParseMediaRangeInfo(featureElement, error);
                    feature.Slots.AddRange(mediaSlots.SelectMany(ms => ms.Value).ToList());

                    if (error.FieldsWithError.Count == 0)
                        features.Add(feature);

                    if (error.HaveError) result.ErrorList.Add(error);
                }
            }

            return features;
        }

        private static List<ResponseTime> ParseResponseTime(XElement source, IParseResult result)
        {
            List<ResponseTime> responseTimes = new List<ResponseTime>();

            var rtimeElements = source.Elements("ResponseTime");

            if (rtimeElements.Any())
            {
                foreach (var rtimeElement in rtimeElements)
                {
                    var time = new ResponseTime();

                    ParseResult error = new ParseResult(rtimeElement.Name.LocalName);
                    string val;

                    TryExecute(
                        () =>
                        {
                            val = ParseAttribute<string>(rtimeElement, "Calibrated");
                            Validate(() => Validator.HasValue(val));
                            time.Calibrated = ParseValue<float>(val);
                        }, "Calibrated", error);                    

                    if (error.FieldsWithError.Count == 0)
                        responseTimes.Add(time);

                    if (error.HaveError) result.ErrorList.Add(error);
                }
            }

            return responseTimes;
        }

        private static Dimension ParserFixtureDimension(XElement source, IParseResult result)
        {
            XElement dimensionElement = source.Element("FixtureDimensions");

            var dimension = new Dimension();

            if (dimensionElement != null)
            {
                ParseResult error = new ParseResult(dimensionElement.Name.LocalName);
                string val;

                TryExecute(
                    () =>
                    {
                        val = ParseAttribute<string>(dimensionElement, "XSize");
                        Validate(() => Validator.HasValue(val));
                        dimension.X = ParseValue<float>(val);
                    }, "XSize", error);

                TryExecute(
                    () =>
                    {
                        val = ParseAttribute<string>(dimensionElement, "YSize");
                        Validate(() => Validator.HasValue(val));
                        dimension.Y = ParseValue<float>(val);
                    }, "YSize", error);

                TryExecute(
                    () =>
                    {
                        val = ParseAttribute<string>(dimensionElement, "ZSize");
                        Validate(() => Validator.HasValue(val));
                        dimension.Z = ParseValue<float>(val);
                    }, "ZSize", error);

                if (error.HaveError) result.ErrorList.Add(error);
            }

            return dimension;
        }

        private static BeamAngle ParserFixtureBeamAngle(XElement source, IParseResult result)
        {
            XElement angleElement = source.Element("FixtureBeamAngle");

            var angle = new BeamAngle();

            if (angleElement != null)
            {
                ParseResult error = new ParseResult(angleElement.Name.LocalName);
                string val;

                TryExecute(
                    () =>
                    {
                        val = ParseAttribute<string>(angleElement, "Angle");
                        Validate(() => Validator.HasValue(val));
                        angle.Angle = ParseValue<float>(val);
                    }, "Angle", error);

                if (error.HaveError) result.ErrorList.Add(error);
            }

            return angle;
        }

        private static List<SpecValue> ParseSpecValue(XElement source, IParseResult result)
        {
            List<SpecValue> specs = new List<SpecValue>();

            var specElements = source.Elements("SpecValue");

            if (specElements.Any())
            {
                foreach (var specElement in specElements)
                {
                    var spec = new SpecValue();

                    ParseResult error = new ParseResult(specElement.Name.LocalName);
                    string val;
                    XElement node;

                    TryExecute(
                        () =>
                        {
                            val = ParseAttribute<string>(specElement, "PercentValue");
                            Validate(() => Validator.HasValue(val));
                            spec.PercentValue = ParseValue<int>(val);
                        }, "PercentValue", error);

                    TryExecute(
                        () =>
                        {
                            val = ParseAttribute<string>(specElement, "UnitValue");
                            Validate(() => Validator.HasValue(val));
                            spec.UnitValue = ParseValue<float>(val);
                        }, "UnitValue", error);

                    if (error.FieldsWithError.Count == 0)
                        specs.Add(spec);

                    if (error.HaveError) result.ErrorList.Add(error);
                }
            }

            return specs;
        }

        /// <summary>
        /// Some slot are inside MediaRange tag. Parent tag mapped in property
        /// E:\Workspace\Mike\FixtureLibrary\source\data\fixtures\clay_paky\goldenscan_3_8_ch\goldenscan_3_8_ch.xml
        /// </summary>
        /// <param name="source"></param>
        /// <param name="parentMediaRange"></param>
        /// <returns></returns>
        private static List<Slot> ParseSlot(XElement source, MediaRangeInfo parentMediaRange, IParseResult result)
        {
            List<Slot> slots = new List<Slot>();

            var slotElements = source.Elements("Slot");

            if (slotElements.Any())
            {
                foreach (var slotElement in slotElements)
                {
                    var slot = new Slot();
                    slot.MediaRangeInfo = parentMediaRange;

                    ParseResult error = new ParseResult(slotElement.Name.LocalName);
                    string val;
                    
                    TryExecute(
                        () =>
                        {
                            val = ParseAttribute<string>(slotElement, "SlotNumber");
                            Validate(() => Validator.HasValue(val));
                            slot.Number = ParseValue<int>(val);
                        }, "SlotNumber", error);

                    TryExecute(
                        () =>
                        {
                            val = ParseAttribute<string>(slotElement, "LocalName");
                            Validate(() => Validator.HasValue(val));
                            slot.Name = val;
                        }, "LocalName", error);

                    TryExecute(
                        () =>
                        {
                            val = ParseAttribute<string>(slotElement, "MediaName");
                            Validate(() => Validator.HasValue(val));
                            slot.MediaName = val;
                        }, "MediaName", error);

                    if (error.FieldsWithError.Count == 0)
                        slots.Add(slot);

                    if (error.HaveError) result.ErrorList.Add(error);
                }
            }

            return slots;
        }

        private static List<DmxTimingSet> ParseDmxTimingSet(XElement source, IParseResult result)
        {
            List<DmxTimingSet> timingSets = new List<DmxTimingSet>();

            var tsElements = source.Elements("DmxTimingSet");

            if (tsElements.Any())
            {
                foreach (var tsElement in tsElements)
                {
                    var timingSet = new DmxTimingSet();

                    ParseResult error = new ParseResult(tsElement.Name.LocalName);
                    string val;
                    XElement node;

                    TryExecute(
                        () =>
                        {
                            val = ParseAttribute<string>(tsElement, "HoldTime");
                            Validate(() => Validator.HasValue(val));
                            timingSet.HoldTime = ParseValue<int>(val);
                        }, "HoldTime", error);

                    TryExecute(
                        () =>
                        {
                            timingSet.ChannelSetting = ParseDmxChannelSetting(tsElement, error).FirstOrDefault();
                        }, "DmxChannelSetting", error);

                    if (error.FieldsWithError.Count == 0)
                        timingSets.Add(timingSet);

                    if (error.HaveError) result.ErrorList.Add(error);
                }
            }

            return timingSets;
        }

        private static List<DmxChannelSetting> ParseDmxChannelSetting(XElement source, IParseResult result)
        {
            List<DmxChannelSetting> channelSettings = new List<DmxChannelSetting>();

            var settingElements = source.Elements("DmxChannelSetting");

            if (settingElements.Any())
            {
                foreach (var settingElement in settingElements)
                {
                    var setting = new DmxChannelSetting();

                    ParseResult error = new ParseResult(settingElement.Name.LocalName);
                    string val;
                    
                    TryExecute(
                        () =>
                        {
                            val = ParseAttribute<string>(settingElement, "ChannelNum");
                            Validate(() => Validator.HasValue(val));
                            setting.ChannelNum = ParseValue<int>(val);
                        }, "ChannelNum", error);

                    TryExecute(
                        () =>
                        {
                            val = ParseAttribute<string>(settingElement, "DmxValue");
                            Validate(() => Validator.HasValue(val));

                            if (HaveRangeValue(val))
                            {
                                setting.DmxValueRange = ParseRangeValue(val);
                                return;
                            }

                            setting.DmxValue = ParseValue<int>(val);
                        }, "DmxValue", error);

                    if (error.FieldsWithError.Count == 0)
                        channelSettings.Add(setting);

                    if (error.HaveError) result.ErrorList.Add(error);
                }
            }

            return channelSettings;
        }

        private static Dictionary<MediaRangeInfo, List<Slot>> ParseMediaRangeInfo(XElement source, IParseResult result)
        {
            Dictionary<MediaRangeInfo,List<Slot>> ranges = new Dictionary<MediaRangeInfo, List<Slot>>();

            var rangeElements = source.Elements("MediaRange");

            if (rangeElements.Any())
            {
                MediaRangeInfo range;
                ParseResult error;
                string val;
                XElement node;

                foreach (var rangeElement in rangeElements)
                {
                    range = new MediaRangeInfo();
                    error = new ParseResult(rangeElement.Name.LocalName);

                    TryExecute(
                        () =>
                        {
                            val = ParseAttribute<string>(rangeElement, "MediaType");
                            Validate(() => Validator.HasValue(val));
                            range.MediaType = ParseEnum<MediaType>(val);
                        }, "MediaType", error);

                    TryExecute(
                        () =>
                        {
                            val = ParseAttribute<string>(rangeElement, "MediaManufacturer");
                            Validate(() => Validator.HasValue(val));
                            range.Manufacturer = val;
                        }, "MediaManufacturer", error);

                    TryExecute(
                        () =>
                        {
                            val = ParseAttribute<string>(rangeElement, "MediaRangeName");
                            Validate(() => Validator.HasValue(val));
                            range.Name = val;
                        }, "MediaRangeName", error);

                    if (error.FieldsWithError.Count == 0)
                    {
                        ranges[range] = ParseSlot(rangeElement, range, error);
                    }

                    if (error.HaveError) result.ErrorList.Add(error);
                }
            }

            return ranges;
        }

        private static RdmSpecification ParseRdmSpecification(XElement source, IParseResult result)
        {
            XElement rdmSpecElement = source.Element("RdmSpecification");

            var rdmSpec = new RdmSpecification();

            if (rdmSpecElement != null)
            {
                ParseResult error = new ParseResult(rdmSpecElement.Name.LocalName);
                string val;
                XElement node;
            }

            return rdmSpec;
        }
    }
}
