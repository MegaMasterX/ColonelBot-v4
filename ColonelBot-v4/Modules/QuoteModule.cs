using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

using Discord;
using Discord.Commands;

using ColonelBot_v4.Models;

using Newtonsoft.Json;

//This version of the Quotes Module utilizes JSON instead of the other unmaintainable methods.

namespace ColonelBot_v4.Modules
{
    [Group("quote")] // !quote...
    public class QuoteModule : ModuleBase<SocketCommandContext>
    {
        //[Command, Alias("random")]
        //public async Task QuoteRandomAsync()
        //{

        //}

        

        [Command("add")]
        [RequireContext(ContextType.Guild)] //Quotes cannot be added via DM.
        public async Task AddQuoteAsync([Remainder] string quote)
        {
            
            //1. Pull down the deserialized list of Quotes into an array for easier handling.
            List<Quote> MasterQuotesList = JsonConvert.DeserializeObject<List<Quote>>(File.ReadAllText(QuoteConfigurationFile()));

            //2. Identify the ID of the last quote in the list
            int LastID = MasterQuotesList[MasterQuotesList.Count - 1].QuoteID;

            //3. Sanitize the quote to remove @ tags and add escape characters for anything that would need it.
            //TODO: Build a regex to sanitize the quote before adding it to the list. 

            //4. Generate a new Quote to be added to the list.
            Quote newQuote = new Quote(Context.User.Id, LastID++, quote);

            //5. Add the quote to the list.
            MasterQuotesList.Add(newQuote);

            //6. Reserialize the JSON, writing it to a file.
            File.WriteAllText(QuoteConfigurationFile(), JsonConvert.SerializeObject(MasterQuotesList));

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

    }
    
}
