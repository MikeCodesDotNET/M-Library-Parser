using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteMic.Core.Fixture
{
    public class DmxSpecification
    {
        public int DmxFootprint { get; set; }

        public List<DmxPatchGroup> PatchGroups { get; set; }

        public List<DmxModuleDefinition> ModuleDefinitions { get; set; }
    }
}
