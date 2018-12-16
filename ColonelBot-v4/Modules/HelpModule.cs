using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using ColonelBot_v4;
using ColonelBot_v4.Tools;

//This module custom-builds help entries to assist people in using ColonelBot's features. 
// This module uses the EmbedTool in BotTools instead of fully outlining the entries. For some reason, V3 didn't even use it.

namespace ColonelBot_v4.Modules
{
    [Group("help")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {//!help base
       [Group("available")][Alias("atb")]
       public class ATBHelp : ModuleBase<SocketCommandContext>
        {
            [Command]
            public async Task ATBHelpAsync()
            {
                await ReplyAsync("",false, EmbedTool.HelpMessage("!atb, !available", "Grants you the Available to Battle role, setting you apart from other users and letting them know you are available to Netbattle. This role is pingable and can be removed with !unav/!unavailable."));

            }
        }

        [Group("unavailable")][Alias("unav")]
        public class UnavHelp : ModuleBase<SocketCommandContext>
        {
            [Command]
            public async Task UnavHelpAsync()
            {
                await ReplyAsync("", false, EmbedTool.HelpMessage("!unav, !unavailable", "Removes the Available to Battle role from you."));
            }
        }

        [Command("hamachi")]
        public async Task HamachiHelp()
        {
            await ReplyAsync("", false, EmbedTool.HelpMessage("!hamachi", "DMs you the current Hamachi credentials.  Use of this command is logged for security."));
        }

        [Command("victors")]
        public async Task VictorsHelp()
        {
            await ReplyAsync("", false, EmbedTool.HelpMessage("!victors", "DMs you a link to a document containing the setups for N1GP Event Winners."));
        }

        [Command("onedrive")]
        public async Task OneDriveHelp()
        {
            await ReplyAsync("", false, EmbedTool.HelpMessage("!onedrive", "Provides a link to the Community OneDrive containing saves, patches, and the VBA Link Emulator."));
        }

        [Command("help")]
        public async Task HelpHelp()
        {
            await ReplyAsync("", false, EmbedTool.HelpMessage("!help <Command>", "Provides a description of a ColonelBot Command and the parameters it accepts.  Example: !help event join - Provides information on joining events and how to build the command properly."));
        }

        [Command("guides")]
        public async Task GuidesHelp()
        {
            await ReplyAsync("", false, EmbedTool.HelpMessage("!guides", "DMs you the community guides. Please ping @MidniteW for any corrections or errors."));
        }

        [Command("welcome")]
        public async Task WelcomeHelp()
        {
            await ReplyAsync("", false, EmbedTool.HelpMessage("!welcome / !welcome @User", "If done by itself, DMs the requestor all of the information needed to get started with Netbattling. If used with an @ User tag, ColonelBot will DM the tagged user with the information."));
        }

        [Command("hostflip")]
        public async Task HostFlipHelp()
        {
            await ReplyAsync("", false, EmbedTool.HelpMessage("!hostflip", "Selects whether or not you or your opponent should host for a Netbattle."));
        }

        [Command("license")]
        public async Task LicenseHelp()
        {
            await ReplyAsync("", false, EmbedTool.HelpMessage("!license", "Adds a pingable role so you can get pertinent announcements and important updates. Call the command once again to remove the role."));
        }

        [Command("deckmaster")]
        public async Task DeckmasterHelp()
        {
            await ReplyAsync("", false, EmbedTool.HelpMessage("!deckmaster", "Adds a pingable role so active players of the MegaMan TCG can matchmake. Call the command again to remove the role."));
        }
    }
}
