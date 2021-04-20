using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

/// <summary>
/// This module is for etcetera administration modules, such as uploading and downloading the bot's configuration file.
/// </summary>
namespace ColonelBot_v4.Modules
{
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        [Command("getconfig"), RequireUserPermission(ChannelPermission.ManageMessages)]
        public async Task GetConfigAdminAsync()
        {
            await Context.Channel.SendFileAsync($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}config.json");
        }

        [Command("setconfig"), RequireUserPermission(ChannelPermission.ManageMessages)]
        public async Task UploadConfigAdminAsync()
        {

        }

    }
}
