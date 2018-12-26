using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

using Discord;
using Discord.Commands;

using ColonelBot_v4.Models;
using ColonelBot_v4.Tools;  

using Newtonsoft.Json;

//This version of the Quotes Module utilizes JSON instead of the other unmaintainable methods.

namespace ColonelBot_v4.Modules
{
    [Group("quote")] // !quote...
    public class QuoteModule : ModuleBase<SocketCommandContext>
    {

        List<Quote> MasterQuoteList;
        Random rnd = new Random(DateTime.Now.Second);

        [Command]
        public async Task QuoteSpecificAsync([Remainder] int quoteID)
        {
            //1. Update the local list for Quotes.
            ResyncQuotesList();

            //2. Locate the quote.
            Quote selectedQuote = null;
            for (int i = 0; i < MasterQuoteList.Count; i++)
            {
                if (MasterQuoteList[i].QuoteID == quoteID) 
                {
                    selectedQuote = MasterQuoteList[i];
                }

            }

            //3. Respond with said quote.
            if (selectedQuote != null)
                await ReplyAsync($"Quote { selectedQuote.QuoteID.ToString()} by {Context.Guild.GetUser(selectedQuote.QuoteAuthor).Nickname}: {selectedQuote.QuoteContents}");
            else
                await ReplyAsync("", embed: EmbedTool.ChannelMessage("The quote you specified is not in the library."));
        }

        [Command, Alias("random")]
        public async Task QuoteRandomAsync()
        {
            //1. Update the local list for Quotes.
            ResyncQuotesList();

            //2. Select a quote that exists in the library.
            Quote selectedQuote = MasterQuoteList[rnd.Next(0, MasterQuoteList.Count)];

            //3. Respond with said quote.
            await ReplyAsync($"Quote { selectedQuote.QuoteID.ToString()} by {Context.Guild.GetUser(selectedQuote.QuoteAuthor).Nickname}: {selectedQuote.QuoteContents}");

        }

        [Command("remove")]
        [RequireContext(ContextType.Guild)]
        public async Task RemoveQuoteAsync([Remainder]int quoteid)
        {
            //1. Pull down the deserialized list.
            ResyncQuotesList();

            //2. Locate the quote in question for verification by enumeration.
            bool QuoteExists = false; //False by default
            for (int i = 0; i < MasterQuoteList.Count; i++)
            {
                if (MasterQuoteList[i].QuoteID == quoteid)
                {//The Quote ID is located. 
                    QuoteExists = true;
                    if (MasterQuoteList[i].QuoteAuthor == Context.User.Id)
                    {//The requestor is the author of the quote.
                        MasterQuoteList.RemoveAt(i); //Remove the quote from the library.
                        WriteQuoteList(); //Write the list to the JSON.
                    } else //The requestor is not the author of the quote.
                        await ReplyAsync("", embed: EmbedTool.ChannelMessage("You are not the author of the quote."));
                }
            }

            //3. Respond based on what was or wasn't found.
            if (QuoteExists)
                await ReplyAsync("", embed: EmbedTool.ChannelMessage("The quote does not exist in the library."));
            else
                await ReplyAsync("", embed: EmbedTool.ChannelMessage($"Quote {quoteid.ToString()} removed from the library."));
            
        }

        [Command("add")]
        [RequireContext(ContextType.Guild)] //Quotes cannot be added via DM.
        public async Task AddQuoteAsync([Remainder] string quote)
        {

            //1. Pull down the deserialized list of Quotes into an array for easier handling.
            ResyncQuotesList();

            //2. Identify the ID of the last quote in the list
            int LastID = MasterQuoteList[MasterQuoteList.Count - 1].QuoteID;

            //3. Sanitize the quote to remove @ tags and add escape characters for anything that would need it.
            //TODO: Build a regex to sanitize the quote before adding it to the list. 

            //4. Generate a new Quote to be added to the list.
            Quote newQuote = new Quote(Context.User.Id, LastID++, quote);

            //5. Add the quote to the list.
            MasterQuoteList.Add(newQuote);

            //6. Reserialize the JSON, writing it to a file.
            WriteQuoteList();

            await ReplyAsync("Added quote " + (LastID + 1).ToString() + " to the Quote Library.");
        }

        public static string QuoteConfigurationFile()
        {
            //1. Verify the Quote file exists.
            string path = Directory.GetCurrentDirectory();
            if (File.Exists(path + "\\Data\\Quotes.json"))
                path += "\\Data\\Quotes.json";
            else
            {//The Quotes file doesn't exist. Create it.
                File.WriteAllText(path + "\\Data\\Quotes.json", "");
                path += "\\Data\\Quotes.json";
            }

            return path;
        }

        /// <summary>
        /// Updates the cached Quote list array with the contents of the JSON.
        /// </summary>
        private void ResyncQuotesList()
        {
            MasterQuoteList = JsonConvert.DeserializeObject<List<Quote>>(File.ReadAllText(QuoteConfigurationFile()));
        }
        /// <summary>
        /// Writes the Quote List to the JSON file.
        /// </summary>
        private void WriteQuoteList()
        {
            File.WriteAllText(QuoteConfigurationFile(), JsonConvert.SerializeObject(MasterQuoteList));

        }
    }
    
}
