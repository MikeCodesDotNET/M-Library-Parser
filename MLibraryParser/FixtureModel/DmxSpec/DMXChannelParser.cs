using Carallon.Helpers;
using Carallon.MLibrary.Fixture;
using Carallon.MLibrary.Fixture.Enums;
using Carallon.MLibrary.Models.Dmx;
using Carallon.MLibrary.Models.Misc;
using Carallon.Parsers;

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace LiteMic.FixtureModel.DmxSpec
{
    internal class DmxChannelParser : BaseParser
    {
        public List<DmxChannel> Parse(XElement source, IParseResult result)
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

                            if (HasRangeValue(val))
                            {
                                dmxChannel.ChanelNumberRange = ParseRangeValue(val);
                            }
                            else
                            {
                                dmxChannel.ChannelNumber = ParseDelimited(val, ",");

                                if (dmxChannel.ChannelNumber.Length == 0)
                                    dmxChannel.ChannelNumber = new[] { ParseValue<int>(val) };
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
                            dmxChannel.Label = val;
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

                    if (error.HasError) result.ErrorList.Add(error);
                }
            }

            return channels;
        }


        Dictionary<ConditionalDmxRangeSet, List<DmxRange>> ParseConditionalDmxRangeSet(XElement source, IParseResult result)
        {
            Dictionary<ConditionalDmxRangeSet, List<DmxRange>> rangeSets = new Dictionary<ConditionalDmxRangeSet, List<DmxRange>>();

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

                    if (error.HasError) result.ErrorList.Add(error);
                }
            }

            return rangeSets;
        }
               
        List<DmxModule> ParseDmxModule(XElement source, IParseResult result)
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

                    if (error.HasError) result.ErrorList.Add(error);
                }
            }

            return dmxRanges;
        }

        List<DmxRange> ParseDmxRange(XElement source, ConditionalDmxRangeSet rangeSet, IParseResult result)
        {
            List<DmxRange> dmxRanges = new List<DmxRange>();

            var rangeElements = source.Elements("DmxRange");

            if (rangeElements.Any())
            {
                foreach (var rangeElement in rangeElements)
                {
                    var range = new DmxRange();
                    var featureRangeParser = new FeatureRangeParser();
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
                            range.FeatureRange = featureRangeParser.Parse(rangeElement, error);
                        }, "FeatureRange", error);

                    range.ConditionalRangeSet = rangeSet;

                    if (error.FieldsWithError.Count == 0)
                        dmxRanges.Add(range);

                    if (error.HasError) result.ErrorList.Add(error);
                }
            }

            return dmxRanges;
        }
               
        List<DmxRangeSetCondition> ParseDmxRangeSetCondition(XElement source, IParseResult result)
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

                    if (error.FieldsWithError.Count == 0)
                        setConditions.Add(condition);

                    if (error.HasError) result.ErrorList.Add(error);
                }
            }

            return setConditions;
        }
                      
    }
}
