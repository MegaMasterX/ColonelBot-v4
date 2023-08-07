using ColonelBot_v4;
using Discord.Interactions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// The Command Config is a serializable object that holds the responses to supported commands and can be updated.
/// 
/// This should be used for commands that have STATIC responses, such as !uninstall.
/// </summary>
public class CommandConfig
{
    public static List<BotCommand> ColbotCommands = new List<BotCommand>();
    private static CommandConfig instance;
    public static CommandConfig Instance { get { if (instance == null) return new CommandConfig(); else return instance; } }

    string commandConfigPath = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Data{Path.DirectorySeparatorChar}CommandLibrary.json";

    public CommandConfig()
    {
        //REL_TODO: Deserialize the command list from disk to ensure its always synced.
        if (!File.Exists(commandConfigPath))
            File.WriteAllText(commandConfigPath, JsonConvert.SerializeObject(ColbotCommands, Formatting.Indented));
        instance = this;
        ColbotCommands = JsonConvert.DeserializeObject<List<BotCommand>>(File.ReadAllText(commandConfigPath));
        if (ColbotCommands.Count() != 0)
            Console.WriteLine($"Loaded {ColbotCommands.Count} commands.");
        else
        {
            GenerateCleanCommandConfig();
            Console.WriteLine("Generated a fresh command config.");
        }
            

    }

    /// <summary>
    /// This method will WIPE the current custom command configuration in favor of a good-known one.
    /// </summary>
    public void GenerateCleanCommandConfig()
    {
        //Some bot commands utilize the old (frankly bad) JSON configuration.
        dynamic BotConfiguration = JsonConvert.DeserializeObject(System.IO.File.ReadAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}config.json"));

        ColbotCommands.Clear();
        ColbotCommands.Add(new BotCommand("uninstall", "Uninstall removes only the following programs: SuperArmor, AirShoes, FloatShoes, any B← power from NaviCustomizer or Patch Cards."));
        ColbotCommands.Add(new BotCommand("drive", $"This folder contains all of the saves, patches, and extra info you will need to netbattle.\n<{BotConfiguration.DriveLink}>"));
        ColbotCommands.Add(new BotCommand("guides", $"The community guides can be obtained at {BotConfiguration.DriveLink}."));
        ColbotCommands.Add(new BotCommand("victors", $"The previous event winners' setups can be found here.  \n\nhttps://goo.gl/dM8UQQ"));
        ColbotCommands.Add(new BotCommand("legacyguides", "Complete guide for how to play BBN3 and other pre-BN6 games online!\n<http://legacy.n1gp.net/>"));
        ColbotCommands.Add(new BotCommand("newmoon", $"NEW MOON is a community-focused weekly roundrobin tournament series by the N1GP. Up to date information has been sent to you."));
        ColbotCommands.Add(new BotCommand("welcome", "Welcome to the N1GP! Get started by reading our FAQ!\n<http://faq.n1gp.net/>\n(This guide requires the Google Sheets app in order to be viewed on mobile phones.)"));

        File.WriteAllText(commandConfigPath, JsonConvert.SerializeObject(ColbotCommands, Formatting.Indented));
    }

    public string GetResponse(string commandName)
    {
        string response = ColbotCommands.FirstOrDefault(x => x.CommandName.ToUpperInvariant() == commandName.ToUpper()).CommandResponse;
        if (response == null)
            return $"The response for {commandName} is not yet set. Use /updateconfig to configure it.";
        else
            return response;
    }

    public bool CommandExists(string CommandName)
    {
        BotCommand result = ColbotCommands.First(x => x.CommandName.ToUpperInvariant() == CommandName.ToUpperInvariant());
        if (result == null) return false; else return true;
    }

    /// <summary>
    /// This method updates the value of <paramref name="commandName"/> to be <paramref name="commandResponse"/>.
    /// 
    /// If the command does not exist, it will be added.
    /// 
    /// Returns the response.
    /// </summary>
    /// <param name="commandName"></param>
    /// <param name="commandResponse"></param>
    public string UpdateUserCommand(string commandName, string commandResponse)
    {
        BotCommand TargetCommand = ColbotCommands.FirstOrDefault(x => x.CommandName.ToUpperInvariant() == commandName.ToUpperInvariant());
        if (TargetCommand == null)
        { // The command doesn't exist.
            TargetCommand = new BotCommand(commandName, commandResponse);
            ColbotCommands.Add(TargetCommand);
            File.WriteAllText(commandConfigPath, JsonConvert.SerializeObject(ColbotCommands,Formatting.Indented));
            return $"Added the response to the command {commandName}.";
        }else
        {
            //Remove the command from the list of commands so it can be replaced. We can't edit it directly.
            ColbotCommands.Remove(TargetCommand);
            BotCommand replacementCommand = new BotCommand(commandName, commandResponse);
            ColbotCommands.Add(replacementCommand);
            File.WriteAllText(commandConfigPath, JsonConvert.SerializeObject(ColbotCommands, Formatting.Indented));
            return $"Updated the response to the command {commandName}.";
        }

        
    }

    public class BotCommand
    {
        public string CommandName;
        public string CommandResponse;
        public BotCommand(string command, string response)
        {
            CommandName = command;
            CommandResponse = response;
        }
    }
}