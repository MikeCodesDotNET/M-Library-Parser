using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteMic.Core.Fixture
{
    /// <summary>
    /// Sample data can be found @\data\gobos\clay_paky\catalogue_0813.xml
    /// </summary>
    public class Media
    {
        // can be alphanumeric. details https://github.com/MichaelJames6/FixtureLibrary/issues/1
        public string Id { get; set; }                                     //Maps to 'MediaId'
        public string Name { get; set; }                                //Maps to 'MediaName'
        public Enums.Litho.BaseImage BaseImage { get; set; }            //Maps to 'BaseImage'
        public Enums.Litho.ImageSize Size { get; set; }                 //Maps to 'ImageSize'
        public string RepeatQuantity { get; set; }                      //Maps to 'RepeatQuantity'
        public Enums.Litho.RepeatPattern RepeatPattern { get; set; }    //Maps to 'RepeatPattern'
        public Enums.Litho.ImageType ImageType {get; set;}              //Maps to 'ImageType'
        public Enums.Litho.ImageColour ImageColour { get; set; }        //Maps to 'ImageColour'
        public string ImageName { get; set; }                           //Maps to image in the root directory
    }
}
