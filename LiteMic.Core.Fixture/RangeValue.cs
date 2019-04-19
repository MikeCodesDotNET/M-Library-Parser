using Carallon.MLibrary.Models.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Carallon.MLibrary.Models.Dmx
{
    public class RangeValue : IRangeValue
    {
        public int Start { get; set; }
        public int End { get; set; }
    }
}
