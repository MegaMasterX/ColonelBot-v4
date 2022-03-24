using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using ColonelBot_v4.Models;

//This class is for quickly constructing an embed with a default setup for quick responses.
//TODO: Update this to mirror ChannelMessages' build.
namespace ColonelBot_v4.Tools
{
    public class EmbedTool
    {
        /// <summary>
        /// Builds a stock Channel Message embed for responding to user requests.
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static Embed ChannelMessage(string Message)
        {
            var embed = new EmbedBuilder
            {
                Color = new Color(0xffcf39)
            };

            embed.AddField("Response", Message);
            
            return embed.Build();
        }
        
        /// <summary>
        /// Builds a stock Help Message embed.
        /// </summary>
        /// <param name="CommandTitle"></param>
        /// <param name="Contents"></param>
        /// <returns></returns>
        public static Embed HelpMessage(string CommandTitle, string Contents)
        {
            EmbedBuilder embed = new EmbedBuilder
            {
                Color = new Color(0xffcf39)
            };
            embed.AddField(CommandTitle, Contents);
            return embed.Build();
        }

        /// <summary>
        /// Returns a pre-built event for !event info, assuming the event has been verified as active.
        /// </summary>
        /// <param name="targetEvent"></param>
        /// <returns></returns>
        public static Embed EventInfoEmbed(Event targetEvent)
        {
            EmbedBuilder embed = new EmbedBuilder
            {
                Color = new Color(0xffcf39),
                Title = targetEvent.EventName
            };
            embed.AddField("Description", targetEvent.Description);
            embed.AddField("Rules & Format", targetEvent.RulesURL);
            embed.AddField("Registration Open", targetEvent.RegistrationOpen.ToString(), true);
            embed.AddField("Event Start Date", targetEvent.StartDate, true);
            return embed.Build();
        }

        /// <summary>
        /// Builds an embed detailing a user that's joined the Discord.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static Embed UserJoinLog(SocketGuildUser user)
        {
            EmbedBuilder embed = new EmbedBuilder
            {
                Title = "User Joined",
                Color = new Color(0x00ff00)
            };
            embed.AddField("Account Name", user.Username);
            embed.AddField("User ID", user.Id.ToString());
            embed.ThumbnailUrl = user.GetAvatarUrl();
            if (user.Nickname != null)
                embed.AddField("Nickname", user.Nickname);
            return embed.Build();
        }

        /// <summary>
        /// Builds an embed meant to assist with logging new quote adds.
        /// </summary>
        /// <param name="user">The SocketGuildUser of the quote author.</param>
        /// <param name="QuoteText">String containing the quotes contents.</param>
        /// <param name="QuoteID">String containing the Quote ID in the Library.</param>
        /// <returns></returns>
        public static Embed QuoteAddLog(SocketGuildUser user, string QuoteText, string QuoteID)
        {
            EmbedBuilder embed = new EmbedBuilder
            {
                Title = "Quote Added",
                Color = new Color(0xffc0cb)
            };
            embed.AddField("Quote Author", user.Username);
            embed.AddField("User ID", user.Id.ToString());
            embed.AddField("Quote ID", QuoteID);
            embed.AddField("Quote Contents", QuoteText);
            return embed.Build();

        }

        /// <summary>
        /// Builds an embed detailing an error.
        /// </summary>
        /// <param name="ErrorMessage">Text to be displayed.</param>
        /// <returns></returns>
        public static Embed CommandError(string ErrorMessage)
        {
            EmbedBuilder embed = new EmbedBuilder
            {
                ThumbnailUrl = "https://cdn.discordapp.com/emojis/447132734763302931.png?v=1",
                Color = new Color(0xff0000)
            };
            embed.AddField("Sorry, but...", ErrorMessage);
            return embed.Build();
        }

        /// <summary>
        /// Builds an embed detailing a Discord user that's requested Hamachi credentials. 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static Embed UserHamachiRequest(SocketGuildUser user)
        {
            EmbedBuilder embed = new EmbedBuilder
            {
                Title = "Hamachi Request",
                Color = new Color(0x0097FF)
            };
            embed.AddField("Account Name", user.Username);
            embed.AddField("User ID", user.Id.ToString());
            embed.ThumbnailUrl = user.GetAvatarUrl();
            if (user.Nickname != null)
                embed.AddField("Nickname", user.Nickname);
            embed.AddField("Date/Time Requested", DateTime.Now);
            return embed.Build();
        }


        /// <summary>
        /// Builds an embed detailing a Discord user that's requested Radmin credentials. 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static Embed UserRadminRequest(SocketGuildUser user)
        {
            EmbedBuilder embed = new EmbedBuilder
            {
                Title = "Radmin Request",
                Color = new Color(0x65EEEE)
            };
            embed.AddField("Account Name", user.Username);
            embed.AddField("User ID", user.Id.ToString());
            embed.ThumbnailUrl = user.GetAvatarUrl();
            if (user.Nickname != null)
                embed.AddField("Nickname", user.Nickname);
            embed.AddField("Date/Time Requested", DateTime.Now);
            return embed.Build();
        }

        /// <summary>
        /// Builds an embed detailing a user that's left the Discord.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Embed</returns>
        public static Embed UserLeaveLog(SocketGuildUser user)
        {
            EmbedBuilder embed = new EmbedBuilder
            {
                Title = "User Left", 
                Color = new Color(0xff0000)
            };
            embed.AddField("Account Name", user.Username);
            embed.AddField("User ID", user.Id.ToString());
            embed.ThumbnailUrl = user.GetAvatarUrl();
            if (user.Nickname != null)
                embed.AddField("Nickname", user.Nickname);

            return embed.Build();
        }

        public static Embed GuidesEmbed(String drivelink)
        {
	    EmbedBuilder embed = new EmbedBuilder
            {
                Color = new Color(0xffcf39) //ColonelBot's Default Yellow
            };

	    embed.AddField("Notice","The N1 Grand Prix cannot provide ROMs. We strongly suggest ripping your own ROM from a physical cart provided that it complies with your country's copyright laws. The N1GP Discord Community and its administrators are not responsible for any action taken by you as a result of participation.");
            embed.AddField("NetBattler Resource Folder", $"This resource folder contains all the saves, patches, and various software you will need to get started with netbattling. \n{drivelink}");
            embed.AddField("How To Netbattle Online [Video Guides]", "These will show you how to import your save file, netbattle as a host, and netbattle as a Client.  \nhttp://bit.ly/2htNN8W");
            embed.AddField("Netbattle 101", "Overview of version differences and game mechanics for new and returning Netbattlers that need a refresher. Includes text guide of how to setup online play. \nhttps://docs.google.com/document/d/1AjvxhqfcMCg6H6o8683emQ7pmUq2ffGo0ogBPd84e00/");
	    embed.AddField("Netbattle EX/Advanced Guides", "In-depth collection of guides with chip functionality, frame data, detailed game mechanic explanations as well as Japanese version exclusive e-reader+ information. \nhttps://docs.google.com/document/d/1Io4EU7S5lYtZS7bmgnfLBOl97umbMRodZEbXgFxTzdI");
            embed.AddField("N1GP Central Document", "Access everything above and more through the central doc, a complete collection of all guides and resources provided by our community. \nhttp://central.n1gp.net/");
	    return embed.Build();
        }

        /// <summary>
        /// Builds a Embed for Battlechips.
        /// </summary>
        /// <param name="chipName">Name of the Battlechip</param>
        /// <param name="chipElement">Element of the Battlechip</param>
        /// <param name="chipMB">Megabyte value of the Battlechip</param>
        /// <param name="chipATK">Attack value of the Battlechip</param>
        /// <param name="chipCodes">Available Chip Codes for the Battlechip</param>
        /// <param name="chipDescription">In-Game Description for the Battlechip</param>
        /// <param name="chipURL">URL of the Image for the thumbnail for the Battlechip.</param>
        /// <param name="MoreInfoURL">Link to the Netbattle 101 Location, if applicable. If "", this will be omitted from the embed.</param>
        /// <returns></returns>
        public static Embed ChipEmbed(Chip sourceChip)
        {
            EmbedBuilder embed = new EmbedBuilder
            {
                Color = new Color(0xffcf39), //ColonelBot's Default Yellow
                ThumbnailUrl = sourceChip.Image //The ChipURL should always be in the library.
            };
            embed.AddField("Chip Name", sourceChip.Name, true);
            embed.AddField("Element", sourceChip.Element, true);
            embed.AddField("MB", sourceChip.MB, true);
            embed.AddField("Attack", sourceChip.ATK, true);
            embed.AddField("Codes", sourceChip.Codes, true);
            embed.AddField("Description", sourceChip.Description, true);
            if (sourceChip.MoreDetails != "") //Add the More Information URL if there is anything specified.
                embed.AddField("More Information", sourceChip.MoreDetails, false);

            return embed.Build();
        }

        public static Embed ModcardEmbed(ModCard modcard)
        {
            EmbedBuilder embed = new EmbedBuilder
            {
                Color = new Color(0x773A8F)
            };
            embed.AddField("Card Name", modcard.Name, false);
            embed.AddField("MB", modcard.MB, false);
            embed.AddField("Positive Abilities", modcard.PositiveAbilities, true);
            embed.AddField("Negative Abilities", modcard.NegativeAbilities, true);
            if (modcard.MoreInformation != "")
                embed.AddField("More Information", modcard.MoreInformation, false);

            return embed.Build();
        }

        /// <summary>
        /// Returns a embed catered to search results from a failed or errored lookup.
        /// </summary>
        /// <param name="Results">Search results string returned by the lookup method.</param>
        /// <returns></returns>
        public static Embed ChipSearchResultsEmbed(string Results)
        {
            EmbedBuilder embed = new EmbedBuilder
            {
                Color = new Color(0xffcf39)
            };

            embed.AddField("Lookup Recommendations", Results, false);

            return embed.Build();
        }
    }
}
