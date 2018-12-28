using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using System.IO;
using ColonelBot_v4.Tools;

//TODO: Rewrite this.

namespace ColonelBot_v4.Services
{
    class LoggingService
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _discordLogger;
        private readonly ILogger _commandsLogger;

        public LoggingService(DiscordSocketClient discord, CommandService commands, ILoggerFactory loggerFactory)
        {
            _discord = discord;
            _commands = commands;

            _loggerFactory = ConfigureLogging(loggerFactory);
            
            _discordLogger = _loggerFactory.CreateLogger("discord");
            _commandsLogger = _loggerFactory.CreateLogger("commands");

            _discord.Log += LogDiscord;
            _commands.Log += LogCommand;
            

            string BotDirectory = new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location).Directory.ToString();
            dynamic BotConfiguration = JsonConvert.DeserializeObject(System.IO.File.ReadAllText(BotDirectory + "\\config.json"));
            bool ReportingEnabled = BotConfiguration.LeaveJoinEnabled;

            if (ReportingEnabled)
            {
                _discord.UserJoined += LogJoin;
                _discord.UserLeft += LogLeave;
                //_discord.ReactionAdded += LogReact;
            }
        }

        private ILoggerFactory ConfigureLogging(ILoggerFactory factory)
        {
            
            //factory.AddConsole();

            return factory;
        }

        //private async Task LogReact(Cacheable<IUserMessage, ulong> user, SocketReaction reaction)
        //{ //TODO: Implement Reaction Logging in Sprint 2.
        //    return Task.CompletedTask;
        //}


        /// <summary>
        /// Creates and sends an embed for a user Joining the Discord server.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task LogJoin(SocketGuildUser user)
        {
            //v4: Moved redundant code to BotTools.   
            await BotTools.GetReportingChannel(_discord).SendMessageAsync("", embed:EmbedTool.UserJoinLog(user));

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

        private Task LogDiscord(LogMessage message)
        {
            _discordLogger.Log(
                LogLevelFromSeverity(message.Severity),
                0,
                message,
                message.Exception,
                (_1, _2) => message.ToString(prependTimestamp: false));
            return Task.CompletedTask;
        }

        private Task LogCommand(LogMessage message)
        {
            // Return an error message for async commands
            if (message.Exception is CommandException command)
            {
                // Don't risk blocking the logging task by awaiting a message send; ratelimits!?
                var _ = command.Context.Channel.SendMessageAsync($"Error: {command.Message}");
            }

            _commandsLogger.Log(
                LogLevelFromSeverity(message.Severity),
                0,
                message,
                message.Exception,
                (_1, _2) => message.ToString(prependTimestamp: false));
            return Task.CompletedTask;
        }

        private static LogLevel LogLevelFromSeverity(LogSeverity severity)
            => (LogLevel)(Math.Abs((int)severity - 5));

    }
}
