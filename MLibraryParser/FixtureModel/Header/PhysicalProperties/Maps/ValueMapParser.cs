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
    //VERY similar to the SlotMap Parser, but pulled it out into seperate class in case I need to handle edge cases.
    internal class ValueMapParser : BaseParser
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


                    TryExecute(() =>
                    {
                        val = ParseAttribute<string>(featureElement, "UnitName");
                        Validate(() => Validator.HasValue(val));
                        feature.UnitName = ParseEnum<UnitType>(val);
                    }, "UnitName", error);


                    //Find all slots that live outside of a MediaRange (normally an open colour or blackout gobo)
                    var specValueElements = featureElement.Elements("SpecValue");
                    if (specValueElements.Any())
                    {
                        foreach (var specElement in specValueElements)
                        {
                            var specValue = new SpecValue();
                            TryExecute(() =>
                            {
                                val = ParseAttribute<string>(specElement, "PercentValue");
                                Validate(() => Validator.HasValue(val));
                                specValue.PercentValue = int.Parse(val);
                            }, "PercentValue", error);

                            TryExecute(() =>
                            {
                                val = ParseAttribute<string>(specElement, "UnitValue");
                                Validate(() => Validator.HasValue(val));
                                specValue.UnitValue = float.Parse(val);
                            }, "UnitValue", error);

                            feature.Specs.Add(specValue);
                        }
                    }

                    features.Add(feature);
                }
            }
            return features;
        }
    }
}
