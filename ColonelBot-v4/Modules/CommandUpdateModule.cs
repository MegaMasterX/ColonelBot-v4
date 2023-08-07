using System;
using System.Threading;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Text;
using Discord.Interactions;
using Discord;
using Discord.WebSocket;

using ColonelBot_v4.Tools;
using ColonelBot_v4.Models;

using Newtonsoft.Json;

using System.Net;

[SupporterEnabled]
public class CommandUpdateModule : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("updatecommand", "Updates a Bot Command"), ModeratorOnly]
    public async Task UpdateCommandModalCallAsync()
    {
        await Context.Interaction.RespondWithModalAsync<DiscordCommandUpdateModal>("CommandUpdateData");
    }

    [ModalInteraction("CommandUpdateData")]
    public async Task CommandUpdateModalResponse(DiscordCommandUpdateModal modal)
    {
        //The min length is set to 1 so they're required by default, no need to null check.
        //PSUEDO: Check to see if modal.CommandName is a valid command in JSON.

        CommandConfig.Instance.UpdateUserCommand(modal.CommandName, modal.NewCommandValue);

        await RespondAsync($"Updating `{modal.CommandName}` to now display:\n\n{modal.NewCommandValue}");
    }

    [SlashCommand("resetcommands", "Resets all information commands to their defaults"), ModeratorOnly]
    public async Task ResetCommandsModalAsync()
    {
        await Context.Interaction.RespondWithModalAsync<ConfirmResetCommandsModal>("ResetCommands");
    }

    [ModalInteraction("ResetCommands")]
    public async Task ResetCommandModalResponse(ConfirmResetCommandsModal modal)
    {
        if (modal.confirmation.ToUpperInvariant() != "YES")
        {
            await RespondAsync("I didn't get the correct response. The custom command responses remain the same.");
        }
        else
        {
            CommandConfig.Instance.GenerateCleanCommandConfig();
            await RespondAsync("All informational slash commands are now reset.");
        }
    }
}

