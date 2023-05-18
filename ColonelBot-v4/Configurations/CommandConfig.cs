using ColonelBot_v4;
using Discord.Interactions;
using System;
using System.Collections.Generic;
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
    public static CommandConfig Instance { get { return instance; } }

    public CommandConfig()
    {
        //REL_TODO: Deserialize the command list from disk to ensure its always synced.
        instance = this;

    }

    public static string GetResponse(string commandName)
    {
        string response = ColbotCommands.FirstOrDefault(x => x.CommandName.ToUpperInvariant() == commandName.ToUpper()).CommandResponse;
        if (response == null)
            return $"The response for {commandName} is not yet set. Use /command configure {commandName} <response> to configure it.";
        else
            return response;
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
    public static string UpdateCommand(string commandName, string commandResponse)
    {
        BotCommand TargetCommand = ColbotCommands.FirstOrDefault(x => x.CommandName.ToUpperInvariant() == commandName.ToUpperInvariant());
        if (TargetCommand.CommandName == null)
        { // The command doesn't exist.
            TargetCommand = new BotCommand(commandName, commandResponse);
            ColbotCommands.Add(TargetCommand);
            return $"Added the response to the command {commandName}.";
        }else
        {
            //Remove the command from the list of commands so it can be replaced. We can't edit it directly.
            ColbotCommands.Remove(TargetCommand);
            BotCommand replacementCommand = new BotCommand(commandName, commandResponse);
            ColbotCommands.Add(replacementCommand);
            return $"Updated the response to the command {commandName}.";
        }
        //REL_TODO: Implement serialization to an on disk file.
    }

    public struct BotCommand
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

public class CommandConfigModule : InteractionModuleBase<SocketInteractionContext>
{
    public InteractionService commands { get; set; }
    private InteractionHandler _handler;
    public CommandConfigModule(InteractionHandler handler)
    {
        _handler = handler;
    }



    [SupporterEnabled]
    [SlashCommand("updatecommand", "(Mods) Updates the static response of the specified command.")]
    public async Task UpdateCommand(string CommandName, string CommandResponse)
    {
        
        await RespondAsync(CommandConfig.UpdateCommand(CommandName, CommandResponse));
    }
}