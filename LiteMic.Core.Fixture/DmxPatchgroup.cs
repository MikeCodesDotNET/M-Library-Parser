using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteMic.Core.Fixture
{
    public class DmxPatchGroup
    {
        public DmxPatchGroup()
        {
            Channels = new List<DmxChannel>();
            Macros = new List<DmxMacro>();
        }

        public int PatchFootprint { get; set; }
        public string PatchGroupLabel { get; set; }
        public List<DmxChannel> Channels { get; set; }
        public List<DmxMacro> Macros { get; set; }
    }
}
