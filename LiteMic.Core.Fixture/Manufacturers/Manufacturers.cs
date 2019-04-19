using Carallon.MLibrary.Models.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carallon.MLibrary.Fixture
{
    public class Manufacturers : IParseModel
    {
        public Manufacturers()
        {
            Error = new ParseResult("Manufacturers");
        }

        public List<Revision> RevisionHistory { get; set; }
        public List<Manufacturer> ManufacturerList { get; set; }
        public ParseResult Error { get; set; }        
    }
}
