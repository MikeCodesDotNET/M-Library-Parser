using Carallon.MLibrary.Models.Physical;
using Carallon.MLibrary.Models.Physical.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Carallon.MLibrary.Fixture
{
    public class ElementStructure
    {
        public ElementStructure()
        {
            OpticalSystem = new OpticalSystem();
            FixtureDimensions = new Dimension();
        }

        public PhysicalType PhysicalType { get; set; }
        public OpticalSystem OpticalSystem { get; set; }
        public MovementType MovementType { get; set; }
        public float FixtureMass { get; set; }
        public Dimension FixtureDimensions { get; set; }
    }
}
