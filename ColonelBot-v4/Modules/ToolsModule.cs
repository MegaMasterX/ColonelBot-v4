using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

using ColonelBot_v4.Models;
using System.Text.RegularExpressions;

namespace ColonelBot_v4.Modules
{
    public class ToolsModule : ModuleBase<SocketCommandContext>
    {
        Random rnd = new Random(DateTime.Now.Second);

        /// <summary>
        /// Create a new selfcontained PauseTimer object that will handle everything.
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        [Command("timer"), Alias("p")]
        [RequireContext(ContextType.Guild)]
        public async Task CreateGuildTimer(IUser target, [Remainder] string arguments)
        {
            string[] args = arguments.Split(' ');
            if (args.Length == 2)
            {//The user has passed Time, Target
                PauseTimer tmr = new PauseTimer(ParseTime(args[1]), Context.User as IUser, target, Context);
            }else
            {//The user has just passed a time.
                PauseTimer tmr = new PauseTimer(ParseTime(args[0]), Context.User as IUser, Context);
            }
            await ReplyAsync("A timer has been created.");
        }

        /// <summary>
        /// Obtains the server icon for some reason?
        /// </summary>
        /// <returns></returns>
        [Command("servericon")]
        public async Task ServerIcon()
        {
            string url = Context.Guild.IconUrl;

            if (url is null)
                await ReplyAsync("There is no server icon");
            else
            {
                //checks if icon is animated aka last path of url starts with "a_"
                if (Regex.IsMatch(url, @"\ba_(?!\/)(?:.(?!\/))+$"))
                {
                    //replaces all possible file extensions with gif
                    url = Regex.Replace(url, @"\..{3,4}$", ".gif");
                }
                await ReplyAsync(url);
            }
        }

        [Command("hostflip")]
        public async Task HostflipAsync()
        {//Untargeted hostflip
            if (rnd.Next(0, 2) == 1)
                await ReplyAsync("You are hosting.");
            else
                await ReplyAsync("Your opponent is hosting.");
        }

        [Command("hostflip"), Alias("🪙")]
        public async Task TargetedHostflipAsync(IUser user)
        {//Targeted hostflip.
            if (rnd.Next(0, 2) == 1)
                await ReplyAsync($"You are hosting, {user.Mention}");
            else
                await ReplyAsync($"You are hosting, {Context.User.Mention}");
        }



        #region Support Methods
        /// <summary>
        /// Parses the message and returns an int in seconds based on what was specified.
        /// </summary>
        /// <param name="TimeMessage"></param>
        /// <returns></returns>
        private int ParseTime(string TimeMessage)
        {
            if (TimeMessage.ToLower().Contains('m'))
                return Convert.ToInt16(TimeMessage.Remove(TimeMessage.Length - 1, 1)) * 60;
            else if (TimeMessage.ToLower().Contains('s'))
                return Convert.ToInt16(TimeMessage.Remove(TimeMessage.Length - 1, 1));
            else
                return 0; //Hours are not supported. Should they be?
        }


        #endregion
    }
}
