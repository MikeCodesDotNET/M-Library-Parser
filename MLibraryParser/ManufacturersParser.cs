using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Carallon.MLibrary.Fixture;
using Carallon.Helpers;
using Carallon.MLibrary.Models.Misc;

namespace Carallon.Parsers
{
    public partial class Parser
    {
        public static Manufacturers ParseManufacturers(string filename, bool removeTestData = true)
        {
            XDocument doc = XDocument.Load(new StreamReader(filename));

            Manufacturers manufacturersRoot = new Manufacturers();

            var root = doc.Element("Manufacturers");

            var manufactrerNodes = root.Elements("Manufacturer");

            ParseRevisions(root, manufacturersRoot);
            List<Manufacturer> manufactrers = new List<Manufacturer>();
            
            string val;
            Manufacturer manufacturer;
            ParseResult error;

            foreach (XElement manufactrerNode in manufactrerNodes)
            {
                manufacturer = new Manufacturer();
                error = new ParseResult(manufactrerNode.Name.LocalName);

                TryExecute(
                    () =>
                    {
                        val = ParseAttribute<string>(manufactrerNode, "ManuId");

                        if (Validate(() => Validator.HasValue(val)).IsSuccess)
                        {
                            Validate(() => Validator.IsLengthBetween(3, 3, val));
                            Validate(() => Validator.IsMatch("^\\d{3}", val));

                            Validate(() =>
                                Validator.Conatins(manufactrers, (m) => m.ManuId.ToString() == val,
                                    m => string.Format("Same ManuId as {0}", m.ManufacturerName)));

                            manufacturer.ManuId = int.Parse(val);
                        }
                    }, "ManuId", error);

                TryExecute(
                    () =>
                    {
                        val = ParseElement<string>(manufactrerNode, "ManufacturerName");

                        if (Validate(() => Validator.HasValue(val)).IsSuccess)
                        {
                            Validate(() => Validator.IsMinLength(1, val));

                            Validate(() =>
                                Validator.Conatins(manufactrers, (m) => m.ManufacturerName == val,
                                    m => string.Format("Same ManufacturerName as {0}", m.ManuId)));

                            manufacturer.ManufacturerName = val;
                        }
                    }, "ManufacturerName", error);

                TryExecute(
                    () =>
                    {
                        val = ParseElement<string>(manufactrerNode, "ManufacturerName_8");
                        if (Validate(() => Validator.HasValue(val)).IsSuccess)
                        {
                            Validate(() => Validator.IsLengthBetween(1, 8, val));

                            Validate(() =>
                                Validator.Conatins(manufactrers, (m) => m.ManufacturerName8 == val,
                                    m => string.Format("Same ManufacturerName_8 as {0}", m.ManuId)));

                            manufacturer.ManufacturerName8 = val;
                        }
                    }, "ManufacturerName_8", error);

                TryExecute(
                    () =>
                    {
                        val = ParseElement<string>(manufactrerNode, "ManufacturerName_3");
                        if (Validate(() => Validator.HasValue(val)).IsSuccess)
                        {
                            Validate(() => Validator.IsLengthBetween(1, 3, val));

                            Validate(() =>
                                Validator.Conatins(manufactrers, (m) => m.ManufacturerName3 == val,
                                    m => string.Format("Same ManufacturerName_3 as {0}", m.ManuId)));

                            manufacturer.ManufacturerName3 = val;
                        }
                    }, "ManufacturerName_3", error);

                TryExecute(
                    () =>
                    {
                        val = ParseElement<string>(manufactrerNode, "ManufacturerDirectory");
                        if (!Validate(() => Validator.HasValue(val)).IsSuccess) return;
                        Validate(() => Validator.IsMinLength(1, val));

                        if (Regex.IsMatch(val, "[A-Z]", RegexOptions.None))
                            throw new Exception("Contains capital letter");

                        if (val.Contains(" "))
                            throw new Exception("Contains space letter");

                        Validate(() => Validator.ContainIllegelCharacter("[^a-z0-9_]", val));

                        Validate(() =>
                            Validator.Conatins(manufactrers, (m) => m.ManufacturerDirectory == val,
                                m => string.Format("Same ManufacturerDirectory as {0}", m.ManuId)));

                        manufacturer.ManufacturerDirectory = val;
                    }, "ManufacturerDirectory", error);

                TryExecute(()
                    =>
                           {
                               val = ParseElement<string>(manufactrerNode, "ESTAId");

                               // optinal element
                               if (!string.IsNullOrWhiteSpace(val))
                               {
                                   Validate(() => Validator.IsLengthBetween(4, 4, val));

                                   Validate(() => Validator.ContainIllegelCharacter("[^A-F0-9]", val));

                                   Validate(() =>
                                       Validator.Conatins(manufactrers, (m) => m.ESTAId == val,
                                           m => string.Format("Same ESTAId as {0}", m.ManuId)));

                                   manufacturer.ESTAId = val;
                               }
                           }, "ESTAId", error);

                if (error.FieldsWithError.Count > 0)
                    manufacturersRoot.Error.ErrorList.Add(error);
                else
                    manufactrers.Add(manufacturer);
            }

            if (removeTestData)
            {
                manufactrers = manufactrers.Where(x => x.ManuId < 900).ToList();
            }

            manufacturersRoot.ManufacturerList = manufactrers;            

            return manufacturersRoot;
        }
    }
}
