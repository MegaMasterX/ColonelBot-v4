using System;
using System.Collections.Generic;
using System.Text;

namespace ColonelBot_v4.Models
{
    class Quote
    {
        public int QuoteID { get; set; }            //The Quote ID, which is callable from the user directly.
        public long QuoteAuthor { get; set; }       //The Author's Discord ID.
        public string QuoteContents { get; set; }   //Contents of the quote. Should be sanitized on add to remove @ tag
        public DateTime QuoteCreated { get; set; }  //DateTime the quote was created for tracking purposes? Neat to have.

    }
}
