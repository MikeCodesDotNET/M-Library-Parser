using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteMic.Core.Fixture.Enums
{
    //Values the Reason attribute can take
    public enum NoPhotometricDataReason
    {
        GenericPersonality,
        NoManufacturersData,
        Accessory,
        Controller,
        DimmerRack,
        Effect,
        FibreOpticSource,
        Laser,
        MediaServer,
        ServerProjector,
        Strobe,
        VideoCamera,
        VideoProjector,
        VideoPanel
    }
}
