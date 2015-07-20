using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteMic.Core.Fixture
{
    public interface IParseModel
    {
        List<Revision> RevisionHistory { get; set; }
    }

    public interface IParseResult
    {
        List<ParseResult> ErrorList { get; }

        bool HaveError { get; }
    }

    public interface IRangeValue
    {
        int Start { get; set; }
        int End { get; set; }
    }
}
