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
using System.Linq;

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
            string LogEntry = $"{DateTime.Now.ToString()} requested by {Context.User.Id} - {Context.User.Username}";
            var ReportChannel = Context.Guild.GetTextChannel(BotTools.GetReportingChannelUlong());
            var Requestor = Context.User as SocketGuildUser;
            var caller = Context.User as IGuildUser;
            //Check the Netbattler Role.
            if (caller.RoleIds.Contains(RoleModule.GetRole("Netbattler", Context.Guild).Id))             {
                //Check to see if the user can accept DMs.
                try
                {
                    string HamachiServer = BotTools.GetSettingString(BotTools.ConfigurationEntries.HamachiServer);
                    string HamachiPass = BotTools.GetSettingString(BotTools.ConfigurationEntries.HamachiPassword);

                    await Context.User.SendMessageAsync($"**N1 Grand Prix Hamachi Server**\n```\nServer: {HamachiServer}\nPassword: {HamachiPass}\n```\n\nPlease ensure that your PC name on Hamachi matches your Nickname on the N1GP Discord to help make matchmaking easier.\n\n**DO NOT provide the N1 Grand Prix Hamachi server credentials to anyone outside the N1GP.**");
                    await ReportChannel.SendMessageAsync("", embed: EmbedTool.UserHamachiRequest(Requestor));
                    
                    await ReplyAsync("You have e-mail.");
                }
                catch (Exception)
                {
                    await ReplyAsync("You currently have DMs disabled. Please enable DMs from users on the server to obtain the Hamachi credentials.");
                    throw;
                }
            }
            else
                await ReplyAsync("You are not authorized to obtain Hamachi credentials. Use `!license` before attempting to use this command.");
        }

        [Command("hamachi update")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)] //Admin-only.
        public async Task UpdateHamachiPWAsync([Remainder] string NewHamachiPW)
        {
            dynamic BotConfiguration = JsonConvert.DeserializeObject(System.IO.File.ReadAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}config.json"));
            BotConfiguration.HamachiPassword = NewHamachiPW;
            System.IO.File.WriteAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}config.json", JsonConvert.SerializeObject(BotConfiguration, Formatting.Indented));
            await ReplyAsync("The Hamachi password has been updated.");

        }

        [Command("uninstall")]
        public async Task UninstallInformAsync()
        {
            await ReplyAsync("Uninstall removes the following programs and nothing more: SuperArmor, AirShoes, FloatShoes, Shield, Reflect, and AntiDamage.");
        }

        [Command("guides")]
        public async Task ReplyGuides()
        {
	    dynamic BotConfiguration = JsonConvert.DeserializeObject(System.IO.File.ReadAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}config.json"));
	    String drivelink = BotConfiguration.DriveLink;
            await Context.User.SendMessageAsync("", false, EmbedTool.GuidesEmbed(drivelink));
            if(!Context.IsPrivate)
		await ReplyAsync($"You have e-mail, {Context.User.Username}");
        }
	
	[Group("drive")]
	public class DriveModule : ModuleBase <SocketCommandContext>
	{
	    [Command]
	    public async Task OneDriveAsync()
	    {
		dynamic BotConfiguration = JsonConvert.DeserializeObject(System.IO.File.ReadAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}config.json"));
		await ReplyAsync($"This folder contains all of the saves, patches, and extra info you will need to netbattle.\n\n<{BotConfiguration.DriveLink}>");
	    }

	    [Command("update")]
	    [RequireUserPermission(GuildPermission.ViewAuditLog)] //Admin-Only.
	    public async Task UpdateOnedriveAsync([Remainder] string newOnedriveLink)
	    {
		dynamic BotConfiguration = JsonConvert.DeserializeObject(System.IO.File.ReadAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}config.json"));
		BotConfiguration.OneDriveLink = newOnedriveLink;
		System.IO.File.WriteAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}config.json", JsonConvert.SerializeObject(BotConfiguration, Formatting.Indented));
		await ReplyAsync("The drive Link has been updated.");
	    }
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

	[Command("welcome")] [Alias("faq")]
        public async Task Welcome()
        {
	    //TODO: Include an AuthenticateUser call.
	    await ReplyAsync("Welcome to the N1GP! Get started by reading our FAQ!\n<http://faq.n1gp.net/>\n(This guide requires the Google Sheets app in order to be viewed on mobile phones.)");
        }

        public async Task AddRole(IGuildUser caller, SocketRole role)
        {
            if (caller.RoleIds.Contains(role.Id) == false)
                await caller.AddRoleAsync(role, null);
        }

        public static SocketRole GetRole(string RoleName, SocketGuild guild)
        {
            var role = guild.Roles.SingleOrDefault(r => r.Name.ToUpper() == RoleName.ToUpper());
            return role;
        }
    }
}
