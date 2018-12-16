using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Discord.Commands;
using Discord;
using ColonelBot_v4.Tools;
using System.Threading.Tasks;
using System.IO;

//This class is for completing menial tasks like obtaining the bot's current directory.

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
            HamachiPassword
        }

        public static string GetSettingString(ConfigurationEntries SettingToRetreive)
        {
            //Returns the setting from the Configuration and throws if it's not found.
            string BotDirectory = new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location).Directory.ToString();
            dynamic BotConfiguration = JsonConvert.DeserializeObject(System.IO.File.ReadAllText(BotDirectory + "\\config.json"));
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
                case ConfigurationEntries.HamachiServer:
                    Result = BotConfiguration.HamachiServer;
                    break;
                case ConfigurationEntries.HamachiPassword:
                    Result = BotConfiguration.HamachiPassword;
                    break;
                default:
                    Result = null;
                    break;
            }
            return Result;
        }

    }
}
