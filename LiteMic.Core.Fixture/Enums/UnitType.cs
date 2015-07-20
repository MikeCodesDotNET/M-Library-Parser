using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteMic.Core.Fixture.Enums
{
    public enum UnitType
    {
        // Primary
        Unary,
        Binary,
        Enum,
        Percent,
        XPercent,
        SlotEntry,

        // Real
        ANSI_Lumens, // space replaced with _
        BPM,
        Degrees,
        FPS,
        Frames,
        Hertz,
        Kelvin,
        Lumens,
        Metres,
        M_S, // slash replaced with _
        Ratio,
        RPM,
        Seconds,
        Watts,
        Uncalibrated,

    }
}
