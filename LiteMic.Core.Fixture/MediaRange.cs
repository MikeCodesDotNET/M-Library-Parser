using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteMic.Core.Fixture
{
    public class MediaRange : IParseModel
    {
        public int ManuId { get; set; }
        public int RangeId { get; set; }
        public string Name { get; set; }
        public Enums.MediaType MediaType { get; set; }

        private ParseResult _error;

        public ParseResult Error
        {
            get
            {
                if(_error == null)
                    _error = new ParseResult("MediaRange");
                return _error;
            }
            set { _error = value; }
        }

        public bool HaveError{get { return Error.HaveError; }}

        private List<Revision> revisionHistory;
        public List<Revision> RevisionHistory
        {
            get
            {
                if (revisionHistory == null)
                    revisionHistory = new List<Revision>();
                return revisionHistory;
            }
            set
            {
                revisionHistory = value;
            }
        }
        

        private List<Media> media;
        public List<Media> Media
        {
            get
            {
                if (media == null)
                    media = new List<Media>();
                return media;
            }
            set
            {
                media = value;
            }
        }
    }
}
