using System;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Net;
using System.Linq;

using Discord;
using Discord.Commands;
using Discord.WebSocket;


using Microsoft.Extensions.DependencyInjection;

using ColonelBot_v4.Tools;
using ColonelBot_v4.Modules;

namespace ColonelBot_v4.Services
{
    public class CommandHandlingService
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;

        public CommandHandlingService(IServiceProvider services)
        {
            _commands = services.GetRequiredService<CommandService>();
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _services = services;

            _commands.CommandExecuted += CommandExecutedAsync;
            _commands.Log += LogAsync;
            _discord.MessageReceived += MessageReceivedAsync;
        }

        public async Task InitializeAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }


        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            // Ignore system messages, or messages from other bots
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;
            var context = new SocketCommandContext(_discord, message);


            //Save Game Importing
            if (message.Attachments.Count == 1) //The message contains an attachment.
            {
                if (IsEventActive())
                {//An event is active.
                    if (EventModule.IsParticipantRegistered(message.Author.Id))
                    {//Check to see if the user is registered.
                        if (rawMessage.Attachments.FirstOrDefault<Attachment>().Filename.ToUpper().Contains("SA1"))
                        {//The message contains a save file. Download and process it. 
                            string url = rawMessage.Attachments.FirstOrDefault<Attachment>().Url;
                            string CacheDirectory = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Cache{Path.DirectorySeparatorChar}";
                            var Client = new WebClient();
                            Client.DownloadFile(new Uri(url), $"{CacheDirectory}{rawMessage.Author.Id}{rawMessage.Attachments.FirstOrDefault<Attachment>().Filename}");
                            Tools.BN6.SaveTool.ProcessSave($"{CacheDirectory}{rawMessage.Author.Id}{rawMessage.Attachments.FirstOrDefault<Attachment>().Filename}", rawMessage.Author.Id, EventModule.GetParticipant(rawMessage.Author).NetbattlerName);
                            EventModule.MarkAsSubmitted(rawMessage.Author);
                            await rawMessage.DeleteAsync();
                            await context.Channel.SendMessageAsync("Save file accepted.");
                        }
                        else if (rawMessage.Attachments.FirstOrDefault<Attachment>().Filename.ToUpper().Contains("SAV") || rawMessage.Attachments.FirstOrDefault<Attachment>().Filename.ToUpper().Contains(".SG"))
                        {
                            await rawMessage.DeleteAsync();
                            await context.Channel.SendMessageAsync("", false, EmbedTool.CommandError("I can only accept Save Files that are *.SA1 format. Please upload the correct save file."));
                        }
                    }
                }
            }


            // This value holds the offset where the prefix ends
            var argPos = 0;
            if (!(message.HasMentionPrefix(_discord.CurrentUser, ref argPos) || message.HasStringPrefix("!", ref argPos))) return;

            var result = await _commands.ExecuteAsync(context, argPos, _services);
            if (result.IsSuccess == false)
            {
                //The command failed?
            }
        }


        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // command is unspecified when there was a search failure (command not found); we don't care about these errors
            if (!command.IsSpecified)
                return;

            // the command was succesful, we don't care about this result, unless we want to log that a command succeeded.

            if (result.IsSuccess)
                return;
            else
            {
		if (result.Error == CommandError.UnmetPrecondition)
		    await context.Channel.SendMessageAsync("<:NO:528279619699212300> You currently don't have the permissions to execute this command");
		else if (result.Error == CommandError.ParseFailed)
                    await context.Channel.SendMessageAsync("<:NO:528279619699212300> The command could not be parsed. Please ensure you are calling commands correctly.");
		else if (result.Error == CommandError.BadArgCount)
                    await context.Channel.SendMessageAsync($"<:NO:528279619699212300> You are not calling the command with the right amount of arguments. Try using `!help <your command>`");
                else if (result.Error == CommandError.Exception)
		    await context.Channel.SendMessageAsync($"Command Execution failed due to an uncaught exception: {result.ErrorReason}");
		else
		    await context.Channel.SendMessageAsync($"<:BarylMeh:297934727682326540> Command Execution failed with following error: {result.ErrorReason}"); //thanks trez
            }
        }

        //Helper methods
        private static bool IsEventActive()
        {
            if (File.Exists($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Data{Path.DirectorySeparatorChar}Event.json"))
                return true;
            else
                return false;
        }

    }
}
