using Carallon.Helpers;
using Carallon.MLibrary.Fixture;
using Carallon.MLibrary.Fixture.Enums;
using Carallon.MLibrary.Models.Dmx;
using Carallon.MLibrary.Models.Misc;
using Carallon.Parsers;
using LiteMic.FixtureModel.DmxSpec;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace LiteMic.FixtureModel
{
    internal class DmxSpecificationParser : BaseParser
    {
        public DmxSpecification Parse(XElement source, ParseResult result)
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

                if (error.HasError) result.ErrorList.Add(error);
            }

            return dmxSpec;
        }

        List<DmxModuleDefinition> ParseDmxModuleDefinition(XElement source, IParseResult result)
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

                    var channelParser = new DmxChannelParser();
                    
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
                            module.Channels = channelParser.Parse(moduleElement, error);
                        }, "DmxChannel", error);

                    if (error.FieldsWithError.Count == 0)
                        moduleDefinitions.Add(module);

                    if (error.HasError) result.ErrorList.Add(error);
                }
            }

            return moduleDefinitions;
        }

        List<DmxPatchGroup> ParseDmxPatchgroup(XElement source, IParseResult result)
        {
            var patchGroups = source.Elements("DmxPatchgroup");

            List<DmxPatchGroup> groups = new List<DmxPatchGroup>();

            if (patchGroups.Any())
            {
                foreach (XElement dmxpgroupElement in patchGroups)
                {
                    var dmxpgroup = new DmxPatchGroup();
                    var channelParser = new DmxChannelParser();

                    ParseResult error = new ParseResult(dmxpgroupElement.Name.LocalName);
                    string val;

                    TryExecute(() =>
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

                    TryExecute(() =>
                        {
                            dmxpgroup.Channels = channelParser.Parse(dmxpgroupElement, error);
                        }, "DmxChannel", error);

                    TryExecute(() =>
                        {
                            var node = dmxpgroupElement.GetElement("DmxMacros", false);
                            if (node != null)
                                dmxpgroup.Macros = ParseDmxMacro(node, error);
                        }, "DmxMacros", error);

                    if (error.FieldsWithError.Count == 0)
                        groups.Add(dmxpgroup);

                    if (error.HasError) result.ErrorList.Add(error);
                }
            }

            return groups;
        }

        List<DmxMacro> ParseDmxMacro(XElement source, IParseResult result)
        {
            List<DmxMacro> macros = new List<DmxMacro>();
            var featureRangeParser = new FeatureRangeParser();
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
                            macro.FeatureRanges = featureRangeParser.Parse(macroElement, error);
                        }, "FeatureRange", error);

                    TryExecute(
                        () =>
                        {
                            macro.TimingSets = ParseDmxTimingSet(macroElement, error);
                        }, "DmxTimingSet", error);

                    if (error.FieldsWithError.Count == 0)
                        macros.Add(macro);

                    if (error.HasError) result.ErrorList.Add(error);
                }
            }

            return macros;
        }


        List<DmxTimingSet> ParseDmxTimingSet(XElement source, IParseResult result)
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

                    if (error.HasError) result.ErrorList.Add(error);
                }
            }

            return timingSets;
        }

        List<DmxChannelSetting> ParseDmxChannelSetting(XElement source, IParseResult result)
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

                            if (HasRangeValue(val))
                            {
                                setting.DmxValueRange = ParseRangeValue(val);
                                return;
                            }

                            setting.DmxValue = ParseValue<int>(val);
                        }, "DmxValue", error);

                    if (error.FieldsWithError.Count == 0)
                        channelSettings.Add(setting);

                    if (error.HasError) result.ErrorList.Add(error);
                }
            }

            return channelSettings;
        }

    }
}
