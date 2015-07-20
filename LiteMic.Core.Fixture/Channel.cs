using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteMic.Core.Fixture
{
    public class Channel
    {
        public int Number { get; set; } //Maps to 'ChannelNum'
        public Enums.FeatureGroup DominantFeatureGroup { get; set; } //Maps to 'DominantFeatureGroup'
        public string FeatureName { get; set; } //Maps to 'FeatureName'
        public byte HighLightValue { get; set; } //Maps to 'Default'
        public List<DmxRange> DmxRanges { get; set; } //Maps to 'DmxRanges'
        public string Label { get; set; } //Maps to 'DmxChannelLabel'
        
    }
}
