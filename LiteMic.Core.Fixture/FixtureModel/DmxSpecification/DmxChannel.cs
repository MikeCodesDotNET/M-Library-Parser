using System.Collections.Generic;

using Carallon.MLibrary.Fixture.Enums;

namespace Carallon.MLibrary.Models.Dmx
{
    public class DmxChannel
    {
        public int Id { get; set; }
        
        public DmxChannel()
        {
            Ranges = new List<DmxRange>();
            Modules = new List<DmxModule>();
        }

        public int[] ChannelNumber { get; set; }

        public RangeValue ChanelNumberRange { get; set; }

        public int ElementNumber { get; set; }
        public int Default { get; set; }
               
        public string Label { get; set; }
        public FeatureGroup? DominantFeatureGroup { get; set; }
        public List<DmxRange> Ranges { get; set; }

        public List<DmxModule>  Modules { get; set; }
    }
}
