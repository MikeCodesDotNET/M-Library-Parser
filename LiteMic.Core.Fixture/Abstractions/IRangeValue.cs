﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carallon.MLibrary.Models.Abstractions
{
    public interface IRangeValue
    {
        int Start { get; set; }
        int End { get; set; }
    }
}
