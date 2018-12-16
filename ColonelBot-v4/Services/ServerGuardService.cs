using System.Linq;
using System;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;
using Microsoft.Extensions.Logging;
using System.IO;
using ColonelBot_v4.Tools;
using Newtonsoft.Json;

//The Join Logs in this Service are to only be used by this particular service.
//Normal user join/leave logs are handled in the Logging Service proper.

namespace ColonelBot_v4.Services
{
    class ServerGuardService
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly ILoggerFactory _loggerFactory;

        private IServiceProvider _provider;

        public ServerGuardService(IServiceProvider provider, DiscordSocketClient discord)
        {
            _discord = discord;
            _provider = provider;
            _discord.MessageReceived += MessageReceived;
        }

        public async Task InitializeAsync(IServiceProvider provider)
        {
            _provider = provider;
            //await _commands.AddModuleAsync(Assembly.GetEntryAssembly());
        }

        private async Task MessageReceived(SocketMessage rawMessage)
        {

        }

        private async Task LogJoin(SocketGuildUser user)
        {
            //Check to see if the timeout threshold has been reached
            //  If so, reset the threshold time to DateTime.Now and reset the Join Counter to 0
            //  Clear the cached users list.
            //Else, Increment the Join Counter. Add the user's name and ID to the cached users list.
            //  If the Join Counter matches or exceeds the threshold
            //      Modify the server's Moderation threshold and notify the Moderators channel with an @ Everyone ping.
            
        }

        private ILoggerFactory ConfigureLogging(ILoggerFactory factory)
        {
            
            return factory;
        }
    }
}
