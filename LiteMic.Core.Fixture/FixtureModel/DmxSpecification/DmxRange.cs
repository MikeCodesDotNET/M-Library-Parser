using Carallon.MLibrary.Fixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carallon.MLibrary.Models.Dmx
{
    /// <summary>
    /// Specifies the range of values required for a modal channel which will result in a particular behaviour of the channel.  
    /// If the channel is dependent on two other channels then there must be two DmxRangeSetConditions, one per channel.
    /// </summary>
    public class DmxRange
    {        
        public string RangeLabel { get; set; }
        public RangeValue Range { get; set; }        
        //public string UnitName { get; set; }


        public List<FeatureRange> FeatureRange { get; set; }
        public ConditionalDmxRangeSet ConditionalRangeSet { get; set; }
    }
}
