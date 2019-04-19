/*
*   The type of beam produced by the fixture, from the following list – defined in spec_enums.xml:
*   ·	None – The fixture does not produce light.  Anything for which the LightGeneration tag is set to “None”.
*   ·	Spot – The fixture produces a spot (profile) beam.
*   ·	Wash – The fixture produces a wash (fresnel, PC or flood) beam.  This includes effects such as strobes. 
*   ·	Fibre Optic – The fixture is designed as a fibre optic source.
*   ·	Effect – The fixture generates multiple beams, eg by use of a multi facet mirror.
*   ·	Unknown – The type is unknown, principally for generic personalities.                                        
*/

namespace Carallon.MLibrary.Models.Physical.Enums
{
    public enum BeamType
    {
        None,
        Spot,
        Wash,
        Fibre,
        Effect,
        Unknown
    }
}
