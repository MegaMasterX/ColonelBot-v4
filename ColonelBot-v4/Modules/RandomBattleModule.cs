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
    [Group("eurandomdx", "Commands in association with Random Battle events.")]
    public class RandomBattleModule : InteractionModuleBase<SocketInteractionContext>
    {
        string eurandomPath = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}EuRandomDX{Path.DirectorySeparatorChar}SetupsLinks";


        [SlashCommand("getsave", "Obtains a Eurandom DX setup from the available saves!")]
        public async Task EuRandomAsync()
        {
            if (!File.Exists(eurandomPath))
            {
                await RespondAsync("No EuRandom Setups available.");
            }
            else
            {
                string[] lines = File.ReadAllLines(eurandomPath);
                Random r = new Random();
                int randomLineNumber = r.Next(0, lines.Length);
                string setupLink = lines[randomLineNumber];
                await RespondAsync($"You will be battling using the following setup: {setupLink}");
            }
        }

        [SlashCommand("add", "Event Organizer Only. Adds a setup (or setups) to the pool of Eurandom DX saves."), EventOrganizerEnabled]
        public async Task EuRandomUpdateAsync(string text)
        {
            if (!text.Contains(Environment.NewLine))
            {
                //append a new line just in case 
                string outtext = text + Environment.NewLine;
                //remove empty lines and also strip added line if it was unnecessary; should catch both \r\n and \n
                string resultString = Regex.Replace(outtext, $"^\\s+$[\n]*", string.Empty, RegexOptions.Multiline);
                // check for duplication
                string[] lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                if (File.Exists(eurandomPath))
                {
                    string[] presentLines = File.ReadAllLines(eurandomPath);
                    lines = Array.FindAll(lines, s => !presentLines.Contains(s));
                }
                //apppend to file
                File.AppendAllLines(eurandomPath, lines);
                await RespondAsync("Added all new setups!");
            }
            
        }

        [SlashCommand("clearall", "Event Organizer Only. Clears all of the Eurandom DX Setups"), EventOrganizerEnabled]
        public async Task EuRandomClearAllAsync()
        {
            File.Delete(eurandomPath);
            await RespondAsync("Cleared all Eurandom DX setups!");
        }

        [SlashCommand("list", "Event Organizer Only. Lists all of the configured Eurandom DX setups."), EventOrganizerEnabled]
        public async Task EuRandomListAllAsync()
        {
            if (!File.Exists(eurandomPath))
            {
                await RespondAsync("No EuRandom Setups available.");
            }
            else
            {
                string text = File.ReadAllText(eurandomPath);
                await ReplyAsync(text);
                await RespondAsync("Responded with list.");
            }
        }
    }
}
