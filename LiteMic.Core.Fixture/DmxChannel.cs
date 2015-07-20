using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteMic.Core.Fixture.Enums;

namespace LiteMic.Core.Fixture
{
    public class DmxChannel
    {
        public DmxChannel()
        {
            Ranges = new List<DmxRange>();
            Modules = new List<DmxModule>();
        }

        public int[] ChannelNum { get; set; }

        public RangeValue ChanelNumRange { get; set; }

        public int ElementNumber { get; set; }
        public int Default { get; set; }
        public string DmxChannelLabel { get; set; }
        public FeatureGroup? DominantFeatureGroup { get; set; }
        public List<DmxRange> Ranges { get; set; }

        public List<DmxModule>  Modules { get; set; }
    }
}
