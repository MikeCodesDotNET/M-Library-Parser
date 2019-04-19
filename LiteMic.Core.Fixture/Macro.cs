using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carallon.MLibrary.Fixture
{
    public class Macro
    {
        public string Name { get; set; } //Maps to 'FeatureName' found in DmxMacros (see Mac 2000) 
        public TimeSpan Holdtime { get; set; } //Maps to 'HoldTime'
        public Channel Channel { get; set; } //Map to 'ChannelNum'
        public ushort DmxValue { get; set; }

    }
}
