using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

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
                //TODO: Perform AuthenticationCheck.
                await ReplyAsync("You have e-mail.");
            }
            catch (Exception)
            {
                await ReplyAsync("You currently have DMs disabled. Please enable DMs from users on the server to obtain the Hamachi credentials.");
                throw;
            }
        }

        [Command("hamachi update")]
        [RequireUserPermission(GuildPermission.Administrator)] //Admin-only.
        public async Task UpdateHamachiPWAsync([Remainder] string NewHamachiPW)
        {
            dynamic BotConfiguration = JsonConvert.DeserializeObject(System.IO.File.ReadAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}config.json"));
            BotConfiguration.HamachiPassword = NewHamachiPW;
            System.IO.File.WriteAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}config.json", JsonConvert.SerializeObject(BotConfiguration, Formatting.Indented));
            await ReplyAsync("The Hamachi password has been updated.");

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
            await Context.User.SendMessageAsync("", false, EmbedTool.WelcomeEmbed());
            await ReplyAsync($"You have e-mail, {Context.User.Username}");
        }

        [Command("onedrive")]
        public async Task OneDriveAsync()
        {
            await ReplyAsync("This folder contains all of the saves, patches, and extra info you will need to Netbattle.\n\n<https://1drv.ms/f/s!AlnkPup_z_U0tZUD-gZeNJ6BsSnkuA>");
        }

        [Command("victors")]
        public async Task ReplyVictors()
        {
            await ReplyAsync("The previous event winners' setups can be found here.  \n\nhttps://goo.gl/dM8UQQ ");
        }

        Random rnd = new Random(DateTime.Now.Second);

        [Command("hostflip")]
        public async Task HostflipAsync()
        {//Untargeted hostflip
            if (rnd.Next(0, 2) == 1)
                await ReplyAsync("You are hosting.");
            else
                await ReplyAsync("Your opponent is hosting.");
        }

        [Command("hostflip")]
        public async Task TargetedHostflipAsync(IUser user)
        {//Targeted hostflip.
            if (rnd.Next(0, 2) == 1)
                await ReplyAsync($"You are hosting, {user.Mention}");
            else
                await ReplyAsync($"You are hosting, {Context.User.Mention}");
        }

        [Group("welcome")]
        public class WelcomeModule : ModuleBase <SocketCommandContext>
        {
            [Command]
            public async Task SelfWelcome()
            {//Untargeted Welcome
                //TODO: Include an AuthenticateUser call.
                await Context.User.SendMessageAsync("", false, EmbedTool.WelcomeEmbed());
                await ReplyAsync("You have e-mail. Welcome to the N1 Grand Prix!");
            }

            [Command]
            public async Task TargetedWelcome(IUser user)
            {
                //TODO: Include an authenticateuser flag
                await user.SendMessageAsync("", false, EmbedTool.WelcomeEmbed());
                await ReplyAsync("E-mail sent. Welcome to the N1 Grand Prix!");
            }
        }

        /// <summary>
        /// Authenticates the user (if not already done) so they are allowed to use !hamachi and !atb.
        /// </summary>
        /// <param name="user"></param>
        private void AuthenticateUser(IGuildUser user)
        {

        }
    }
}
