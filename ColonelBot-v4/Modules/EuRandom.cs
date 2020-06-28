using Discord;
using Discord.Commands;
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
    [Group("eurandom")]
    public class EuRandomModule : ModuleBase<SocketCommandContext>
    {
        string eurandomPath = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}EuRandom{Path.DirectorySeparatorChar}SetupsLinks";

        [Command]
        public async Task EuRandomAsync()
        {
            if (!File.Exists(eurandomPath)) 
            {
                    await ReplyAsync("No EuRandom Setups available.");
            }
            else {
                string[] lines = File.ReadAllLines(eurandomPath);
                Random r = new Random();
                int randomLineNumber = r.Next(0, lines.Length);
                string setupLink = lines[randomLineNumber];
                await ReplyAsync($"You will be battling using the following setup: {setupLink}");
            }
        }
        [Command("add")]
        [RequireUserPermission(GuildPermission.ManageGuild)] //Supporter+ only.
        public async Task EuRandomUpdateAsync([Remainder] string text)
        {
            //append a new line just in case 
            string outtext = text + Environment.NewLine;
            //remove empty lines and also strip added line if it was unnecessary; should catch both \r\n and \n
            string resultString = Regex.Replace(outtext, $"^\\s+$[\n]*", string.Empty, RegexOptions.Multiline);
            // check for duplication
            string[] lines = text.Split(new [] {"\r\n","\r","\n"}, StringSplitOptions.None);
            if (File.Exists(eurandomPath))
            {
                string[] presentLines = File.ReadAllLines(eurandomPath);
                lines = Array.FindAll(lines ,s => !presentLines.Contains(s));
            }
            //apppend to file
            File.AppendAllLines(eurandomPath, lines);
            await ReplyAsync("Added all new setups!");
        }

        [Command("clear all")]
        [RequireUserPermission(GuildPermission.ManageGuild)] //Supporter+ only.
        public async Task EuRandomClearAllAsync()
        {
            File.Delete(eurandomPath);
            await ReplyAsync("cleared setups!");
        }

        [Command("list")]
        [RequireUserPermission(GuildPermission.ManageGuild)] //Supporter+ only.
        public async Task EuRandomListAllAsync()
        {
            if (!File.Exists(eurandomPath)) 
            {
                    await ReplyAsync("No EuRandom Setups available.");
            }
            else {
                string text = File.ReadAllText(eurandomPath);
                await ReplyAsync(text);
            }
        }
    }
}
