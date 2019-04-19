namespace Carallon.MLibrary.Fixture
{
    /// <summary>
    /// This tag brackets a conditional set of DMX ranges for this channel.  The DMX ranges must span the channel's DMX space.  
    /// The details of the conditions required for this set to be active are specified in the subsequent DmxRangeSetCondition tag(s). 
    /// When taken over the channel as a whole, the conditions for all ConditionalDmxRangeSet entries must span the space of each of the modifier channels such that for an arbitrary set of 
    /// modifier channel DMX values, one and only one ConditionalDmxRangeSet can always be found that is satisfied.
    /// </summary>
    public class ConditionalDmxRangeSet
    {
        public DmxRangeSetCondition RangeSetCondition { get; set; }
    }
}
