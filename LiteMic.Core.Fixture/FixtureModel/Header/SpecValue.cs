/*
*   There are two tags for recording unit values, the SpecValue and the CalibratedValue tags.  
*   These indicate the source of the real world data.
*   ·	A SpecValue is taken directly from the user manual for the fixture.
*   ·	A CalibratedValue has been measured by Carallon.                                                    
*   
*   Note that Carallon cannot guarantee the accuracy of this data.  
*   There must be at least one SpecValue or CalibratedValue tag in each Feature tag.
*
*/

namespace Carallon.MLibrary.Fixture
{
    //Tag to map a real world value of a feature from manufacturer supplied information to the associated percentage value used in the DMX specification.
    public class SpecValue : RealWorldValue
    {
    }

    //Tag to map a real world value of a feature measured by Carallon to the associated percentage value used in the DMX specification.
    public class CalibratedValue : RealWorldValue
    {
    }

    //Base Implementation of RealWorld Value 
    public class RealWorldValue
    {
        public float PercentValue { get; set; }
        public float UnitValue { get; set; }
    }

}
