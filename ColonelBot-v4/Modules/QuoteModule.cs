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
        public async Task QuoteSpecificAsync([Remainder] uint quoteID)
        {
            //1. Update the local list for Quotes.
            ResyncQuotesList();

            //2. Locate the quote.
            Quote selectedQuote = null;
            try
            {
                selectedQuote = MasterQuoteList.Find(x => x.QuoteID == quoteID);
            }
            catch (Exception ex)
            {
                await ReplyAsync(ex.Message);
                throw;
            }

            
            //3. Respond with said quote.
            if (selectedQuote != null)
                if (selectedQuote.QuoteAuthorNickname == null)
                    await ReplyAsync($"Quote { selectedQuote.QuoteID.ToString()} by Library Export: {selectedQuote.QuoteContents}");
                else
                    await ReplyAsync($"Quote { selectedQuote.QuoteID.ToString()} by {selectedQuote.QuoteAuthorNickname}: {selectedQuote.QuoteContents}");
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
            await ReplyAsync($"Quote { selectedQuote.QuoteID.ToString()} by {selectedQuote.QuoteAuthorNickname}: {selectedQuote.QuoteContents}");

        }

       [Command("admin remove"), Alias("admin delete")]
       [RequireUserPermission(GuildPermission.Administrator)] //Admin-only.
       public async Task AdminQuoteRemove([Remainder]int QuoteID)
       {
            ResyncQuotesList();
            for (int i = 0; i < MasterQuoteList.Count; i++)
            {
                if (MasterQuoteList[i].QuoteID == QuoteID)
                {//The Quote ID is located. 
                    
                    MasterQuoteList.RemoveAt(i); //Remove the quote from the library.
                    WriteQuoteList(); //Write the list to the JSON.
                    await ReplyAsync("", embed: EmbedTool.ChannelMessage($"Quote {QuoteID.ToString()} removed from the library by a moderator."));
                }
            }
        }
    


       [Command("remove"), Alias("delete")]
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
                        QuoteExists = true; //Quote was found and deleted.
                    } else //The requestor is not the author of the quote.
                        await ReplyAsync("", embed: EmbedTool.ChannelMessage("You are not the author of the quote."));
                }
            }

            //3. Respond based on what was or wasn't found.
            if (QuoteExists == false)
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
            LastID++;
            Console.WriteLine($"Quote ID: {MasterQuoteList[MasterQuoteList.Count - 1].QuoteID} is the last in the list.");


            //3. Sanitize the quote to remove @ tags and add escape characters for anything that would need it.
            quote = quote.Replace('@', ' '); //Nice try, TREZ.
            //TODO: Build a regex to sanitize the quote before adding it to the list. 

            //4. Generate a new Quote to be added to the list.
            Quote newQuote = new Quote(Context.User.Id, Context.User.Username.Replace('@', ' '), LastID, quote);
            Console.WriteLine($"Attempting to add {newQuote.QuoteID.ToString()} to the library.");

            //5. Add the quote to the list.
            MasterQuoteList.Add(newQuote);

            //6. Reserialize the JSON, writing it to a file.
            WriteQuoteList();

            await ReplyAsync("Added quote " + (LastID).ToString() + " to the Quote Library.");
        }

        public static string QuoteConfigurationFile()
        {
            //1. Verify the Quote file exists.
            string path = Directory.GetCurrentDirectory();
            if (File.Exists($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Data{Path.DirectorySeparatorChar}Quotes.json"))
                path = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Data{Path.DirectorySeparatorChar}Quotes.json";
            else
            {//The Quotes file doesn't exist. Create it.
                File.WriteAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Data{Path.DirectorySeparatorChar}Quotes.json", "");
                path = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Data{Path.DirectorySeparatorChar}Quotes.json";
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
            File.WriteAllText(QuoteConfigurationFile(), JsonConvert.SerializeObject(MasterQuoteList,Formatting.Indented));

        }

        
    }
    
}
