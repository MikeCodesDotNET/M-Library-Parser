using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carallon.MLibrary.Models.Misc
{
    public class ParseResult : IParseResult
    {
        private ParseResult()
        {
            FieldsWithError = new Dictionary<string, string>();
            ErrorList = new List<ParseResult>();
        }

        public ParseResult(string element) : this()
        {
            Element = element;
        }

        public string Element { get; set; }

        public Dictionary<string, string> FieldsWithError { get; set; }

        public List<ParseResult> ErrorList { get; private set; }

        public bool HasError {get
        {
            return (ErrorList != null && ErrorList.Count != 0) || (FieldsWithError != null && FieldsWithError.Count != 0);
        }}

        public override string ToString()
        {
            return $"Element {Element} has error";
        }
    }
}
