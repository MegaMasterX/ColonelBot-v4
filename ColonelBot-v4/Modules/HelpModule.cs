﻿using System;
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
	//TODO Add Summary of all commands
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

        [Group("quote")]
        public class QuoteHelpBase : ModuleBase<SocketCommandContext>
        {
            [Command]
            public async Task QuoteHelpAsync()
            {
                await ReplyAsync("", false, EmbedTool.HelpMessage("!quote", "Calls a random quote from the library.\n**Additional Commands:** *!quote add <text>, !quote remove <quote number>, !quote library*"));
            }

            [Command("add")]
            public async Task QuoteAddHelpAsync()
            {
                await ReplyAsync("", false, EmbedTool.HelpMessage("!quote add <quote contents>", "Adds a quote to the library. For disclosure, this records your Discord ID as well as the date/time the quote was added"));
            }

            [Command("remove")]
            public async Task QuoteRemoveHelpAsync()
            {
                await ReplyAsync("", false, EmbedTool.HelpMessage("!quote remove <Quote ID>", "If you are the author of a quote, remove it from the library."));
            }

            [Command("admin")]
	    [RequireUserPermission(GuildPermission.ViewAuditLog)]
            public async Task QuoteAdminHelpAsync()
            {
                await ReplyAsync("", false, EmbedTool.HelpMessage("!quote admin <Edit/Remove> <Quote ID>", "Moderator-only command. Removes or edits a quote regardless of the quote author."));
            }

            [Command("library")]
            public async Task QuoteLibraryHelpAsync()
            {
                await ReplyAsync("", false, EmbedTool.HelpMessage("!quote library", "Replys with a HTML file containing the quotes library."));
            }
        }

        

        [Group("event")]
        public class EventHelpBase : ModuleBase<SocketCommandContext>
        {
            [Command]
            public async Task EventHelp()
            {
                await ReplyAsync("", false, EmbedTool.HelpMessage("!event <join/drop/update/admin/create>", "Event commands.  Only usable when there is an active event."));
            }

            [Command("create")]
            public async Task EventCreateHelp()
            {
                await ReplyAsync("", false, EmbedTool.HelpMessage("!event create <Event Name>", "Creates an event with the specified name."));
            }

            [Command("join")] //!help event join
            public async Task EventJoinHelp()
            {
                await ReplyAsync("", false, EmbedTool.HelpMessage("!event join <Netbattler Name>", "Joins the event with the netbattler name specified if the event is accepting registrations. Example: !event join MidniteW"));
            }

            [Command("drop")]
            public async Task EventDropHelp()
            {
                await ReplyAsync("", false, EmbedTool.HelpMessage("!event drop", "Removes you from the current active event. This can be done even if the event is not accepting registration."));
            }

            [Command("update")]
            public async Task EventUpdateHelp()
            {
                await ReplyAsync("", false, EmbedTool.HelpMessage("!event update <New Netbattler Name>", "If you've registered for the event, update your Netbattler Name to the one specified."));
            }

            [Command("create")]
            public async Task EventCreateHelpFull()
            {
                await ReplyAsync("", false, EmbedTool.HelpMessage("!event create <Event Name>", "Creates an event with the specified title. You must be an Event Organizer to perform this. For more information on becoming an Event Organizer, please contact MegaMasterX."));
            }

            [Command("admin")]
            public async Task EventAdminOverviewHelp()
            {
                await ReplyAsync("", false, EmbedTool.HelpMessage("!event admin <title/rules/startdate/list/openreg/closereg/remind/end", "Event Organizer tools."));
            }

            [Command("admin title")]
            public async Task EventAdminTitleHelp()
            {
                await ReplyAsync("", false, EmbedTool.HelpMessage("!event admin title <New Title>", "Updates the event title to the one you specify."));
            }

            [Command("admin rules")]
            public async Task EventAdminRulesHelp()
            {
                await ReplyAsync("", false, EmbedTool.HelpMessage("!event admin rules <Rule URL>", "Updates the Event Rules URL for participants to refer to."));
            }

            [Command("admin startdate")]
            public async Task EventAdminStartdateHelp()
            {
                await ReplyAsync("", false, EmbedTool.HelpMessage("!event admin startdate <New Date>", "Updates the Event Start Date."));
            }

            [Command ("admin openreg")]
            public async Task EventAdminOpenregHelp()
            {
                await ReplyAsync("", false, EmbedTool.HelpMessage("!event admin openreg", "Opens registration for participants. This must be open in order to accept setups.")); 
            }

            [Command("admin list")]
            public async Task EventListHelp()
            {
                await ReplyAsync("", false, EmbedTool.HelpMessage("!event admin list", "DMs the organizer a list of participants, their Discord IDs, and if they've submitted a setup or not."));
            }

            [Command("admin closereg")]
            public async Task EventAdminCloseregHelp()
            {
                await ReplyAsync("", false, EmbedTool.HelpMessage("!event admin closereg", "Closes registration and disables the ability for participants to upload their save files.  Save files uploaded (As Discord messages) when the registration is closed and an event is active will be deleted."));
            }

            [Command("admin end")]
            public async Task EventEndHelp()
            {
                await ReplyAsync("", false, EmbedTool.HelpMessage("!event admin end", "Concludes the event, deleting the event config, participant list, and all participant setups."));
            }

            [Command("admin remind")]
            public async Task EventRemindHelp()
            {
                await ReplyAsync("", false, EmbedTool.HelpMessage("!event admin remind", "DMs all registered users who have not yet provided setups to do so and how to do so."));
            }
       
            [Command("admin getsetups")]
            public async Task EventGetSetupsHelp()
            {
                await ReplyAsync("", false, EmbedTool.HelpMessage("!event admin getsetups", "Provides the Event Organizer a ZIP File containing all participant submitted setups."));
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

        [Command("drive")]
        public async Task OneDriveHelp()
        {
            await ReplyAsync("", false, EmbedTool.HelpMessage("!drive", "Provides a link to the community drive containing saves, patches, and the VBA Link Emulator."));
        }

        [Command("help")]
        public async Task HelpHelp()
        {
            await ReplyAsync("", false, EmbedTool.HelpMessage("!help <Command>", "Provides a description of a ColonelBot Command and the parameters it accepts.  Example: !help event join - Provides information on joining events and how to build the command properly."));
        }

        [Command("guides")]
        public async Task GuidesHelp()
        {
            await ReplyAsync("", false, EmbedTool.HelpMessage("!guides", "DMs you the community guides. Please mention any corrections or errors in <#554311759079407638>."));
        }

        [Command("welcome")]
        public async Task WelcomeHelp()
        {
            await ReplyAsync("", false, EmbedTool.HelpMessage("!welcome", "Provides a link to all information needed to get started with Netbattling."));
        }

        [Command("hostflip")]
        public async Task HostFlipHelp()
        {
            await ReplyAsync("", false, EmbedTool.HelpMessage("!hostflip", "Selects whether or not you or your opponent should host for a Netbattle."));
        }

        [Command("servericon")]
        public async Task ServerIconHelp()
        {
            await ReplyAsync("", false, EmbedTool.HelpMessage("!servericon","returns the URL of the server icon."));
        }
	
	[Group("newmoon")]
	public class NewMoonHelp : ModuleBase<SocketCommandContext>
	{
	    [Command]
	    public async Task NewMoonInfoHelp()
	    {
		await ReplyAsync("",false,EmbedTool.HelpMessage("!newmoon","Messages up to date information on the NEW MOON tournament series.\n Can be @ targeted."));
	    }
	    [Command("update")]
	    public async Task NewMoonInfoUpdate()
	    {
		await ReplyAsync("",false,EmbedTool.HelpMessage("!newmoon update <text>","Supporter+ command.\nSets NEW MOON information to <text>."));
	    }
	}

	[Group("eurandom")]
	public class EuRandomHelp : ModuleBase<SocketCommandContext>
	{
	    [Command]
	    public async Task EuRandomBaseHelp()
	    {
		    await ReplyAsync("",false,EmbedTool.HelpMessage("!eurandom","Provides a link to a random EuRandom savestate. \n Available subcommands: add, clear all, list"));
	    }
	    
        [Command("add")]
	    public async Task EuRandomAddHelp()
	    {
	    	await ReplyAsync("",false,EmbedTool.HelpMessage("!eurandom add <text> (Supporter+ command)","Adds new link savestates.\n Duplicate links won't be added. Every link has to be provided in an extra line"));
	    }
	    
        [Command("clear all")]
	    public async Task EuRandomClearAllHelp()
	    {
    		await ReplyAsync("",false,EmbedTool.HelpMessage("!eurandom clear all (Supporter+ command)", "Removes all saved links."));
	    }
        
        [Command("list")]
	    public async Task EuRandomListAllHelp()
	    {
		    await ReplyAsync("",false,EmbedTool.HelpMessage("!eurandom list (Supporter+ command)", " Lists all saved links."));
	    }
	}

	[Command("license")]
        public async Task LicenseHelp()
        {
            await ReplyAsync("", false, EmbedTool.HelpMessage("!license", "Adds a pingable role so you can get pertinent announcements and important updates. Call the command once again to remove the role."));
        }
	
	[Command("linkcable")][Alias("legacy battler")]
        public async Task LegacyHelp()
        {
            await ReplyAsync("", false, EmbedTool.HelpMessage("!linkcable", "Adds a pingable role so active players of BBN3 and other pre-BN6 Megaman Battle Network games can matchmake. Call the command once again to remove the role."));
        }

        [Command("deckmaster")]
        public async Task DeckmasterHelp()
        {
            await ReplyAsync("", false, EmbedTool.HelpMessage("!deckmaster", "Adds a pingable role so active players of the MegaMan TCG can matchmake. Call the command again to remove the role."));
        }
    }
}
