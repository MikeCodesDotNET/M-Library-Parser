using System.IO;
using System.Linq;
using System.Xml.Linq;

using Carallon.MLibrary.Fixture;
using Carallon.MLibrary.Fixture.Enums;
using Carallon.MLibrary.Fixture.Enums.Litho;
using Carallon.Helpers;
using Carallon.MLibrary.Models.Misc;

namespace Carallon.Parsers
{
    public class MediaParser : BaseParser
    {
        public MediaRange ParseMediaRange(string filename, bool removeTestData = true)
        {
            var directory = Path.GetDirectoryName(filename);

            var files = Directory.GetFiles(directory).ToDictionary(f => Path.GetFileNameWithoutExtension(f), f => Path.GetFileName(f));

            XDocument doc = XDocument.Load(new StreamReader(filename));

            var mediaRange = new MediaRange();

            var root = doc.Element("MediaRange");
            var mediaNodes = root.Elements("Media");

            mediaRange.RevisionHistory = ParseRevisions(root);

            XElement node;
            
            string val;
            Media media;
            ParseResult error;

            string mediaRangePrefix = string.Empty;

            TryExecute(() =>
                       {
                           val = ParseElement<string>(root, "RangeName");
                           Validate(() => Validator.HasValue(val));
                           mediaRange.Name = val;
                       }, "RangeName", mediaRange.Error);

            
            TryExecute(() =>
                       {
                           val = ParseElement<string>(root, "MediaType");
                           if(!Validate(() => Validator.HasValue(val)).IsSuccess) return;
                           mediaRange.MediaType = ParseEnum<MediaType>(val);
                       }, "MediaType", mediaRange.Error);

            TryExecute(() =>
                       {
                           val = ParseAttribute<string>(root, "ManuId");
                           if(!Validate(() => Validator.HasValue(val)).IsSuccess) return;
                           Validate(() => Validator.IsLengthBetween(3, 3, val));
                           Validate(() => Validator.IsMatch("^\\d{3}", val));
                           mediaRange.ManuId = int.Parse(val);
                           mediaRangePrefix = val;
                       }, "ManuId", mediaRange.Error);

            TryExecute(() =>
                       {
                           val = ParseAttribute<string>(root, "RangeId");
                           if(!Validate(() => Validator.HasValue(val)).IsSuccess) return;
                           Validate(() => Validator.IsLengthBetween(3, 3, val));
                           Validate(() => Validator.IsMatch("^\\d{3}", val));
                           mediaRange.RangeId = int.Parse(val);
                           mediaRangePrefix += val;
                       }, "RangeId", mediaRange.Error);

            foreach (XElement mediaNode in mediaNodes)
            {
                media = new Media();
                error = new ParseResult(mediaNode.Name.LocalName);

                TryExecute(() =>
                           {
                               val = ParseAttribute<string>(mediaNode, "MediaId");
                               if(!Validate(() => Validator.HasValue(val)).IsSuccess) return;
                               Validate(() => Validator.IsLengthBetween(2, 3, val));
                               media.Id = GetCanonicalized(val);
                           }, "MediaId", error);

                TryExecute(() =>
                           {
                               val = ParseAttribute<string>(mediaNode, "MediaName");
                               if(!Validate(() => Validator.HasValue(val)).IsSuccess) return;
                               Validate(() => Validator.IsMinLength(1, val));
                               media.Name = val;
                           }, "MediaName", error);

                TryExecute(() =>
                    {
                        // ensure child node exist
                        node = mediaNode.GetElement("LithoImage", true);
                        TryExecute(()
                            =>
                                   {
                                       val = ParseAttribute<string>(node, "BaseImage");
                                       media.LithoImage.BaseImage = ParseEnum<BaseImage>(val);
                                   }, "BaseImage", error);

                        TryExecute(() =>
                                   {
                                       val = ParseAttribute<string>(node, "ImageSize");
                                       media.LithoImage.Size = ParseEnum<ImageSize>(val);
                                   }, "ImageSize", error);

                        TryExecute(() =>
                                   {
                                       val = ParseAttribute<string>(node, "RepeatQuantity");
                                       if (!Validate(() => Validator.HasValue(val)).IsSuccess) return;
                                       media.LithoImage.RepeatQuantity = val;
                                   }, "BaseImage", error);

                        TryExecute(() =>
                                   {
                                       val = ParseAttribute<string>(node, "RepeatPattern");
                                       media.LithoImage.RepeatPattern = ParseEnum<RepeatPattern>(val);
                                   }, "RepeatPattern", error);

                    }, "LithoImage", error);

                TryExecute(() =>
                    {
                        // ensure child node exist
                        node = mediaNode.GetElement("LithoColour", true);

                        TryExecute(()
                            =>
                                   {
                                       val = ParseAttribute<string>(node, "ImageType");
                                       media.LithoColour.ImageType = ParseEnum<ImageType>(val);
                                   }, "ImageType", error);

                        TryExecute(()
                            =>
                                   {
                                       val = ParseAttribute<string>(node, "ImageColour");
                                       media.LithoColour.ImageColour = ParseEnum<ImageColour>(val);
                                   }, "ImageColour", error);

                    }, "LithoColour", error);

                TryExecute(() =>
                           {
                               if (files.ContainsKey(mediaRangePrefix + media.Id))
                                   media.ImageName = files[mediaRangePrefix + media.Id];
                           }, null, null);

                if (error.FieldsWithError.Count > 0)
                    mediaRange.Error.ErrorList.Add(error);
                else
                    mediaRange.Media.Add(media);
            }
            return mediaRange;
        }
    }
}
