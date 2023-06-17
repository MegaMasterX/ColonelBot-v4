using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Discord.API;

using ColonelBot_v4.Services;
using ColonelBot_v4.Tools;
using ColonelBot_v4.Modules;
using Discord.Interactions;

//ColonelBot v4 
//Developed by MegaMasterX for the N1 Grand Prix MMBN Community
// Please learn to code better, thanks.

namespace ColonelBot_v4
{
    public class Program
    {

        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _services;
        public DiscordSocketClient _discord;

        public Program()
        {
            //The configuration for botconfig.json should be mandantory in the future.
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("botconfig.json", optional: true)
                .Build();

            _services = new ServiceCollection()
               .AddSingleton(_configuration)
               .AddSingleton(_socketConfig)
               .AddSingleton<DiscordSocketClient>()
               .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
               .AddSingleton<InteractionHandler>()
               .AddSingleton<ImageService>()
               .BuildServiceProvider();
        }

        private readonly DiscordSocketConfig _socketConfig = new()
        {
            AlwaysDownloadUsers = true,
            MessageCacheSize = 200,
            GatewayIntents = GatewayIntents.GuildEmojis |
                        GatewayIntents.DirectMessages |
                        GatewayIntents.MessageContent |
                        GatewayIntents.GuildIntegrations |
                        GatewayIntents.GuildMessages |
                        GatewayIntents.GuildMembers |
                        GatewayIntents.Guilds |
                        GatewayIntents.GuildMessageReactions
        };

        static void Main(string[] args)
            => new Program().MainAsync()
                .GetAwaiter()
                .GetResult();

        public async Task MainAsync()
        {
            //Verify the directories required for the bot to run are active.
            await SetupDirectories();

          
            var client = _services.GetRequiredService<DiscordSocketClient>();

            _discord = client;
            client.Log += LogAsync;
            client.UserJoined += LogJoin;
            client.UserLeft += LogLeave;

            //Initialize the cache for various lookup modules.
            Modules.LookupModule.InitialCache();
            //Modules.ModcardLookupModule.InitializeCache();

            //Token up
            await client.LoginAsync(TokenType.Bot, BotTools.GetSettingString(BotTools.ConfigurationEntries.BotToken));
            await client.StartAsync();

            await _services.GetRequiredService<InteractionHandler>().InitializeAsync();
            await Task.Delay(-1);

        }

	    private async Task SetupDirectories()
	    {
	        await Task.Run(() =>  Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}NewMoon"));
	        await Task.Run(() =>  Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Data"));
	        await Task.Run(() =>  Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}EuRandom"));
            await Task.Run(() =>  Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Cache"));
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
        private async Task LogLeave(SocketGuild guild, SocketUser user)
        {
            await BotTools.GetReportingChannel(_discord).SendMessageAsync("", embed: EmbedTool.UserLeaveLog(user));
        }

    }
}
