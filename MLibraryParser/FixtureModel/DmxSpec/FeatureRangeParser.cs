using Carallon.Helpers;
using Carallon.MLibrary.Fixture;
using Carallon.MLibrary.Fixture.Enums;
using Carallon.MLibrary.Models.Misc;
using Carallon.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace LiteMic.FixtureModel.DmxSpec
{
    internal class FeatureRangeParser : BaseParser
    {
        public List<FeatureRange> Parse(XElement source, IParseResult result)
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

                    TryExecute(() =>
                        {
                            val = ParseAttribute<string>(rangeElement, "FeatureName");
                            Validate(() => Validator.HasValue(val));
                            range.FeatureName = ParseEnum<FeatureName>(val);
                        }, "FeatureName", error);

                    TryExecute(() =>
                        {
                            node = rangeElement.GetElement("Value", false);

                            if (node == null) return;
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

                    if (error.HasError) result.ErrorList.Add(error);
                }
            }

            return ranges;
        }

    }
}
