using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;


namespace ColonelBot_v4.Modules
{
    public class LookupModule : ModuleBase<SocketCommandContext>
    {
        [Group("lookup")]
        public class LookupSpecificGame
        {
            [Command("1")]
            public async Task LookupBN1Async([Remainder] string rndr)
            {//!lookup 1 <chip name>

            }

            [Command("2")]
            public async Task LookupBN2Async([Remainder] string rndr)
            {//!lookup 2 <chip name>

            }

            [Command("3")]
            public async Task LookupBN3Async([Remainder] string rndr)
            {//!lookup 3 <chip name>

            }

            [Command("4")]
            public async Task LookupBN4Async([Remainder] string rndr)
            {//!lookup 4 <chip name>

            }

            [Command("5")]
            public async Task LookupBN5Async([Remainder] string rndr)
            {//!lookup 5 <chip name>

            }

            [Command("6")]
            public async Task LookupBN6Async([Remainder] string rndr)
            {//!lookup 6 <chip name>

            }

            [Command]
            public async Task LookupDefaultAsync([Remainder] string rndr)
            {//!lookup <chipname> - Defaults to MMBN6.

            }
        }
    }
}
