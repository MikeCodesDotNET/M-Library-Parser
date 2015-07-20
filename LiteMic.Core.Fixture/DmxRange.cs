using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteMic.Core.Fixture
{
    public class DmxRange
    {        
        public string RangeLabel { get; set; }
        public RangeValue Range { get; set; }        
        //public string UnitName { get; set; }
        public List<FeatureRange> FeatureRange { get; set; }
        public ConditionalDmxRangeSet ConditionalRangeSet { get; set; }
    }
}
