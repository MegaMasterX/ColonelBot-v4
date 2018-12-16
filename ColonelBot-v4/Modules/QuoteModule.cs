using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.Commands;
using System.Threading;
using System.Threading.Tasks;

//This version of the Quotes Module utilizes JSON instead of the other unmaintainable methods.

namespace ColonelBot_v4.Modules
{
    [Group("quote")] // !quote...
    public class QuoteModule : ModuleBase<SocketCommandContext>
    {
        [Command, Alias("random")]
        public async Task QuoteRandomAsync()
        {

        }


        public async Task RecacheQuotes()
    }
}
