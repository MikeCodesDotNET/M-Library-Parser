using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteMic.Core.Fixture
{
    public class PatchGroup
    {

        public PatchGroup()
        {
            Channels = new List<Channel>();
        }

        public int Footprint
        {
            get
            {
                return Channels.Count;
            }
        }

        public List<Channel> Channels { get; set; }
    }
}
