using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading;
using System.Threading.Tasks;

using ColonelBot_v4.Tools;

namespace ColonelBot_v4.Modules
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        [Command("hamachi")]
        [RequireContext(ContextType.Guild)] //Cannot be requested via DM. 
        public async Task GiveHamachiAsync()
        {
            //Log the caller.
            string LogEntry = DateTime.Now.ToString() + " requested by " + Context.User.Id + " - " + Context.User.Username;
            var ReportChannel = Context.Guild.GetTextChannel(BotTools.GetReportingChannelUlong());
            var Requestor = Context.User as SocketGuildUser;
            //Check to see if the user can accept DMs.
            try
            {
                string HamachiServer = BotTools.GetSettingString(BotTools.ConfigurationEntries.HamachiServer);
                string HamachiPass = BotTools.GetSettingString(BotTools.ConfigurationEntries.HamachiPassword);

                await Context.User.SendMessageAsync("**N1 Grand Prix Hamachi Server**\n```\nServer: " + HamachiServer + "\nPassword: " + HamachiPass + "\n```");
                await ReportChannel.SendMessageAsync("", embed: EmbedTool.UserHamachiRequest(Requestor));
            }
            catch (Exception)
            {
                await ReplyAsync("You currently have DMs disabled. Please enable DMs from users on the server to obtain the Hamachi credentials.");
                throw;
            }
        }

        [Command("faq")]
        public async Task ReplyFAQAsync()
        {
            await ReplyAsync("The answers to some of the most commonly asked community questions are located at <https://pastebin.com/vbt9i23q>!");
        }

        [Command("uninstall")]
        public async Task UninstallInformAsync()
        {
            await ReplyAsync("Uninstall removes the following programs and nothing more: SuperArmor, AirShoes, FloatShoes, Shield, Reflect, and AntiDamage.");
        }

        [Command("guides")]
        public async Task ReplyGuides()
        {
            //await Context.User.SendMessageAsync()
        }
    }
}
