using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteMic.Core.Fixture
{
    public class DmxChannelSetting
    {
        public int ChannelNum { get; set; }

        public int? DmxValue { get; set; }

        public RangeValue DmxValueRange { get; set; }        
    }
}
