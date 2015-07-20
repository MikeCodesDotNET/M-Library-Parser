using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;

namespace LiteMic.Helpers
{
    static class Extension
    {
        public static string GetAttributeValue(this XElement source, string name)
        {
            var attr = source.Attribute(name);

            if (attr == null) return null;

            return attr.Value;
        }

        public static string GetElementValue(this XElement source, string name)
        {
            var elmt = source.Element(name);

            if (elmt == null) return null;

            return elmt.Value;
        }

        public static bool HaveElement(this XElement source, string name)
        {
            var elmt = source.Element(name);
            
            return elmt != null;
        }

        public static bool HaveAttribute(this XElement source, string name)
        {
            var elmt = source.Attribute(name);

            return elmt != null;
        }

        public static XElement GetElement(this XElement source, string name, bool isRequired)
        {
            var elmt = source.Element(name);

            if (elmt == null && !isRequired) return null;

            if (elmt == null && isRequired) throw new Exception(string.Format("Element '{0}' missing.", name));

            return elmt;
        }

        public static XAttribute GetAttribute(this XElement source, string name, bool isRequired)
        {
            var elmt = source.Attribute(name);

            if (elmt == null && !isRequired) return null;

            if (elmt == null && isRequired) throw new Exception(string.Format("Attribute '{0}' missing.", name));

            return elmt;
        }
    }
}
