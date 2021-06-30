using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Discord.API;

using ColonelBot_v4.Models;
using Newtonsoft.Json;

/// <summary>
/// This tool is used to help filter message content for automoderation purposes.
/// 
/// This tool requires a file to be placed in the Config directory and an entry in the config.json
///    pointing to it.
/// </summary>

namespace ColonelBot_v4.Tools
{
    public class FilteringTool
    {
        List<FilteredTerm> BannedWords = new List<FilteredTerm>();

        /// <summary>
        /// Initializes and loads the filtered word list.
        /// </summary>
        public void InitializeFilterList()
        {

        }

        public bool MessageAllowed(SocketMessage msg)
        {
            //PSUEDO: Split the words in the message up.
            //PSUEDO: Enumerate through the words in the message
            //PSUEDO:  Compare the word to entries in the banned word list (Maybe LINQ where keyword?)
            //PSUEDO:   If a match is found, delete the message and place a log entry in. Return false here to break out of the loop, the message is deleted.
            //PSUEDO:   If not, Fuzzy-Search through the filter again, perhaps with a regex?
            //PSUEDO:       If something's found, delete - log - return false. 
            //PSUEDO:   Continue enumerating
            return true; //Returning true if the message got through this point without being filtered out.
        }

        /// <summary>
        /// This method performs the appropriate moderation action based on the term.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ActionLevel"></param>
        public void PerformAction(SocketMessage msg, FilteredTerm targetTerm)
        {

        }
    }
}
