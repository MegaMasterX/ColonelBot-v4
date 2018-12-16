using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace ColonelBot_v4.Modules
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        [Command("hamachi")]
        public async Task GiveHamachiAsync()
        {
            //Log the caller.
            string LogEntry = DateTime.Now.ToString() + " requested by " + Context.User.Id + " - " + Context.User.Username;
            //TODO: Write a threadsafe logging method for adding the entry to the existing log.

            //Check to see if the user can accept DMs.
            try
            {
                await Context.User.SendMessageAsync("Test Hamachi");
                
            }
            catch (Exception)
            {
                await ReplyAsync("You currently have DMs disabled. Please enable DMs from users on the server to obtain the Hamachi credentials.");
                throw;
            }
        }

        [Command("faq")]
        public async Task ReplyFAQAsync()
        {
            await ReplyAsync("The answers to some of the most commonly asked community questions are located at <https://pastebin.com/vbt9i23q>!");
        }

        [Command("uninstall")]
        public async Task UninstallInformAsync()
        {
            await ReplyAsync("Uninstall removes the following programs and nothing more: SuperArmor, AirShoes, FloatShoes, Shield, Reflect, and AntiDamage.");
        }


    }
}
