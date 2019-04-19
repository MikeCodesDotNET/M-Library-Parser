using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carallon.MLibrary.Fixture
{
    /// <summary>
    /// Sample data can be found @\data\gobos\clay_paky\catalogue_0813.xml
    /// </summary>
    public class Media
    {
        public Media()
        {
            LithoColour = new LithoColour();
            LithoImage = new LithoImage();
        }

        public string Id { get; set; }                                     //Maps to 'MediaId'
        public string Name { get; set; }                                //Maps to 'MediaName'       
        public LithoColour LithoColour { get; set; }
        public LithoImage LithoImage { get; set; }
        public string ImageName { get; set; }                           //Maps to image in the root directory
    }
}
