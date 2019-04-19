using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Carallon.Helpers
{
    public class ValidateResult
    {
        public bool IsSuccess { get { return Error == null; } }
        public Exception Error { get; internal set; }        
    }

    class Validator
    {
        public static bool Conatins<T>(IEnumerable<T> items, Func<T,bool> matchExpr, Func<T,string> message)
        {
            foreach (var item in items)
            {
                if (matchExpr(item))
                    throw new Exception(message(item));
            }

            return false;
        }

        public static bool IsLengthBetween(int? minLength, int? maxLength, string val)
        {
            if (minLength.HasValue)
                IsMinLength(minLength.Value, val);

            if (maxLength.HasValue)
                IsMaxLength(maxLength.Value, val);

            return true;
        }

        public static bool IsMaxLength(int maxLength, string val)
        {
            val = val.Trim();
            if (val.Length > maxLength)
            {
                throw new Exception("Length is greater than " + maxLength);
            }

            return true;
        }

        public static bool IsMinLength(int minLength, string val)
        {
            val = val.Trim();            
            if (val.Length < minLength)
            {
                throw new Exception("Length is less than " + minLength);
            }

            return true;
        }

        public static bool HasValue(string val)
        {
            if (string.IsNullOrWhiteSpace(val))
            {
                throw new Exception("Undefined");                
            }

            return true;
        }

        public static bool ContainIllegelCharacter(string pattern,string val, RegexOptions options = RegexOptions.None)
        {
            if (Regex.IsMatch(val, pattern, options))
                throw new Exception(string.Format("Contains illegal character. Match: {0}", Regex.Match(val, pattern, options).Value));
            
            return true;
        }

        public static bool IsMatch(string pattern, string val)
        {
            if (!Regex.IsMatch(val, pattern))
                throw new Exception(string.Format("Doesn't match pattern. ({0})", pattern));

            return true;
        }
        
    }
}
