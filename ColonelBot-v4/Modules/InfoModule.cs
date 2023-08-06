using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
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


        [Command("uninstall")]
        public async Task UninstallInformAsync()
        {
            await ReplyAsync("Uninstall removes only the following programs: SuperArmor, AirShoes, FloatShoes, any B← power from NaviCustomizer or ModCards.");

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

	[Command("legacy")]
        public async Task LegacyGuides()
        {
            await ReplyAsync("", false, EmbedTool.ChannelMessage("Complete guide for how to play BBN3 and other pre-BN6 games online!\n<http://legacy.n1gp.net/>"));
        }



	[Group("drive")]
	public class DriveModule : ModuleBase <SocketCommandContext>
	{
	    [Command]
	    public async Task OneDriveAsync()
	    {
		dynamic BotConfiguration = JsonConvert.DeserializeObject(System.IO.File.ReadAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}config.json"));
		await ReplyAsync("", false, EmbedTool.ChannelMessage($"This folder contains all of the saves, patches, and extra info you will need to netbattle.\n<{BotConfiguration.DriveLink}>"));
	    }

	    [Command("update")]
	    [RequireUserPermission(GuildPermission.ViewAuditLog)] //Admin-Only.
	    public async Task UpdateOnedriveAsync([Remainder] string newOnedriveLink)
	    {
		dynamic BotConfiguration = JsonConvert.DeserializeObject(System.IO.File.ReadAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}config.json"));
		BotConfiguration.OneDriveLink = newOnedriveLink;
		System.IO.File.WriteAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}config.json", JsonConvert.SerializeObject(BotConfiguration, Formatting.Indented));
		await ReplyAsync("", false, EmbedTool.ChannelMessage("The drive Link has been updated."));
	    }
	}

        [Command("victors")]
        public async Task ReplyVictors()
        {
            await ReplyAsync("The previous event winners' setups can be found here.  \n\nhttps://goo.gl/dM8UQQ ");
        }


	[Group("newmoon")]
	public class NewMoonInfo : ModuleBase<SocketCommandContext>
	{
	    [Command]
	    public async Task NewMoonInfoAsync()
	    {

		if (!File.Exists($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}NewMoon{Path.DirectorySeparatorChar}InfoText"))
		{
		    await ReplyAsync("No NEW MOON information was set up");
		}
		else
		{
		string infotext = System.IO.File.ReadAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}NewMoon{Path.DirectorySeparatorChar}InfoText");
		    await Context.User.SendMessageAsync(infotext);
		    await ReplyAsync("NEW MOON is a community-focused weekly roundrobin tournament series by the N1GP. Up to date information has been sent to you.");
		}
	}
	    [Command]
	    public async Task NewMoonTargetInfoAsync(IUser user)
	    {
		if (!File.Exists($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}NewMoon{Path.DirectorySeparatorChar}InfoText"))
		{
		    await ReplyAsync("No New Moon Information was set up.");
		}
		else
		{
		    string infotext = System.IO.File.ReadAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}NewMoon{Path.DirectorySeparatorChar}InfoText");
		    await user.SendMessageAsync(infotext);
		    await ReplyAsync("NEW MOON is a community-focused weekly roundrobin tournament series by the N1GP. Up to date information has been sent to you.");
		}
	    }
	    [Command("update")]
	    [RequireUserPermission(GuildPermission.ManageGuild)] //Supporter+ only.
	    public async Task UpdateNewmoonAsync([Remainder] string NewmoonInfo)
	    {
		System.IO.File.WriteAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}NewMoon{Path.DirectorySeparatorChar}InfoText",NewmoonInfo);
		await ReplyAsync("The NewMoon information has been updated.");
            }
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
