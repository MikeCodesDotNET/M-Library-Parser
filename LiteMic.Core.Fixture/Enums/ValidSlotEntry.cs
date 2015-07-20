using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteMic.Core.Fixture
{
    //Standard names that can be used as a LocalName in a Slot tag in the SlotMap. Any Slot tag with one of these names must not be inside a MediaRange tag
    public enum ValidSlotEntry //More correctly DefinedLocalName
    {
        Open,
        Blackout
    }
}
