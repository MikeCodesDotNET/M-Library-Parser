using Carallon.MLibrary.Models.Dmx;

namespace Carallon.MLibrary.Fixture
{
    /// <summary>
    /// Specifies the range of values required for a modal channel which will result in a particular behaviour of the channel.  
    /// If the channel is dependent on two other channels then there must be two DmxRangeSetConditions, one per channel.
    /// </summary>
    public class DmxRangeSetCondition
    {
        /// <summary>
        /// ChannelNum - the number of the channel within this DmxModule which the condition relates to.  
        /// </summary>
        public int ChannelNum { get; set; }

        /// <summary>
        /// The DmxRange tags within the DmxRangeSetCondition specify the DMX ranges for that channel for which the condition is satisfied.
        /// Non-contiguous ranges on the same channel are entered as separate ranges.
        /// </summary>
        public RangeValue DmxRange { get; set; }
    }
}
