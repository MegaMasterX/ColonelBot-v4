using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using ColonelBot_v4.Tools;
using System.Threading.Tasks;
using System.IO;
using System.Linq;

//This class is for completing menial tasks like obtaining the bot's current directory and quickly accessing settings.

namespace ColonelBot_v4.Tools
{
    public class BotTools
    {
        public enum ConfigurationEntries
        { //Add any quickly obtainable settings to this if necessary.
            BotToken,
            LeaveJoinEnabled,
            ReportingChannel,
            HamachiServer,
            RadminServer,
            OrganizerRoleName,
            ChallongeApiKey,
            HamachiPassword,
            RadminPassword,
            ChipLibraryFileLocation,
            RadminCredentialString,               //Added per Mars' request to collapse Radmin to a single mod-editable string. -MMX 6/18/21
            AutomodFilterFileLocation,            //Added to support automod. -MMX 6/24/21
            QuoteReportChannelID,                 //Added per popular request. -MMX 6/25/21
            ModcardLibraryFileLocation,           //Added for feature update -MMX 2/17/22
        }

        /// <summary>
        /// Gets the SocketTextChannel of the configured Reporting Channel.
        /// </summary>
        /// <returns></returns>
        public static SocketTextChannel GetReportingChannel(DiscordSocketClient discord)
        {
            dynamic BotConfiguration = JsonConvert.DeserializeObject(System.IO.File.ReadAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}config.json"));
            ulong ReportChanID = BotConfiguration.ReportChannel;
            var ReportChannel = discord.GetChannel(ReportChanID) as SocketTextChannel;
            
            return ReportChannel;
        }
        /// <summary>
        /// Gets the ULong Channel ID from the configuration for the Reporting Channel. 
        /// </summary>
        /// <returns></returns>
        public static ulong GetReportingChannelUlong()
        {
            
            dynamic BotConfiguration = JsonConvert.DeserializeObject(System.IO.File.ReadAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}config.json"));
            ulong ReportChanID = BotConfiguration.ReportChannel;
            return ReportChanID;
        }

        /// <summary>
        /// Gets the Challonge API key.
        /// </summary>
        /// <returns></returns>
        public static string GetChallongeAPIKey()
        {
            dynamic BotConfiguration = JsonConvert.DeserializeObject(System.IO.File.ReadAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}config.json"));
            return BotConfiguration.ChallongeApiKey;
        }

        /// <summary>
        /// Obtains the current Banned User List.
        /// </summary>
        /// <returns></returns>
        public List<IUser> GetBannedUserList()
        {
            return null; //Not Yet Implemented.
        }

        /// <summary>
        /// Returns the Setting from the Configuration and throws an error if it's not found.
        /// </summary>
        /// <param name="SettingToRetreive"></param>
        /// <returns></returns>
        public static string GetSettingString(ConfigurationEntries SettingToRetreive)
        {
            
            dynamic BotConfiguration = JsonConvert.DeserializeObject(System.IO.File.ReadAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}config.json"));
            string Result = "";
            switch (SettingToRetreive)
            {
                case ConfigurationEntries.BotToken:
                    Result = BotConfiguration.token;
                    break;
                case ConfigurationEntries.LeaveJoinEnabled:
                    Result = BotConfiguration.LeaveJoinEnabled.ToString();
                    break;
                case ConfigurationEntries.ReportingChannel:
                    Result = BotConfiguration.ReportingChannel;
                    break;
                case ConfigurationEntries.OrganizerRoleName:
                    Result = BotConfiguration.OrganizerRoleName;
                    break;
                case ConfigurationEntries.ChallongeApiKey:
                    Result = BotConfiguration.ChallongeApiKey;
                    break;
                case ConfigurationEntries.HamachiServer:
                    Result = BotConfiguration.HamachiServer;
                    break;
                case ConfigurationEntries.RadminServer:
                    Result = BotConfiguration.RadminServer;
                    break;
                case ConfigurationEntries.RadminPassword:
                    Result = BotConfiguration.RadminPassword;
                    break;
                case ConfigurationEntries.HamachiPassword:
                    Result = BotConfiguration.HamachiPassword;
                    break;
                case ConfigurationEntries.ChipLibraryFileLocation:
                    Result = BotConfiguration.ChipLibraryFileLocation;
                    break;
                case ConfigurationEntries.RadminCredentialString:
                    Result = BotConfiguration.RadminCredentialString;               //Added per Mars' request to collapse Radmin to a single mod-editable string. -MMX 6/18/21
                    break;
                case ConfigurationEntries.AutomodFilterFileLocation:                //Added with the automod implementation - MMX 6/24/21
                    Result = BotConfiguration.AutomodFilterFileLocation;
                    break;
                case ConfigurationEntries.QuoteReportChannelID:
                    Result = BotConfiguration.QuoteReportChannelID;
                    break;
                case ConfigurationEntries.ModcardLibraryFileLocation:
                    Result = BotConfiguration.ModcardLibraryFileLocation;
                    break;
                default:
                    Result = null;
                    break;
            }
            return Result;
        }

        /// <summary>
        /// Returns the Role of a specified RoleName in the specified guild.
        /// </summary>
        /// <param name="RoleName">Plaintext Name of the Role. CASE SENSITIVE.</param>
        /// <param name="guild">SocketGuild of the Guild in which to check.</param>
        /// <returns></returns>
        public static SocketRole GetRole(string RoleName, SocketGuild guild)
        {
            var role = guild.Roles.SingleOrDefault(r => r.Name.ToUpper() == RoleName.ToUpper());
            return role;
        }

    }
}
