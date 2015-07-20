using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteMic.Core.Fixture.Enums
{
    public enum ValidPatchGroupLabel
    {
        Part,                   //Used if multiple identical parts to a fixture
        ExternalDimmer,         //Used where fixture lamp is controlled by an external dimmer
        MasterChannels,         //Used for the master channels in a fixture
        Fixture                 //Used for the rest of a fixture that is not controlled by an external dimmer or by master channels
    }
}
