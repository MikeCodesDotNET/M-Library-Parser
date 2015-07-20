using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteMic.Core.Fixture
{
    public class Slot
    {
        public int Number { get; set; }  //Maps to 'SlotNumber'
        public string Name { get; set; } //Maps to 'LocalName'
        public string MediaName { get; set; } //Maps to 'MediaName'
        public MediaRangeInfo MediaRangeInfo { get; set; } // Maps to parent MediaRange
    }
}
