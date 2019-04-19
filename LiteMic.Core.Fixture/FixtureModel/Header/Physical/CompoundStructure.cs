using Carallon.MLibrary.Fixture.Enums;

namespace Carallon.MLibrary.Models.Physical
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
