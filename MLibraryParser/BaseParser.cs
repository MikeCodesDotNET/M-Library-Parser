using Carallon.Helpers;
using Carallon.MLibrary.Models.Abstractions;
using Carallon.MLibrary.Models.Dmx;
using Carallon.MLibrary.Models.Misc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace Carallon.Parsers
{
    public class BaseParser
    {
        public void TryExecute(Action action, string field, ParseResult parseResult)
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

        public List<Revision> ParseRevisions(XElement source)
        {
            var results = new List<Revision>();
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
                revision.Text = revisionNode.Value.Trim();

                results.Add(revision);
            }
            return results;
        }

        public ValidateResult Validate(Func<bool> validator)
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

        public string GetCanonicalized(string val)
        {
            if (string.IsNullOrWhiteSpace(val)) return string.Empty;

            return val.Replace("/", "-").Replace("\\", "-");
        }

        public bool HasRangeValue(string val)
        {
            return val.Contains("...");
        }

        public RangeValue ParseRangeValue(string val)
        {
            var range = new RangeValue();
            ParseRangeValue(range, val);
            return range;
        }

        public bool ParseRangeValue(IRangeValue range, string val)
        {
            if (!val.Contains("...")) return false;
            string[] parts = val.Split(new string[] { "..." }, StringSplitOptions.RemoveEmptyEntries);
            range.Start = ParseValue<int>(parts[0]);
            range.End = ParseValue<int>(parts[1]);
            return true;
        }

        public int[] ParseDelimited(string val, string delimeter)
        {
            if (!val.Contains(delimeter)) return new int[0];
            string[] parts = val.Split(new string[] { delimeter }, StringSplitOptions.RemoveEmptyEntries);
            return parts.Where(p => !string.IsNullOrWhiteSpace(p))
                .Select(p => ParseValue<int>(p.Trim()))
                .ToArray();
        }

        public T ParseEnum<T>(string val) where T : struct
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

        public T ParseAttribute<T>(XElement parentElement, string attributName)
        {
            var val = parentElement.GetAttributeValue(attributName);
            return ParseValue<T>(val);
        }

        public T ParseElement<T>(XElement parentElement, string elementName)
        {
            var val = parentElement.GetElementValue(elementName);
            return ParseValue<T>(val);
        }

        public T ParseValue<T>(string val)
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

        public List<string> ParseList(string val)
        {
            if (string.IsNullOrWhiteSpace(val))
            {
                return new List<string>();
            }
            else
            {
                val = val.Trim();
                var tempList = new List<string>();
                var splitList = val.Split('\n').ToList();
                foreach(var line in splitList)
                {
                    tempList.Add(line.Trim());
                }
                return tempList;
            }
        }

    }
}
