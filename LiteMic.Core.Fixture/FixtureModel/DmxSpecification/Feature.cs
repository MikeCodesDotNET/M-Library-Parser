/*      
 *   [compulsory single entry, optional multiples]
 *   Holder for the value elements of a feature.  There must be at least one feature declared inside each DmxModule tag.  
 *   If there are no DmxModule tags there must be at least one feature declared in the ValueMap.
 *   FeatureName – The name of the feature (with unit type Real) defined in features.xml (e.g. Colour Wheel Spin#2).
*/

using System.Collections.Generic;
using Carallon.MLibrary.Fixture.Enums;
using Carallon.MLibrary.Models;

namespace Carallon.MLibrary.Fixture
{
    public class Feature
    {
        public Feature()
        {
            Specs = new List<SpecValue>();
            Slots = new List<Slot>();
            Enums = new List<EnumValue>();
        }

        public FeatureName FeatureName { get; set; }

        /// <summary>
        /// The feature's unit type tends to decide how to form a label for a FeatureRange:
        /// 
        /// ·	If the feature's unit is the Unary unit, the label will just be the client mapped Feature name, e.g. Fixture Control Reset might map to "reset"
        /// 
	    /// ·	If the feature's unit is the SlotEntry unit, the slot label should be taken from the SlotMap lookup at the top of the personality file using the <UnitValue> entry as the key, 
        ///     with the wheel percentage appended (e.g. "gobo2@50%" might map to "breakup 50%")
        ///     
	    /// ·	If the feature's unit type is discrete, the label should be formed from the feature name mapping followed by the discrete <UnitVal> label, e.g. "laser off".  
        ///     Particular cases may be hard coded to drop parts of the string, e.g. "shutter open" -> "open", "laser on" -> "laser". These rules will be dependent on target library specification.
        ///     
    	/// ·	If the feature's unit type is continuous (and not SlotEntry), the label should be formed from the feature name mapping followed by the continuous value or range of values.  
        ///     Again, some hard coded rules may be introduced to drop ranges spanning the entire space to clarify labelling, e.g. "magenta 0-100%" -> "magenta".
        /// </summary>
        public UnitType? UnitName { get; set; }
        public WheelType? WheelType { get; set; }

        /// <summary>
        /// Tag to map a real world value of a feature.
        /// There must be at least one SpecValue or CalibratedValue tag in each Feature tag.
        /// </summary>
        public List<SpecValue> Specs { get; set; }
        public int SlotCount { get; set; }

        public List<Slot> Slots { get; set; }
        public List<EnumValue> Enums { get; set; }

        public ResponseTime ResponseTime { get; set; }


        /// <summary>
        /// Validate if the feature conforms to MLibrary rules. 
        /// </summary>
        public bool Valid
        {
            get
            {
                return IsValid();
            }
        }

        /// <summary>
        /// Model validation rules.
        /// </summary>
        /// <returns></returns>
        bool IsValid()
        {
            if (Specs.Count < 1)
                return false;

            return true;
        }
    }
}
