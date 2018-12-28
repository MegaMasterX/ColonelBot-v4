using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Discord;
using Discord.WebSocket;
using Discord.Commands;

using ColonelBot_v4.Services;
using ColonelBot_v4.Tools;

//ColonelBot v4 
//Developed by MegaMasterX for the N1 Grand Prix MMBN Community

namespace ColonelBot_v4
{
    class Program
    {
        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        DiscordSocketClient _discord;

        public async Task MainAsync()
        {
            using (var services = ConfigureServices())
            {
                var client = services.GetRequiredService<DiscordSocketClient>();
                _discord = client;
                client.Log += LogAsync;
                services.GetRequiredService<CommandService>().Log += LogAsync;
                client.UserJoined += LogJoin;
                client.UserLeft += LogLeave;
                //Token up
                await client.LoginAsync(TokenType.Bot, BotTools.GetSettingString(BotTools.ConfigurationEntries.BotToken));
                await client.StartAsync();
                               
                await services.GetRequiredService<CommandHandlingService>().InitializeAsync();
                
                await Task.Delay(-1);
            }

                
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                // Base
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<HttpClient>()
                .AddSingleton<ImageService>()
                .BuildServiceProvider();
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());

            return Task.CompletedTask;
        }

        private async Task LogJoin(SocketGuildUser user)
        {
            //v4: Moved redundant code to BotTools.   
            await BotTools.GetReportingChannel(_discord).SendMessageAsync("", embed: EmbedTool.UserJoinLog(user));

        }

        /// <summary>
        /// Creates and sends an embed for a user Leaving the Discord server.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task LogLeave(SocketGuildUser user)
        {
            await BotTools.GetReportingChannel(_discord).SendMessageAsync("", embed: EmbedTool.UserLeaveLog(user));
        }

    }
}
