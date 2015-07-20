using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteMic.Core.Fixture.Enums;

namespace LiteMic.Core.Fixture
{
    // \data\fixtures\etc\pearl_42\pearl_42.xml
    public class CompoundStructure
    {
        public int GeometryXCount { get; set; }

        public int GeometryYCount { get; set; }

        public int CellSizeX { get; set; }

        public int CellSizeY { get; set; }

        public CellType? CellShape { get; set; }
    }
}
