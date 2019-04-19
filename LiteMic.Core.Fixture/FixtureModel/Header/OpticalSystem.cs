using Carallon.MLibrary.Fixture.Enums.ColourMixing;
using Carallon.MLibrary.Models.Enums.ColourMixing;
using Carallon.MLibrary.Models.Physical.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carallon.MLibrary.Fixture
{
    public class OpticalSystem
    {        
        public LightGeneration SourceType { get; set; }
        public MixingType ColourMixingType { get; set; }
        public MixingSystem ColourMixingSystem { get; set; }
        public BeamType BeamType { get; set; }
        public int BeamAngle { get; set; }
    }
}
