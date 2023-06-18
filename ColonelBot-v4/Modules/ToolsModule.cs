using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Interactions;

using ColonelBot_v4.Models;
using System.Text.RegularExpressions;

namespace ColonelBot_v4.Modules
{
    public class ToolsModule : InteractionModuleBase<SocketInteractionContext>
    {
        Random rnd = new Random(DateTime.Now.Second);

        /// <summary>
        /// Create a new selfcontained PauseTimer object that will handle everything.
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        [SlashCommand("timer", "Creates a timer that pings you when it expires. Use <num><m/s> for mins/seconds. Ex. 5m, 30s")]
        [RequireContext(ContextType.Guild)]
        public async Task CreateGuildTimer(string Time, IUser target = null)
        {
            if (Time.Contains("everyone") || Time.Contains("here"))
            {
                await RespondAsync("<:NO:528279619699212300>");
            }else
            {
                string[] args = Time.Split(' ');
                
                try
                {
                    if (target == null)
                    {//The user has passed Time, Target

                        PauseTimer tmr = new PauseTimer(ParseTime(args[0]), Context.User as IUser, target, Context as SocketInteractionContext);
                        await RespondAsync("A timer has been created.");
                    }
                    else
                    {//The user has just passed a time.
                        PauseTimer tmr = new PauseTimer(ParseTime(args[1]), Context.User as IUser, Context as SocketInteractionContext);
                        await RespondAsync("A timer has been created.");
                    }
                }
                catch (Exception)
                { //This is likely due to the caller just doing !timer <elapsed>.
                    await RespondAsync("<:NO:528279619699212300> Please use #s or #m (Example: 5s for 5 Seconds or 2m for 2 Minutes) as a time for your timer. Don't forget to tag either yourself or your opponent!\n\nExample: !timer @<user tag> 5s");
                }

                
            }
            
        }

        /// <summary>
        /// Obtains the server icon for some reason?
        /// </summary>
        /// <returns></returns>
        [SlashCommand("servericon", "Utility Command. Pulls the server icon. Mod only.")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task ServerIcon()
        {
            string url = Context.Guild.IconUrl;

            if (url is null)
                await RespondAsync("There is no server icon");
            else
            {
                //checks if icon is animated aka last path of url starts with "a_"
                if (Regex.IsMatch(url, @"\ba_(?!\/)(?:.(?!\/))+$"))
                {
                    //replaces all possible file extensions with gif
                    url = Regex.Replace(url, @"\..{3,4}$", ".gif");
                }
                await RespondAsync(url);
            }
        }

        [SlashCommand("hostflip", "Effectively flips a coin to determine a host.")]
        public async Task HostflipAsync()
        {//Untargeted hostflip
            if (rnd.Next(0, 2) == 1)
                await RespondAsync("You are hosting.");
            else
                await RespondAsync("Your opponent is hosting.");
        }


        #region Support Methods
        /// <summary>
        /// Parses the message and returns an int in seconds based on what was specified.
        /// </summary>
        /// <param name="TimeMessage"></param>
        /// <returns></returns>
        private int ParseTime(string TimeMessage)
        {
            int MinuteCount = Convert.ToInt16(TimeMessage.Remove(TimeMessage.Length - 1, 1));
            if (MinuteCount > 5)
                MinuteCount = 5; //Cap the minutes at 5 per request
            if (TimeMessage.ToLower().Contains('m'))
                return Convert.ToInt16(MinuteCount) * 60;
            else if (TimeMessage.ToLower().Contains('s'))
                return Convert.ToInt16(TimeMessage.Remove(TimeMessage.Length - 1, 1));
            else
                return 0; //Hours are not supported. Should they be?
        }


        #endregion
    }
}
