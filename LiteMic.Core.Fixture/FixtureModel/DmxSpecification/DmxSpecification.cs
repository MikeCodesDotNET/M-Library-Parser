using Carallon.MLibrary.Models.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carallon.MLibrary.Models.Dmx
{
    public class DmxSpecification : IDmxSpecification
    {
        public int DmxFootprint { get; set; }

        public List<DmxPatchGroup> PatchGroups { get; set; }

        public List<DmxModuleDefinition> ModuleDefinitions { get; set; }
    }
}
