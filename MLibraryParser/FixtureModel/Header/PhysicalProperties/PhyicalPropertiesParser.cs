using Carallon.Helpers;
using Carallon.MLibrary.Fixture;
using Carallon.MLibrary.Fixture.Enums;
using Carallon.MLibrary.Fixture.Enums.ColourMixing;
using Carallon.MLibrary.Models;
using Carallon.MLibrary.Models.Misc;
using Carallon.MLibrary.Models.Physical;
using Carallon.MLibrary.Models.Physical.Enums;
using LiteMic.FixtureModel.Header;
using LiteMic.FixtureModel.Header.PhysicalProperties;
using LiteMic.FixtureModel.Header.PhysicalProperties.SlotMap;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Carallon.Parsers
{
    internal class PhyicalPropertiesParser : BaseParser
    {
        public PhysicalProperties Parse(XElement source, ParseResult result)
        {
            XElement propertiesElement = source.Element("PhysicalProperties");

            var physicalProperties = new PhysicalProperties();

            if (propertiesElement != null)
            {
                ParseResult error = new ParseResult(propertiesElement.Name.LocalName);
                string val;
                XElement node;

                // Physical Type 
                TryExecute(
                    () =>
                    {
                        val = ParseElement<string>(propertiesElement, "PhysicalType");
                        Validate(() => Validator.HasValue(val));

                        physicalProperties.Type = ParseEnum<PhysicalType>(val);
                    }, "PhysicalType", error);

                // Light Generation
                TryExecute(
                    () =>
                    {
                        val = ParseElement<string>(propertiesElement, "LightGeneration");
                        Validate(() => Validator.HasValue(val));

                        physicalProperties.LightGeneration = ParseEnum<LightGeneration>(val);
                    }, "LightGeneration", error);

                // Movement Type 
                TryExecute(
                    () =>
                    {
                        val = ParseElement<string>(propertiesElement, "MovementType");
                        Validate(() => Validator.HasValue(val));

                        physicalProperties.MovementType = ParseEnum<MovementType>(val);
                    }, "MovementType", error);

                // Beam Type
                TryExecute(
                    () =>
                    {
                        val = ParseElement<string>(propertiesElement, "BeamType");
                        Validate(() => Validator.HasValue(val));

                        physicalProperties.BeamType = ParseEnum<BeamType>(val);
                    }, "BeamType", error);

                // Colour Mixing 
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

                                physicalProperties.ColourMixingType = ParseEnum<MixingType>(val);
                            }, "MixingType", error);
                    }, "ColourMixing", error);

                // Compound Structure
                TryExecute(
                    () =>
                    {
                        if (propertiesElement.HaveElement("CompoundStructure"))
                            physicalProperties.CompoundStructure = ParseCompoundSturcture(propertiesElement, error);
                    }, "CompoundStructure", error);

                // Fixture Dimensions
                TryExecute(
                    () =>
                    {
                        if (propertiesElement.HaveElement("FixtureDimensions"))
                            physicalProperties.Dimension = ParserFixtureDimension(propertiesElement, error);
                    }, "FixtureDimensions", error);

                // Fixture Mass 
                TryExecute(
                    () =>
                    {
                        if (propertiesElement.HaveElement("FixtureMass"))
                            physicalProperties.FixtureMass = ParseValue<float>(propertiesElement.GetElementValue("FixtureMass"));
                    }, "FixtureMass", error);

                // Fixture Beam Angel
                TryExecute(
                    () =>
                    {
                        if (propertiesElement.HaveElement("FixtureBeamAngle"))
                            physicalProperties.BeamAngle = ParserFixtureBeamAngle(propertiesElement, error);
                    }, "FixtureBeamAngle", error);


                
                // Slot Map
                node = propertiesElement.GetElement("SlotMap", false);
                if (node != null)
                {
                    var slotMapParser = new SlotMapParser();
                    var features = slotMapParser.Parse(node, error);
                    physicalProperties.SlotMaps = features;
                }
                       
                
                // Value Map
                node = propertiesElement.GetElement("ValueMap", false);
                if (node != null)
                {
                    var valueMapParser = new ValueMapParser();
                    var features = valueMapParser.Parse(node, error);
                    physicalProperties.ValueMaps = features;
                }


                // Enum Map
                node = propertiesElement.GetElement("EnumMap", false);
                if (node != null)
                {
                    var enumMapParser = new EnumMapParser();
                    var features = enumMapParser.Parse(node, error);
                    physicalProperties.EnumMaps = features;
                }

                if (error.HasError) result.ErrorList.Add(error);

            }
            else
            {
                result.FieldsWithError["PhysicalProperties"] = "Undefined";
            }

            return physicalProperties;
        }
    


        /// <summary>
        /// Some slot are inside MediaRange tag. Parent tag mapped in property
        /// </summary>
        /// <param name="source"></param>
        /// <param name="parentMediaRange"></param>
        /// <returns></returns>
        List<Slot> ParseSlots(XElement source, MediaRangeInfo parentMediaRange, IParseResult result)
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

                    TryExecute(() =>
                        {
                            val = ParseAttribute<string>(slotElement, "SlotNumber");
                            Validate(() => Validator.HasValue(val));
                            slot.SlotNumber = ParseValue<int>(val);
                        }, "SlotNumber", error);

                    TryExecute(() =>
                        {
                            val = ParseAttribute<string>(slotElement, "LocalName");
                            Validate(() => Validator.HasValue(val));
                            slot.LocalName = val;
                        }, "LocalName", error);

                    TryExecute(() =>
                        {
                            val = ParseAttribute<string>(slotElement, "MediaName");
                            Validate(() => Validator.HasValue(val));
                            slot.MediaName = val;
                        }, "MediaName", error);

                    if (error.FieldsWithError.Count == 0)
                        slots.Add(slot);

                    if (error.HasError) result.ErrorList.Add(error);
                }
            }

            return slots;
        }
          

        Dimension ParserFixtureDimension(XElement source, IParseResult result)
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

                if (error.HasError) result.ErrorList.Add(error);
            }

            return dimension;
        }

        BeamAngle ParserFixtureBeamAngle(XElement source, IParseResult result)
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

                if (error.HasError) result.ErrorList.Add(error);
            }

            return angle;
        }
               
        CompoundStructure ParseCompoundSturcture(XElement source, IParseResult result)
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

                    if (structureElement.HaveElement("CellGeometry"))
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

            if (error.HasError) result.ErrorList.Add(error);

            return structure;
        }

    }
}
