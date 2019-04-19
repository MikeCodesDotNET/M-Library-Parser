using Carallon.MLibrary.Models.Dmx;

namespace Carallon.MLibrary.Fixture
{
    public class DmxChannelSetting
    {
        public int ChannelNum { get; set; }

        public int? DmxValue { get; set; }

        public RangeValue DmxValueRange { get; set; }        
    }
}
