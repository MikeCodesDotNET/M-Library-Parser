using Carallon.MLibrary.Models.Dmx;
using System.Collections.Generic;

namespace Carallon.MLibrary.Fixture
{
    public class Channel
    {
        public int Id { get; set; }

        public int Number { get; set; } //Maps to 'ChannelNum'
        public Enums.FeatureGroup DominantFeatureGroup { get; set; } //Maps to 'DominantFeatureGroup'
        public string FeatureName { get; set; } //Maps to 'FeatureName'
        public ushort DefaultValue { get; set; } //Maps to 'Default'
        public List<DmxRange> DmxRanges { get; set; } //Maps to 'DmxRanges'
        public string Label { get; set; } //Maps to 'DmxChannelLabel'
        
    }
}
