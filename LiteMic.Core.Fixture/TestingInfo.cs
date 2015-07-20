using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteMic.Core.Fixture
{
    public class TestingInfo
    {
        public Enums.DmxStatus DmxStatus { get; set; }
        public Enums.RdmStatus RdmStatus { get; set; }
        public string FirmwareVersionTested { get; set; }
        public Enums.ColourMixStatus ColourMixStatus { get; set; }
        public List<string> TestingNotes { get; set; }

    }
}
