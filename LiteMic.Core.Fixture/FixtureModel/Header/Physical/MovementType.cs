/*
*   If and how the fixture moves, from the following list – defined in spec_enums.xml:
*   ·	None – The fixture cannot move, e.g. a conventional fixture, an accessory, a smoke* machine etc
*   ·	Manual – The fixture can move while in use, but not by DMX control.  This is typically for followspots.
*   ·	Mirror – Beam position is set by a tilting mirror
*   ·	Yoke – Beam position is set by moving the head on a yoke
*   ·	Split Pedestal – Beam position is set by moving the head on a split pedestal (see glossary)
*   ·	Periscope – Beam position is set by a periscopic head                                                  
*/

namespace Carallon.MLibrary.Models.Physical.Enums
{
    public enum MovementType
    {
        None,
        Manual,
        Mirror,
        Yoke,
        Periscope,
        SplitPedestal,
        Caterpillar,
        Pan,
        PanOffset,
        Tilt,
        Unknown
    }
}
