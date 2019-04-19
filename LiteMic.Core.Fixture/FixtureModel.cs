using Carallon.MLibrary.Models.Dmx;
using Carallon.MLibrary.Models.Header;
using Carallon.MLibrary.Models.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carallon.MLibrary.Models
{
    public class FixtureModel 
    {
        /// <summary>
        /// ID used in exported Fixture Library
        /// </summary>
        public int Id { get; set; }

        public FixtureHeader Header { get; set; }

        public string PersonalityUUID { get; set; }
        public bool Discontinued { get; set; }
        public bool DefaultMode { get; set; }

        public int ManuId { get; set; }
        public int ModelId { get; set; }
        public int ModeId { get; set; }
        public int ModeId2 { get; set; }

        public bool HasGobos { get; set; }

        private List<Slot> slots;
        public List<Slot> Slots
        {
            get
            {
                if (slots == null)
                    slots = new List<Slot>();
                return slots;
            }
            set
            {
                slots = value;
            }
        }
        
        public RdmSpecification RdmSpefication { get; set; }

        public DmxSpecification DmxSpecification { get; set; }
    }
}
