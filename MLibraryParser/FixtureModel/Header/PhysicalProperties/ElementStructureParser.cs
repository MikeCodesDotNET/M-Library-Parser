using Carallon.Helpers;
using Carallon.MLibrary.Fixture;
using Carallon.MLibrary.Fixture.Enums.ColourMixing;
using Carallon.MLibrary.Models.Enums.ColourMixing;
using Carallon.MLibrary.Models.Misc;
using Carallon.MLibrary.Models.Physical.Enums;
using Carallon.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LiteMic.FixtureModel.Header.PhysicalProperties
{
    internal class ElementStructureParser : BaseParser
    {
        public ElementStructure Parse(XElement source, ParseResult result)
        {
            XElement elementStructureElement = source.Element("ElementStructure");
            var elementStructor = new ElementStructure();

            if (elementStructureElement != null)
            {
                ParseResult error = new ParseResult(elementStructureElement.Name.LocalName);
                string val;

                // Physical Type 
                TryExecute(() =>
                {
                    val = ParseElement<string>(elementStructureElement, "PhysicalType");
                    Validate(() => Validator.HasValue(val));

                    elementStructor.PhysicalType = ParseEnum<PhysicalType>(val);
                }, "PhysicalType", error);

                var opticalSystemElement = elementStructureElement.GetElement("OpticalSystem", false);
                if (opticalSystemElement != null)
                {
                    // Source Type 
                    TryExecute(() =>
                    {
                        val = ParseAttribute<string>(opticalSystemElement, "SourceType");
                        Validate(() => Validator.HasValue(val));

                        elementStructor.OpticalSystem.SourceType = ParseEnum<LightGeneration>(val);
                    }, "SourceType", error);

                    TryExecute(() =>
                    {
                        val = ParseAttribute<string>(opticalSystemElement, "BeamType");
                        Validate(() => Validator.HasValue(val));

                        elementStructor.OpticalSystem.BeamType = ParseEnum<BeamType>(val);
                    }, "BeamType", error);


                    //Anything above will 100% be in the fixture. Below are optionals 
                    if(opticalSystemElement.HaveAttribute("ColourMixingType"))
                    {
                        TryExecute(() =>
                        {
                            val = ParseAttribute<string>(opticalSystemElement, "ColourMixingType");
                            Validate(() => Validator.HasValue(val));

                            elementStructor.OpticalSystem.ColourMixingType = ParseEnum<MixingType>(val);
                        }, "ColourMixingType", error);
                    }

                    if (opticalSystemElement.HaveAttribute("ColourMixingSystem"))
                    {
                        TryExecute(() =>
                        {
                            val = ParseAttribute<string>(opticalSystemElement, "ColourMixingSystem");
                            Validate(() => Validator.HasValue(val));

                            elementStructor.OpticalSystem.ColourMixingSystem = ParseEnum<MixingSystem>(val);
                        }, "ColourMixingSystem", error);
                    }


                    if (opticalSystemElement.HaveAttribute("BeamType"))
                    {
                        TryExecute(() =>
                        {
                            val = ParseAttribute<string>(opticalSystemElement, "BeamType");
                            Validate(() => Validator.HasValue(val));

                            elementStructor.OpticalSystem.BeamType = ParseEnum<BeamType>(val);
                        }, "BeamType", error);
                    }

                    if (opticalSystemElement.HaveAttribute("BeamAngle"))
                    {
                        TryExecute(() =>
                        {
                            val = ParseAttribute<string>(opticalSystemElement, "BeamAngle");
                            Validate(() => Validator.HasValue(val));

                            elementStructor.OpticalSystem.BeamAngle = int.Parse(val);
                        }, "BeamAngle", error);
                    }

                }

                var movemementElement = elementStructureElement.Element("Movement");
                if (movemementElement != null)
                {
                    // Source Type 
                    TryExecute(() =>
                    {
                        val = ParseAttribute<string>(movemementElement, "MovementType");
                        Validate(() => Validator.HasValue(val));

                        elementStructor.MovementType = ParseEnum<MovementType>(val);
                    }, "MovementType", error);                  
                }

                var dimensionsElement = elementStructureElement.Element("FixtureDimensions");
                if (dimensionsElement != null)
                {
                    TryExecute(() =>
                    {
                        val = ParseAttribute<string>(dimensionsElement, "XSize");
                        Validate(() => Validator.HasValue(val));

                        elementStructor.FixtureDimensions.X = int.Parse(val);
                    }, "XSize", error);

                    TryExecute(() =>
                    {
                        val = ParseAttribute<string>(dimensionsElement, "YSize");
                        Validate(() => Validator.HasValue(val));

                        elementStructor.FixtureDimensions.Y = int.Parse(val);
                    }, "YSize", error);

                    TryExecute(() =>
                    {
                        val = ParseAttribute<string>(dimensionsElement, "ZSize");
                        Validate(() => Validator.HasValue(val));

                        elementStructor.FixtureDimensions.Z = int.Parse(val);
                    }, "ZSize", error);
                }

                // Physical Type 
                TryExecute(
                    () =>
                    {
                        val = ParseElement<string>(elementStructureElement, "FixtureMass");
                        Validate(() => Validator.HasValue(val));

                        elementStructor.FixtureMass = float.Parse(val);
                    }, "FixtureMass", error);

            }

            return elementStructor;            
        }
    }    
}
