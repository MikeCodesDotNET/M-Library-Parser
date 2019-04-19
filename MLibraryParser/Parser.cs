using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Carallon.Helpers;
using Carallon.MLibrary.Models.Misc;
using Carallon.MLibrary.Models.Dmx;
using Carallon.MLibrary.Models.Abstractions;

namespace Carallon.Parsers
{
    public partial class Parser
    {
        public static void ParseRevisions(XElement source, IParseModel model)
        {
            model.RevisionHistory = new List<Revision>();
            string val;
            Revision revision;

            var revisionNodes = source.Descendants("Revision");

            foreach (XElement revisionNode in revisionNodes)
            {
                revision = new Revision();
                val = revisionNode.GetAttributeValue("RevisionNum");
                revision.RevisionNum = int.Parse(val);

                val = revisionNode.GetAttributeValue("Date");
                revision.Date = DateTime.ParseExact(val, "dd MMM yyyy", CultureInfo.InvariantCulture);

                revision.Author = revisionNode.GetAttributeValue("Author");
                revision.Text = revisionNode.Value;

                model.RevisionHistory.Add(revision);
            }            
        }

        public static ValidateResult Validate(Func<bool> validator)
        {
            ValidateResult result = new ValidateResult();
            
            if (validator != null)
            {
                try
                {
                    validator();
                }
                catch (Exception e)
                {
                    result.Error = e;
                    throw;
                }
            }

            return result;
        }

        internal static void TryExecute(Action action, string field, ParseResult parseResult)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                if (parseResult != null)
                    parseResult.FieldsWithError[field] = e.Message;
            }
        }

        internal static string GetCanonicalized(string val)
        {
            if (string.IsNullOrWhiteSpace(val)) return string.Empty;

            return val.Replace("/", "-").Replace("\\", "-");
        }

        internal static bool HaveRangeValue(string val)
        {
            return val.Contains("...");
        }

        internal static RangeValue ParseRangeValue(string val)
        {
            var range = new RangeValue();
            ParseRangeValue(range, val);
            return range;
        }

        internal static bool ParseRangeValue(IRangeValue range, string val)
        {
            if (!val.Contains("...")) return false;
            string[] parts = val.Split(new string[] { "..." }, StringSplitOptions.RemoveEmptyEntries);
            range.Start = ParseValue<int>(parts[0]);
            range.End = ParseValue<int>(parts[1]);
            return true;
        }

        internal static int[] ParseDelimited(string val, string delimeter)
        {
            if (!val.Contains(delimeter)) return new int[0];
            string[] parts = val.Split(new string[] { delimeter }, StringSplitOptions.RemoveEmptyEntries);
            return parts.Where(p => !string.IsNullOrWhiteSpace(p))
                .Select(p => ParseValue<int>(p.Trim()))
                .ToArray();
        }

        internal static T ParseEnum<T>(string val) where T : struct 
        {
            try
            {

                string orgVal = val;

                if (val.Contains(" ")) val = val.Replace(" ", "_");
                if (val.Contains("/")) val = val.Replace("/", "_");
                if (val.Contains("+")) val = val.Replace("+", "_");

                var parts = val.Split(new[] { "#" }, StringSplitOptions.RemoveEmptyEntries);

                Enum.TryParse(parts[0], true, out T enumVal);

                if (!Enum.IsDefined(typeof(T), enumVal))
                    throw new Exception(string.Format("Enum '{0}' value '{1}' not defined.", typeof(T).Name, orgVal));

                return enumVal;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Cannot parse enum '{0}' value.", typeof(T).Name));
            }            
        }

        internal static T ParseAttribute<T>(XElement parentElement, string attributName)
        {
            var val = parentElement.GetAttributeValue(attributName);
            return ParseValue<T>(val);
        }

        internal static T ParseElement<T>(XElement parentElement, string elementName)
        {
            var val = parentElement.GetElementValue(elementName);
            return ParseValue<T>(val);
        }

        private static T ParseValue<T>(string val)
        {
            if (string.IsNullOrWhiteSpace(val))
            {                
                return default(T);
            }
            else
            {
                val = val.Trim();
                return (T)Convert.ChangeType(val, typeof(T));
            }
        }

        private static List<string> ParseList(string val)
        {
            if (string.IsNullOrWhiteSpace(val))
            {
                return new List<string>();
            }
            else
            {
                val = val.Trim();
                return val.Split('\n').ToList();
            }
        }
    }
}
