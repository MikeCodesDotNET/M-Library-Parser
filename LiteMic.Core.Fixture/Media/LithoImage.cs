using Carallon.MLibrary.Fixture.Enums.Litho;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carallon.MLibrary.Fixture
{
    public class LithoImage
    {
        public string ImageBase64 { get; set;  }            //Base64 of lithoimage (not in the Mlib).

        public BaseImage BaseImage { get; set; }            //Maps to 'BaseImage'
        public ImageSize Size { get; set; }                 //Maps to 'ImageSize'
        public string RepeatQuantity { get; set; }                      //Maps to 'RepeatQuantity'
        public RepeatPattern RepeatPattern { get; set; }    //Maps to 'RepeatPattern'
    }
}
