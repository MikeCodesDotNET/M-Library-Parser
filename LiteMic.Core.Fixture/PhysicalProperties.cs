using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteMic.Core.Fixture
{
    public class PhysicalProperties
    {

        #region Compulsory
        /// <summary>
        /// What sort of device is it?  Luminaire, accessory, controller, media server, etc
        /// </summary>
        public Enums.PhysicalType Type { get; set; }

        /// <summary>
        /// Does the device have a light source, and if so what type?  None, incandescent, discharge, LED, etc
        /// </summary>
        public Enums.LightGeneration LightGeneration { get; set; }

        /// <summary>
        /// Can the device move, and if so what mechanism does it use?  None, moving yoke, moving mirror, periscope, etc
        /// </summary>
        public Enums.MovementType MovementType { get; set; }

        /// <summary>
        /// What type of beam does the device generate?  None, spot, wash, effect, fibre optic
        /// </summary>
        public Enums.BeamType BeamType { get; set; }
        #endregion

        #region Optional

        private List<Feature> valueMaps;
        public List<Feature> ValueMaps
        {
            get
            {
                if (valueMaps == null)
                    valueMaps = new List<Feature>();
                return valueMaps;
            }
            set
            {
                valueMaps = value;
            }
        }

        private List<Feature> slotMaps;
        public List<Feature> SlotMaps
        {
            get
            {
                if (slotMaps == null)
                    slotMaps = new List<Feature>();
                return slotMaps;
            }
            set
            {
                slotMaps = value;
            }
        }


        //The ColourMixing tag is optional and is only present in colour mixing fixtures. It has two attributes:

        /// <summary>
        /// What basic type of colour mixing mechanism is used?  Additive, subtractive, video.
        /// </summary>
        public Enums.ColourMixing.MixingType ColourMixingType { get; set; }

        /// <summary>
        /// What colours does the mixing mechanism use?  RGB, CMY, RGBA, etc.
        /// 
        /// Note that this value does not refer to the functions of the DMX channels used to control the 
        /// colour mixing mechanism, only to the mechanism itself.  We will not record whether a fixture 
        /// has an intensity channel or mechanism in this attribute.
        /// </summary>
       // public Enums.ColourMixing.MixingSystem MixingSystem { get; set; }

        public Dimension Dimension { get; set; }

        public BeamAngle BeamAngle { get; set; }

        public float FixtureMass { get; set; }

        public CompoundStructure CompoundStructure { get; set; }

        #endregion

    }
}
