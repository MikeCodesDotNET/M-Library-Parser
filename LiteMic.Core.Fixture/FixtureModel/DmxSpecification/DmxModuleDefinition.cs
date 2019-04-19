using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carallon.MLibrary.Models.Dmx
{
    public class DmxModuleDefinition
    {
        public string ModuleName { get; set; }
        public int ModuleFootprint { get; set; }

        public List<DmxChannel> Channels { get; set; }
    }
}
