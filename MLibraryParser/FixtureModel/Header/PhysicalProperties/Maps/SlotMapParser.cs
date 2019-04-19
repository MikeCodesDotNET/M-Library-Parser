using Carallon.Helpers;
using Carallon.MLibrary.Fixture;
using Carallon.MLibrary.Fixture.Enums;
using Carallon.MLibrary.Models;
using Carallon.MLibrary.Models.Misc;
using Carallon.Parsers;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace LiteMic.FixtureModel.Header.PhysicalProperties.SlotMap
{
    /// <summary>
    /// The SlotMap is the equivalent of the ValueMap for features that take the unit SlotEntry, 
    /// in that it maps the abstract values used in the DMX spec to real world values – in this case to actual gobos, gels, dichroics, effects or animation disks. 
    /// It lists every position used in each wheel, together with the local name used in the DMX spec.  
    /// It then maps them to the actual media fitted in that position (the gobos, gels, dichroics, etc) which are recorded elsewhere in the database.  
    /// The DMX spec would work without the SlotMap, but the console would not be able to tell the user that, for example, gel 3 on a scroll is Lee 101 gel, 
    /// or that gobo 2 on a gobo wheel is Robe gobo part no 15010/101.  It would not be able to extract further information about it therefore, such as the colour swatch, 
    /// gobo thumbnail, etc.  The second function of the SlotMap is to show the exact order of the slots on the wheel, including any open or blackout positions.
    /// </summary>
    internal class SlotMapParser : BaseParser
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
                        val = ParseElement<string>(featureElement, "SlotCount");
                        Validate(() => Validator.HasValue(val));
                        feature.SlotCount = int.Parse(val);
                    }, "SlotCount", error);

                    
                    //Find all slots that live outside of a MediaRange (normally an open colour or blackout gobo)
                    var slotElementsOutsideOfMediaRange = featureElement.Elements("Slot");
                    if (slotElementsOutsideOfMediaRange.Any())
                    {
                        foreach (var slotElement in slotElementsOutsideOfMediaRange)
                        {
                            var slot = new Slot();
                            TryExecute(() =>
                            {
                                val = ParseAttribute<string>(slotElement, "SlotNumber");
                                Validate(() => Validator.HasValue(val));
                                slot.SlotNumber = int.Parse(val);
                            }, "SlotNumber", error);

                            TryExecute(() =>
                            {
                                val = ParseAttribute<string>(slotElement, "LocalName");
                                Validate(() => Validator.HasValue(val));
                                slot.LocalName = val;
                            }, "LocalName", error);
                            feature.Slots.Add(slot);
                        }
                    }

                    //Proces MediaRanges
                    var mediaRangeElements = featureElement.Elements("MediaRange");
                    if (mediaRangeElements.Any())
                    {
                        foreach (var mediaRangeElement in mediaRangeElements)
                        {
                            var mediaRange = new MediaRangeInfo();
                            TryExecute(() =>
                            {
                                val = ParseAttribute<string>(mediaRangeElement, "MediaType");
                                Validate(() => Validator.HasValue(val));
                                mediaRange.MediaType = ParseEnum<MediaType>(val);
                            }, "MediaType", error);

                            TryExecute(() =>
                            {
                                val = ParseAttribute<string>(mediaRangeElement, "MediaManufacturer");
                                Validate(() => Validator.HasValue(val));
                                mediaRange.Manufacturer = val;
                            }, "MediaManufacturer", error);

                            TryExecute(() =>
                            {
                                val = ParseAttribute<string>(mediaRangeElement, "MediaRangeName");
                                Validate(() => Validator.HasValue(val));
                                mediaRange.Name = val;
                            }, "MediaRangeName", error);

                            //Loop through Slots that belong to a MediaRange 
                            var slotElements = mediaRangeElement.Elements("Slot");
                            if (slotElements.Any())
                            {
                                foreach (var slotElement in slotElements)
                                {
                                    var slot = new Slot();
                                    slot.MediaRangeInfo = mediaRange;
                                    TryExecute(() =>
                                    {
                                        val = ParseAttribute<string>(slotElement, "SlotNumber");
                                        Validate(() => Validator.HasValue(val));
                                        slot.SlotNumber = int.Parse(val);
                                    }, "SlotNumber", error);

                                    TryExecute(() =>
                                    {
                                        val = ParseAttribute<string>(slotElement, "MediaName");
                                        Validate(() => Validator.HasValue(val));
                                        slot.MediaName = val;
                                    }, "MediaName", error);

                                    TryExecute(() =>
                                    {
                                        val = ParseAttribute<string>(slotElement, "LocalName");
                                        Validate(() => Validator.HasValue(val));
                                        slot.LocalName = val;
                                    }, "LocalName", error);
                                    feature.Slots.Add(slot);
                                }
                            }
                        }
                    }

                    features.Add(feature);
                }                
            }
            return features;
        }
    }
}
