using System.Collections.Generic;

namespace Carallon.MLibrary.Fixture
{
    /// <summary>
    ///  Groups together channels into logical groups. Msot fixtures will have just 1 PatchGroup but fixtures like the VL5 have 2. 1 for intensity and a second for other attributes.
    /// </summary>
    public class PatchGroup
    {
        public PatchGroup()
        {
            Channels = new List<Channel>();
        }

        /// <summary>
        /// The Footprint attribute specifies how many DMX channels are in this patchgroup, the total of the patchgroup footprints must equal the DmxFootprint for the fixture.
        /// </summary>
        public int Footprint
        {
            get
            {
                return Channels.Count;
            }
        }

        /// <summary>
        /// The DmxChannel elements within the patch group that follows must exactly account for the footprint size, with no channels duplicated or left undefined. 
        /// </summary>
        public List<Channel> Channels { get; set; }

        /// <summary>
        /// The PatchGroupLabel attribute is compulsory if there is more than one patchgroup, but must not be used otherwise.
        /// </summary>
        public string Label { get; set; }
    }
}
