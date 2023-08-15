using Discord;
using Discord.Interactions;
using Discord.WebSocket;

using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

using System.Text;
using System.Text.RegularExpressions;


using ColonelBot_v4.Tools;

namespace ColonelBot_v4.Modules
{
    [Group("eurandom", "Commands in association with Eurandom events.")]
    public class EuRandomModule : InteractionModuleBase<SocketInteractionContext>
    {
        string eurandomPath = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}EuRandom{Path.DirectorySeparatorChar}SetupsLinks";

        
        [SlashCommand("getsave", "Obtains a EuRandom setup from the available saves!")]
        public async Task EuRandomAsync()
        {
            if (!File.Exists(eurandomPath)) 
            {
                    await RespondAsync("No EuRandom Setups available.");
            }
            else {
                string[] lines = File.ReadAllLines(eurandomPath);
                Random r = new Random();
                int randomLineNumber = r.Next(0, lines.Length);
                string setupLink = lines[randomLineNumber];
                await RespondAsync($"[Here's your Eurandom Setup!]({setupLink})\nYour NetNavi is: {GetRandomEurandomNavi()}.\nGood luck!");
                
                
            }
        }

        [SlashCommand("add", "Add setups to Eurandom. Organizer only."), EventOrganizerEnabled]
        public async Task EuRandomUpdateAsync(string txt)
        {
            await Context.Interaction.RespondWithModalAsync<EurandomSetupAddModal>("Setups_Provided");
        }

        [ModalInteraction("Setups_Provided")]
        public async Task ModalResponse(EurandomSetupAddModal modal)
        {
            if (string.IsNullOrWhiteSpace(modal.Setups))
            {
                await RespondAsync("No setups were provided.");
            }else
            {
                string parseText = modal.Setups;
                string[] lines = parseText.Split(Environment.NewLine);
                if (File.Exists(eurandomPath))
                {
                    string[] presentLines = File.ReadAllLines(eurandomPath);
                    lines = Array.FindAll(lines, s => !presentLines.Contains(s));
                }
                //apppend to file
                File.AppendAllLines(eurandomPath, lines);
                await ReplyAsync($"Added {lines.Count()} new setups.");
            }
        }



        [SlashCommand("clearall", "Event Organizer Only. Clears all of the Eurandom Setups"), EventOrganizerEnabled]
        public async Task EuRandomClearAllAsync()
        {
            File.Delete(eurandomPath);
            await RespondAsync("Cleared all Eurandom setups!");
        }

        [SlashCommand("list", "Event Organizer Only. Lists all of the configured Eurandom setups."), EventOrganizerEnabled]
        public async Task EuRandomListAllAsync()
        {
            if (!File.Exists(eurandomPath)) 
            {
                    await RespondAsync("No EuRandom Setups available.");
            }
            else {
                string text = File.ReadAllText(eurandomPath);
                await RespondAsync(text);
            }
        }


        /// <summary>
        /// Helper method - returns a random MMBN 4.5 Navi for Eurandoms.
        /// </summary>
        /// <returns></returns>
        private string GetRandomEurandomNavi()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            return Enums.EXE4_5Navis[rnd.Next(0, Enums.EXE4_5Navis.Count - 1)];

        }
    }
}
