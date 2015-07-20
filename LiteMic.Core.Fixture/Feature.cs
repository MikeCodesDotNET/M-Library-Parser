using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteMic.Core.Fixture.Enums;

namespace LiteMic.Core.Fixture
{
    public class Feature
    {
        public FeatureName FeatureName { get; set; }
        public UnitType? UnitName { get; set; }
        public WheelType? WheelType { get; set; }
        public List<SpecValue> Specs { get; set; }
        public List<Slot> Slots { get; set; }
        public ResponseTime ResponseTime { get; set; }
    }
}
