using System;
using System.Collections.Generic;
using System.Text;

namespace ColonelBot_v4.Models
{
    class Quote
    {
        public int QuoteID { get; set; }            //The Quote ID, which is callable from the user directly.
        public ulong QuoteAuthor { get; set; }       //The Author's Discord ID.
        public string QuoteContents { get; set; }   //Contents of the quote. Should be sanitized on add to remove @ tag
        public DateTime QuoteCreated { get; set; }  //DateTime the quote was created for tracking purposes? Neat to have.

        /// <summary>
        /// Creates a new quote based on the specified parameters.
        /// </summary>
        /// <param name="Author">The Discord ID of the author. Usually Context.User.Id.</param>
        /// <param name="ID">The index that will be used.</param>
        /// <param name="contents">That sweet, sweet meme.</param>
        public Quote(ulong Author, int ID, string contents)
        {
            QuoteID = ID;
            QuoteAuthor = Author;
            QuoteContents = contents;
            QuoteCreated = DateTime.Now;
        }
    }
}
