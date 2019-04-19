using Carallon.Helpers;
using Carallon.MLibrary.Fixture;
using Carallon.MLibrary.Fixture.Enums;
using Carallon.MLibrary.Models.Misc;
using Carallon.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LiteMic.FixtureModel.Header.PhysicalProperties
{
    internal class EnumMapParser : BaseParser
    {
        public List<Feature> Parse(XElement source, ParseResult result)
        {
            var featureElements = source.Elements("Feature");
            List<Feature> features = new List<Feature>();

            if (featureElements.Any())
            {
                foreach (var featureElement in featureElements)
                {
                    var feature = new Feature();

                    ParseResult error = new ParseResult(featureElement.Name.LocalName);
                    string val;

                    TryExecute(() =>
                    {
                        val = ParseAttribute<string>(featureElement, "FeatureName");
                        Validate(() => Validator.HasValue(val));
                        feature.FeatureName = ParseEnum<FeatureName>(val);
                    }, "FeatureName", error);
                                                         

                    var enumValueElements = featureElement.Elements("Enum");
                    if (enumValueElements.Any())
                    {
                        foreach (var enumValueElement in enumValueElements)
                        {
                            var enumValue = new EnumValue();
                            TryExecute(() =>
                            {
                                val = ParseAttribute<string>(enumValueElement, "EnumValue");
                                Validate(() => Validator.HasValue(val));
                                enumValue.Value = int.Parse(val);
                            }, "EnumValue", error);

                            TryExecute(() =>
                            {
                                val = ParseAttribute<string>(enumValueElement, "EnumLabel");
                                Validate(() => Validator.HasValue(val));
                                enumValue.Label = val;
                            }, "EnumLabel", error);

                            feature.Enums.Add(enumValue);
                        }
                    }

                    features.Add(feature);
                }
            }
            return features;
        }
    }
}
