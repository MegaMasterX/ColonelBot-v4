using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Linq;



using ColonelBot_v4.Tools;
using Discord.Interactions;

namespace ColonelBot_v4.Modules
{
    public class InfoModule : InteractionModuleBase<SocketInteractionContext>
    {


        [SlashCommand("uninstall", "What does Uninstall remove in BN6?")]
        public async Task UninstallInformAsync()
        {
            await RespondAsync("Uninstall removes only the following programs: SuperArmor, AirShoes, FloatShoes, any B← power from NaviCustomizer or ModCards.");

        }

        [SlashCommand("guides", "Quickly obtain the Commnunity Guides.")]
        public async Task ReplyGuides()
        {
			dynamic BotConfiguration = JsonConvert.DeserializeObject(System.IO.File.ReadAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}config.json"));
			String drivelink = BotConfiguration.DriveLink;
				await Context.User.SendMessageAsync("", false, EmbedTool.GuidesEmbed(drivelink));
			await RespondAsync("You have email!");
        }

		[SlashCommand("legacyguides", "Obtain Legacy Guides.")]
        public async Task LegacyGuides()
        {
            await RespondAsync("", embed: EmbedTool.ChannelMessage("Complete guide for how to play BBN3 and other pre-BN6 games online!\n<http://legacy.n1gp.net/>"));
        }



	[Group("drive", "Resources for getting started with the N1GP!")]
	public class DriveModule : InteractionModuleBase<SocketInteractionContext>
	{
	    [SlashCommand("link", "Obtains the community resource links.")]
	    public async Task OneDriveAsync()
	    {
		dynamic BotConfiguration = JsonConvert.DeserializeObject(System.IO.File.ReadAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}config.json"));
		await RespondAsync("", embed: EmbedTool.ChannelMessage($"This folder contains all of the saves, patches, and extra info you will need to netbattle.\n<{BotConfiguration.DriveLink}>"));
	    }

	    [SlashCommand("update", "Updates the Drive command's URL."), ModeratorOnly]
	    public async Task UpdateOnedriveAsync(string newOnedriveLink)
	    {
		dynamic BotConfiguration = JsonConvert.DeserializeObject(System.IO.File.ReadAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}config.json"));
		BotConfiguration.OneDriveLink = newOnedriveLink;
		System.IO.File.WriteAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}config.json", JsonConvert.SerializeObject(BotConfiguration, Formatting.Indented));
		await RespondAsync("", embed: EmbedTool.ChannelMessage("The drive Link has been updated."));
	    }
	}

        [SlashCommand("victors", "Obtain a doc outlining past event winners' setups")]
        public async Task ReplyVictors()
        {
            await RespondAsync("The previous event winners' setups can be found here.  \n\nhttps://goo.gl/dM8UQQ ");
        }


	[Group("newmoon", "Obtain MOON cycle event information.")]
	public class NewMoonInfo : InteractionModuleBase<SocketInteractionContext>
	{
	    [SlashCommand("info", "Obtains information on the current NEW MOON cycle.")]
	    public async Task NewMoonInfoAsync()
	    {

		if (!File.Exists($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}NewMoon{Path.DirectorySeparatorChar}InfoText"))
		{
		    await RespondAsync("No NEW MOON information was set up");
		}
		else
		{
		string infotext = System.IO.File.ReadAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}NewMoon{Path.DirectorySeparatorChar}InfoText");
		    await Context.User.SendMessageAsync(infotext);
		    await RespondAsync("NEW MOON is a community-focused weekly roundrobin tournament series by the N1GP. Up to date information has been sent to you.");
		}
	}
	 //   [Command]
	 //   public async Task NewMoonTargetInfoAsync(IUser user)
	 //   {
		//if (!File.Exists($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}NewMoon{Path.DirectorySeparatorChar}InfoText"))
		//{
		//    await RespondAsync("No New Moon Information was set up.");
		//}
		//else
		//{
		//    string infotext = System.IO.File.ReadAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}NewMoon{Path.DirectorySeparatorChar}InfoText");
		//    await user.SendMessageAsync(infotext);
		//    await ReplyAsync("NEW MOON is a community-focused weekly roundrobin tournament series by the N1GP. Up to date information has been sent to you.");
		//}
	   // }

	    [SlashCommand("update", "Updates NEWMOON cycle info command."), ModeratorOnly]
	    [RequireUserPermission(GuildPermission.ManageGuild)] //Supporter+ only.
	    public async Task UpdateNewmoonAsync(string NewmoonInfo)
	    {
		System.IO.File.WriteAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}NewMoon{Path.DirectorySeparatorChar}InfoText",NewmoonInfo);
		await RespondAsync("The NewMoon information has been updated.");
            }
	}

		[SlashCommand("welcome", "Welcome to the N1GP! Gets all basic information.")]
        public async Task Welcome()
        {
	    //TODO: Include an AuthenticateUser call.
	    await RespondAsync("Welcome to the N1GP! Get started by reading our FAQ!\n<http://faq.n1gp.net/>\n(This guide requires the Google Sheets app in order to be viewed on mobile phones.)");
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
