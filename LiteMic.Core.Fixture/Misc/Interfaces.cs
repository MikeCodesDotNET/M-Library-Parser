using System.Collections.Generic;

namespace Carallon.MLibrary.Models.Misc
{
    public interface IParseModel
    {
        List<Revision> RevisionHistory { get; set; }
    }

    public interface IParseResult
    {
        List<ParseResult> ErrorList { get; }
        bool HasError { get; }
    }

   
}
