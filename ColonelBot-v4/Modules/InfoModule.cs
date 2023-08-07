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
            await RespondAsync(CommandConfig.Instance.GetResponse("uninstall"));

        }

        [SlashCommand("guides", "Quickly obtain the Commnunity Guides.")]
        public async Task ReplyGuides()
        {
			await Context.User.SendMessageAsync(CommandConfig.Instance.GetResponse("guides"));
			await RespondAsync("You have email!");
        }

		[SlashCommand("legacyguides", "Obtain Legacy Guides.")]
        public async Task LegacyGuides()
        {
            await RespondAsync("", embed: EmbedTool.ChannelMessage(CommandConfig.Instance.GetResponse("legacyguides")));
        }


		[SlashCommand("drive", "Obtain the community resource links.")]
        public async Task DriveAsync()
        {
            await RespondAsync("", embed: EmbedTool.ChannelMessage(CommandConfig.Instance.GetResponse("drive")));
        }


	

        [SlashCommand("victors", "Obtain a doc outlining past event winners' setups")]
        public async Task ReplyVictors()
        {
            await RespondAsync(CommandConfig.Instance.GetResponse("victors"));
        }

		[SlashCommand("newmoon", "Obtains information on the current NEW MOON cycle.")]
		public async Task GetNewmoonInfoAsync()
		{
            await RespondAsync(CommandConfig.Instance.GetResponse("newmoon"));
        }


			//This needs to be changed to a UserCommand.

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
	   //

		[SlashCommand("welcome", "Welcome to the N1GP! Gets all basic information."), NetbattlerRequired]
        public async Task Welcome()
        {
	        await RespondAsync(CommandConfig.Instance.GetResponse("welcome"));
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
