using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteMic.Core.Fixture
{
    public class DmxModuleDefinition
    {
        public string ModuleName { get; set; }
        public int ModuleFootprint { get; set; }

        public List<DmxChannel> Channels { get; set; }
    }
}
