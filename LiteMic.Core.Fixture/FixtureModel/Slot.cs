using Carallon.MLibrary.Fixture;

namespace Carallon.MLibrary.Models
{
    /// <summary>
    /// Each position on a wheel has a corresponding Slot tag in the map.  The tag gives a SlotNumber, and the LocalName used in the DMX spec.  
    /// These names are the link between the map and the SlotEntry definitions within the DMX spec.  Each local name must only appear in one slot tag in the map for any given wheel. 
    /// If the fixture ships from the factory with a particular position empty or unpopulated(no gobo fitted, no gel scroll fitted, etc) then its Slot tag will only contain a local name and the slot number.
    /// If the position is populated when the fixture ships then the slot map will also link it to the media that is fitted to it.
    /// </summary>
    public class Slot
    {
        /// <summary>
        ///The SlotNumber is an integer which gives the position of the slot of the wheel.  
        ///Numbers start at 1 and increase by 1 for each successive slot, up to the value of the SlotCount tag.  
        /// </summary>
        public int SlotNumber { get; set; }

        /// <summary>
        /// Each local name defined in the SlotMap for a particular wheel must be used in the DMX Specification, and each LocalName used in the DMX Specification must be defined in the SlotMap.  
        /// There is only one exception to this rule, see section 3.3.17.5. 
        /// </summary>
        public string LocalName { get; set; } 


        public string MediaName { get; set; } 

        public MediaRangeInfo MediaRangeInfo { get; set; } // Maps to parent MediaRange
    }
}
