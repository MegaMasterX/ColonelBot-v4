using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Text;
using Discord.Commands;
using Discord;
using Discord.WebSocket;

using ColonelBot_v4.Tools;
using ColonelBot_v4.Models;

using Newtonsoft.Json;

/// <summary>
/// This Module supplements the Event Module.
/// </summary>

namespace ColonelBot_v4.Modules
{
    class WeeklyEventModule : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// DMs the caller the New Moon registrations with the following spec:
        /// 
        /// 1. "[Registrant Name] (Discord Nickname)"
        /// 2. Folder 1
        /// 3. Folder 2
        /// 4. Navicust Setups
        /// 
        /// This method compiles all active registrations into a singular text file for easy parsing.
        /// </summary>
        /// <returns></returns>
        [Command("newmoon setups")]
        public async Task GetNewMoonRegistrantsAsync()
        {
           //PSUEDO:
           // obtain a list of setup files from the setups folder.
           // substring out the Discord ID for lookup
           // 
        }


        //TODO: Split out Registration open/close and Setup acceptance open/close in EventModule
    }
}
