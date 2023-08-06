using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

using Discord;
using Discord.Interactions;

using ColonelBot_v4.Models;
using ColonelBot_v4.Tools;  

using Newtonsoft.Json;
using Discord.WebSocket;
using Discord.Rest;

//This version of the Quotes Module utilizes JSON instead of the other unmaintainable methods.

namespace ColonelBot_v4.Modules
{
    [Group("quote", "Obtain a quote from the N1GP Meme Vault:tm:")] // !quote...
    public class QuoteModule : InteractionModuleBase<SocketInteractionContext>
    {
        List<Quote> MasterQuoteList;
        Random rnd = new Random();
        DiscordSocketClient discordclient;
        public List<Quote> QuotesPendingApproval = new List<Quote>();

        [SlashCommand("specific","Pulls a specific quote by ID.")]
        public async Task QuoteSpecificAsync(uint QuoteID)
        {
            //1. Update the local list for Quotes.
            ResyncQuotesList();

            //2. Locate the quote.
            Quote selectedQuote = null;
            try
            {
                selectedQuote = MasterQuoteList.Find(x => x.QuoteID == QuoteID);
            }
            catch (Exception ex)
            {
                await RespondAsync(ex.Message);
                throw;
            }

            
            //3. Respond with said quote.
            if (selectedQuote != null)
                if (selectedQuote.QuoteAuthorNickname == null)
                    await RespondAsync($"Quote { selectedQuote.QuoteID.ToString()}: {selectedQuote.QuoteContents}");
                else
                    await RespondAsync($"Quote { selectedQuote.QuoteID.ToString()} by {selectedQuote.QuoteAuthorNickname}: {selectedQuote.QuoteContents}");
            else
                await RespondAsync("", embed: EmbedTool.ChannelMessage("The quote you specified is not in the library."));
        }


        [SlashCommand("random","Gets a random quote.")]
        public async Task QuoteRandomAsync()
        {
            //1. Update the local list for Quotes.
            ResyncQuotesList();

            //2. Reseed RNG and Select a quote that exists in the library.
            
            Quote selectedQuote = MasterQuoteList[rnd.Next(0, MasterQuoteList.Count)];

            //3. Respond with said quote.
            if (selectedQuote.QuoteAuthorNickname == null)
                await RespondAsync($"Quote { selectedQuote.QuoteID.ToString()}: {selectedQuote.QuoteContents}");
            else
                await RespondAsync($"Quote { selectedQuote.QuoteID.ToString()} by {selectedQuote.QuoteAuthorNickname}: {selectedQuote.QuoteContents}");

        }

       [SlashCommand("admin-remove", "Admin Only, Forcefully removes a quote from the library")]
       [ModeratorOnly] //Admin-only.
       public async Task AdminQuoteRemove(int QuoteID)
       {
            ResyncQuotesList();
            for (int i = 0; i < MasterQuoteList.Count; i++)
            {
                if (MasterQuoteList[i].QuoteID == QuoteID)
                {//The Quote ID is located. 
                    
                    MasterQuoteList.RemoveAt(i); //Remove the quote from the library.
                    WriteQuoteList(); //Write the list to the JSON.
                    await RespondAsync("", embed: EmbedTool.ChannelMessage($"Quote {QuoteID.ToString()} removed from the library by a moderator."));
                } else  // The Quote ID is not located.
                    await RespondAsync("", embed: EmbedTool.ChannelMessage($"Quote {QuoteID.ToString()} does not exist in the library."));
            }
        }
    


        [SlashCommand("remove", "Removes a quote from the library if you're the author.")]
        [RequireContext(ContextType.Guild)]
        public async Task RemoveQuoteAsync(int QuoteID)
        {
            //1. Pull down the deserialized list.
            ResyncQuotesList();

            //2. Locate the quote in question for verification by enumeration.
            bool QuoteExists = false; //False by default
            for (int i = 0; i < MasterQuoteList.Count; i++)
            {
                if (MasterQuoteList[i].QuoteID == QuoteID)
                {//The Quote ID is located. 
                    QuoteExists = true;
                    if (MasterQuoteList[i].QuoteAuthor == Context.User.Id)
                    {//The requestor is the author of the quote.
                        MasterQuoteList.RemoveAt(i); //Remove the quote from the library.
                        WriteQuoteList(); //Write the list to the JSON.
                        QuoteExists = true; //Quote was found and deleted.
                    } else //The requestor is not the author of the quote.
                        await RespondAsync("", embed: EmbedTool.ChannelMessage("You are not the author of the quote."));
                }
            }

            //3. Respond based on what was or wasn't found.
            if (QuoteExists == false)
                await RespondAsync("", embed: EmbedTool.ChannelMessage("The quote does not exist in the library."));
            else
                await RespondAsync("", embed: EmbedTool.ChannelMessage($"Quote {QuoteID.ToString()} removed from the library."));
            
        }


        [SlashCommand("add", "Adds a quote to the library")]
        [RequireContext(ContextType.Guild)] //Quotes cannot be added via DM.
        public async Task AddQuoteAsync(string QuoteContents)
        {

            //1. Pull down the deserialized list of Quotes into an array for easier handling.
            ResyncQuotesList();                                                                                                         //Sync the cached list with the on-disk JSON.

            //2. Identify the ID of the last quote in the list
            
            int LastID = MasterQuoteList[MasterQuoteList.Count - 1].QuoteID;                                                            //The ID is determined by the ID of the last quote in the library.

            if (QuotesPendingApproval.Count == 0)
                LastID++;                                                                                                                   //Increment the ID up by 1.
            else
            {
                LastID = QuotesPendingApproval[QuotesPendingApproval.Count - 1].QuoteID + 1;
            }


            Console.WriteLine($"Quote ID: {MasterQuoteList[MasterQuoteList.Count - 1].QuoteID} is the last in the list.");              //Log the last ID.


            //3. Sanitize the quote to remove @ tags and add escape characters for anything that would need it.
            QuoteContents = QuoteContents.Replace('@', ' ');                                                                                            //Remove @ tags, preventing the bot and users from calling @everyone or @roles
            //TODO: Build a regex to sanitize the quote before adding it to the list. 


            //4. [Rewrite] Reply with a message informing the caller that its been sent for moderator review and approval.
            await RespondAsync("Your e-mail has been sent!");                                                                           //Reply to the caller letting them know the quote was sent off for approval.
            QuotesPendingApproval.Add(new Quote(Context.User.Id, Context.User.Username.Replace('@', ' '), LastID, QuoteContents));             //Place the quote in the approval queue.
                                                                                                                                       //5. Send a mesage to the channel and request that it be added

            await SendQuoteForApprovalAsync(Context, Context.User, QuoteContents, LastID);                                                      //Place the Approval Quote in the appropriate channel.

            



            //PSUEDO: YOU STOPPED HERE <=========================================================================================>


            ////4. Generate a new Quote to be added to the list.
            //Quote newQuote = new Quote(Context.User.Id, Context.User.Username.Replace('@', ' '), LastID, quote);
            //Console.WriteLine($"Attempting to add {newQuote.QuoteID.ToString()} to the library.");

            ////5. Add the quote to the list.
            //MasterQuoteList.Add(newQuote);

            ////6. Reserialize the JSON, writing it to a file.
            //WriteQuoteList();

            //await ReplyAsync("Added quote " + (LastID).ToString() + " to the Quote Library.");
        }

        private async Task ListenForQuoteApproval(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            //Emote confirmEmote = Emote.Parse("<:qbotDENY:858130936376590407>");
            //Emote declineEmote = Emote.Parse("<:qbotAPPROVE:858130946985689088>");
            
            IMessage msg = await arg2.GetMessageAsync(arg1.Id); //This works.  This gets the appropriate message.
            if (msg != null)
            {
                try
            {
                foreach (var item in msg.Reactions)
                {
                    if (item.Key.Name == "qbotAPPROVE" && item.Value.ReactionCount == 2)
                    {
                        //Get the quote ID from the string.
                        string[] QuoteData = msg.Content.Split('\n');
                        string QuoteID = QuoteData[2].Split(' ')[2]; //Pulls the quote ID from the 2nd line.
                            Quote targetQuote = QuotesPendingApproval.Find(x => x.QuoteID == Convert.ToInt32(QuoteID));
                            //Add the quote to the list based on the last ID of the actual quote library.
                            targetQuote.QuoteID = MasterQuoteList[MasterQuoteList.Count - 1].QuoteID + 1;
                            await RespondAsync($"Yes! A new Quote ({QuoteID}) is born!\nID: {targetQuote.QuoteID}\n\n{targetQuote.QuoteContents}");
                        await arg2.DeleteMessageAsync(msg); //Delete the message in the Colonel Quote Audit Channel. You already have the ID.
                                                            //Time to add the quote.
                       
                        QuotesPendingApproval.Remove(targetQuote);
                        MasterQuoteList.Add(targetQuote);
                        WriteQuoteList();
                        //discordclient.ReactionAdded -= ListenForQuoteApproval;

                    }

                    else if (item.Key.Name == "qbotDENY" && item.Value.ReactionCount == 2)
                    {
                        string[] QuoteData = msg.Content.Split('\n');
                        string QuoteID = QuoteData[2].Split(' ')[2]; //Pulls the quote ID from the 2nd line.
                                                                     //await ReplyAsync($"Quote {QuoteID} was denied by a moderator (Method 2)");
                        await arg2.DeleteMessageAsync(msg); //Delete the message in the Colonel Quote Audit Channel. You already have the ID.
                        //discordclient.ReactionAdded -= ListenForQuoteApproval;
                    }

                }
            }
            catch (Exception)
            {
                //discordclient.ReactionAdded -= ListenForQuoteApproval;
            }
           
            
           
            }
            
            
        }




        //This method, when called from the Reaction event, adds the quote to the library as it was approved.
        public void ApproveQuote(Quote quote)
        {
            MasterQuoteList.Add(quote);
            WriteQuoteList();
            //PSUEDO: Send a message to the #memes channel with the message - "<:CobChamp:594961555179962368> Yeah! A new quote is born! <:lilguy:297934732925337601>" and the contents of the quote. No user ping.
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

       

        /// <summary>
        /// This method asynchronously sends a message to the quote approval chat channel.
        /// </summary>
        /// <returns></returns>
        private async Task SendQuoteForApprovalAsync(SocketInteractionContext context, SocketUser caller, string QuoteContents, int QuoteID)

        {
            SocketTextChannel chan = context.Guild.GetTextChannel(ulong.Parse(BotTools.GetSettingString(BotTools.ConfigurationEntries.QuoteReportChannelID)));
            var Caller = caller as SocketGuildUser;
            //IEmote confirmEmote = caller.Guild.GetEmoteAsync()
            Emote confirmEmote = Emote.Parse("<:qbotDENY:858130936376590407>");
            Emote declineEmote = Emote.Parse("<:qbotAPPROVE:858130946985689088>");
            Emote ChonkboiL = Emote.Parse("<:qbotBUFFER:858131961939361834>");
            Emote ChonkboiR = Emote.Parse("<:qbotBUFFERER:858131975323779153>");
            Emote TFC = Emote.Parse("<:TFC:297934728470855681>");
            var sent =  await chan.SendMessageAsync($"__Quote Add Request__\n" +
                $"**Requestor**: {Caller.Nickname} ({Caller.Username} - {Caller.Id})\n" +
                $"**Quote**: {QuoteContents}");
            await sent.AddReactionAsync(confirmEmote);
            await sent.AddReactionAsync(ChonkboiL);
            await sent.AddReactionAsync(ChonkboiR);
            await sent.AddReactionAsync(declineEmote);
            discordclient = context.Client;
            //context.Client.ReactionAdded += ListenForQuoteApproval;
        }

        
    }
    
}
