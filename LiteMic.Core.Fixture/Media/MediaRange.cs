using Carallon.MLibrary.Models.Misc;
using System.Collections.Generic;


namespace Carallon.MLibrary.Fixture
{
    /// <summary>
    /// All slots which have media from the same media range file fitted have their Slot tags grouped together within a MediaRange tag which points to that range file.  
    /// The MediaRange file specifies the media type, manufacturer and range.  
    /// The Slot tags within a MediaRange tag have an additional attribute, the MediaName, which points to the specific piece of media from that range that is fitted.  
    /// The MediaName is the link between the SlotMap and the individual entry in the range file.  A wheel, scroller etc can have several media range tags if needed.
    /// Where a fixture is supplied without gobos, colours etc fitted, the media range definition in the SlotMap currently has the manufacturer and range entries set to “NA” 
    /// to indicate that there is no associated range file, together with the MediaName in the Slot tags.
    /// This is being changed, and we will in future have all slot tags that do not refer to something in a media range file will be moved so they are outside the MediaRange tags.
    /// They will loose their MediaName attributes at the same time.
    /// </summary>
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

        public bool HaveError{get { return Error.HasError; }}

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
