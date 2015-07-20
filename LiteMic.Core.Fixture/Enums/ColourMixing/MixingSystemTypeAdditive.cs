using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteMic.Core.Fixture.Enums.ColourMixing
{
    //Values that the MixingSystem attribute (within ColourMixing tag) can take if MixingType is Additive
    
    /// <summary>
        /// Following scheme used with additive colour mixing systems

        ///Any of RGB that fixture has
        ///followed by the other colours *in alphabetical order*
        ///followed by the number of sources if >4

        ///Code for colours:
        ///Each colour channel will use a single capital letter, supplemented by one (or possibly more) lower case letters.  If fixture has two channels with the same colour LEDs then use the same letter multiple times.

        ///R  - red
        ///G  - green
        ///B  - blue

        ///A  - amber
        ///C  - cyan
        ///Cw - cool white
        ///Gc - green cyan
        ///In - indigo (use In not I to avoid confusion with intensity)
        ///L  - lime
        ///M  - magenta
        ///Mg - mint green
        ///Mw - medium white
        ///O  - orange
        ///P  - purple
        ///Ro - red orange
        ///Uv - ultra violet
        ///W  - white
        ///Ww - warm white
        ///Y  - yellow
    /// </summary>
    public enum MixingSystemTypeAdditive
    {
        //2 Colours
        AW,             
        GB,             
        RG,             
        CwWw,           

        //3 Colours
        AWB,            
        AWW,            //DTS Titan Solo White
        ACwWw,          //Litecraft outdoor bar WT9 SWA
        RGB,
        AWUv,           //Leader Light LL Stage Wash 600 AWUV
        CwMwWw,         //Color Kinetics iW Fuse Powercore and iW Cove MX Powercore
        RBA,            //Anolis Arcline Outdoor Optic Range

        //4 Colours
        RBInMg,         //ETC S4 LED S2 Daylight
        RGBA,
        RGBW,
        RGAW,
        RGBL,           //ETC ColorSource Par

        //5 Colours
        AWWWW5,         //0energylighting Flexarray 75V
        RGBAC5,         //Reveal CW
        RGBAW5,
        RGBCwWw5,       //PixelRange Pixelsmart
        RGBCIn5,        //D40 Ice
        RGAInRo5,       //D40 Fire
        RBInMgO5,       //ETC S4 LED S2 Tungsten
        RBInMgRo5,      //S4 LED studio HD
        RGBUvW5,        //Light Emotion Flat1012
        RGBWY5,         //Osram Kreios Par 162W

        //6 Colours
        RGBAUvW6Chauvet, //Slimpar HEX 3 IRC
        RGBCOY6,       //Element Labs Kelvin Tile range
        RBACwGcWw6,     //D40 Studio 6
        RGBAInW6,      //Elektralite 1018-AI

        //7 Colours
        RGBACInRo7,
        RGBACInW7,      //D40 Lustr+ 7
        RGBACInL7,       //S4 LED Lustr+ Mk 2
        
        //Other
        Unknown
    }
}
