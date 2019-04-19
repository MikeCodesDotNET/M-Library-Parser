using Carallon.MLibrary.Fixture;
using Carallon.MLibrary.Models.Header;
using Carallon.MLibrary.Models.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carallon.MLibrary.Models.Header
{
    public class FixtureHeader
    {
        public FixtureHeader()
        {
            PhysicalProperties = new PhysicalProperties();
            KnownIssues = new List<string>();
            UserNotes = new List<string>();
            TestingInfo = new TestingInfo();
            Revisions = new List<Revision>();
        }

        public string ModelName { get; set; }
        public string ModeName { get; set; }

        public string PersonalityName { get; set; }
        
        public string DocumentationVersion { get; set; }

        public ElementStructure ElementStructure { get; set; }
        public PhysicalProperties PhysicalProperties { get; set; }

        public List<string> UserNotes { get; set; }

        public List<string> KnownIssues { get; set; }

        public TestingInfo TestingInfo { get; set; }

        /// <summary>
        /// List of all the changes to the original personality XML definition
        /// </summary>
        public List<Revision> Revisions { get; set; }
    }
}
