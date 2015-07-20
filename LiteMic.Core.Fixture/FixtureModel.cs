using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteMic.Core.Fixture
{
    public class FixtureModel : IParseModel
    {
        public FixtureModel()
        {
            Error = new ParseResult("FixtureModel");
        }

        public ParseResult Error { get; set; }

        public int ManuId { get; set; }
        public int ModelId { get; set; }
        public int ModeId { get; set; }
        public int ModeId2 { get; set; }

        /// <summary>
        /// Manufacturers occasionally rename fixtures, keeping them identical in every other respect.  
        /// This is typically for marketing reasons (eg Martin changed the Mac 700 to the Mac 700 Profile 
        /// when they launched the Mac 700 Wash).  Where this happens, the personality will keep the same 
        /// manufacturer, model and mode IDs, but the ModelName (and hence filename) will change to the more 
        /// recent name.  The previous name(s) will be stored in the AlternativeModelName tag.  This tag will 
        /// also be used if the manufacturer has two ways of referring to a fixture, such as the Chauvet ColorSplash 
        /// 196 which is also referred to as the LED-Par 196.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A two-digit code assigned to a particular mode of this fixture model.  It is again unique within its product. 
        /// If the fixture superseded, the DmxStatus will be set to "Superseded" To make it doubly clear which personality 
        /// to use (if they are not removed from console libraries) we will also add "- Superseded" to the mode name.  
        /// The filename will also have "_superseded" appended.
        /// </summary>
        public string ModeName { get; set; }

        public string PersonalityName { get; set; }
        public string DocumentationVersion { get; set; }

        public string UserNotes { get; set; }

        public List<string> KnownIssues { get; set; }

        public bool HasGobos
        {
            get
            {
                if (Slots.Count != 0)
                    return true;
                return false;
            }
        }

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

        public PhysicalProperties Physical { get; set; }

        public TestingInfo TestingInfo { get; set; }

        private List<Revision> revisionHistory;
        public List<Revision> RevisionHistory
        {
            get
            {
                if (revisionHistory == null)
                    revisionHistory = new List<Revision>();
                return revisionHistory;
            }
            set
            {
                revisionHistory = value;
            }
        }

        public RdmSpecification RdmSpefication { get; set; }

        public  DmxSpecification DmxSpecification { get; set; }
    }
}
