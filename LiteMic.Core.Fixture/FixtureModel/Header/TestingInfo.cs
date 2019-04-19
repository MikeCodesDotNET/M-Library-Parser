using Carallon.MLibrary.Fixture.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carallon.MLibrary.Models.Header
{
    public class TestingInfo
    {
        public DmxStatus DmxStatus { get; set; }
        public RdmStatus RdmStatus { get; set; }
        public string FirmwareVersionTested { get; set; }
        public ColourMixStatus ColourMixStatus { get; set; }
        public List<string> TestingNotes { get; set; }

    }
}
