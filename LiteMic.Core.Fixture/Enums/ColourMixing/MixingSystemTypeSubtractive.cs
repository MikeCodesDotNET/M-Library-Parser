using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carallon.MLibrary.Fixture.Enums.ColourMixing
{
    //Values that the MixingSystem attribute (within ColourMixing tag) can take if MixingType is Subtractive
    public enum MixingSystemTypeSubtractive
    {
        Linear_CMY,
        Linear_CMYG,
        Linear_CMY_Not_Full,      //CMY but does not go to blackout when CMY at 100% - Added for NovaLight NovaFlower Moving Yoke
        Angular_CMY,
        Stepped_CMY,
        Unknown_CMY,
        Unknown_CMMY,
        Linear_CMMY,
        Linear_CM_CY,
        Linear_CM_MY,
        Linear_CY_MY,
        HS_Plate,
        HS_Unknown,
        Linear_CCsMMsY,
        Unknown,
    }
}
