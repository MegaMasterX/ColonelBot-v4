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
            [Command]
            public async Task LookupDefaultAsync([Remainder] string rndr)
            {//!lookup <chipname> - 

            }

            [Command("code")] //!lookup code <specified>
            public async Task LookupChipsByCode([Remainder] string remainder)
            {

            }
        }


        /// <summary>
        /// This method will return a built list of all chips matching the Lookup string criteria.  
        /// 
        /// Lookups, if not found, will obtain a list of chips containing the first 4 characters. It will also look up the 
        /// Aliases (Assnswrd for AssassinSword, for example.)
        /// </summary>
        /// <param name="lookupString"></param>
        /// <returns></returns>
        private string ObtainSuggestions(string lookupString)
        {
            string result = "";

            return result;
        }

        /// <summary>
        /// Returns all chips that contain the specified chip code. 
        /// </summary>
        /// <param name="CodeLookup"></param>
        /// <returns></returns>
        private string ObtainCodeResults(string CodeLookup)
        {
            string result = "";

            return result;
        }
    }
}
