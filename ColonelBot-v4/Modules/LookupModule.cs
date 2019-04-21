using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using ColonelBot_v4.Models;
using ColonelBot_v4.Tools;
using Newtonsoft.Json;

namespace ColonelBot_v4.Modules
{
    public class LookupModule : ModuleBase<SocketCommandContext>
    {

        static List<Chip> ChipLibrary;

        [Group("lookup")]
        public class LookupSpecificGame : ModuleBase<SocketCommandContext>
        {
            [Command]
            public async Task LookupDefaultAsync([Remainder] string rndr)
            {//!lookup <chipname> - 
                if (SearchChip(rndr) == false)
                {
                    //Perform an Alias Search
                    if (SearchAliases(rndr) == false)
                    {
                        //Perform a fuzzy search and provide recommendations based on the user's input.

                    }
                    else
                    {
                        await ReplyAsync("", embed: EmbedTool.ChipEmbed(GetChipByAlias(rndr)));
                    }
                }
                else
                {
                    await ReplyAsync("", embed: EmbedTool.ChipEmbed(GetChipByName(rndr)));
                }

            }

            [Command("code"), Priority(1)] //!lookup code <specified>
            public async Task LookupChipsByCode([Remainder] string remainder)
            {
                await ReplyAsync("Code Lookup Implementation Test");
            }
        }

        /// <summary>
        /// Returns the Chip type of the specified chip, assuming it's been verified and found with SearchChip.
        /// </summary>
        /// <param name="ChipName"></param>
        /// <returns>Returns a Chip model instance that matches the chip that was located in the Library.</returns>
        public static Chip GetChipByName(string ChipName)
        {
            Chip selectedChip = null;
            selectedChip = ChipLibrary.Find(x => x.Name.ToUpper() == ChipName.ToUpper());
            if (selectedChip == null)
                return null;
            else
                return selectedChip;
        }

        /// <summary>
        /// Returns the Chip type of the specified chip's alias, assuming its been verified and found with SearchAlias.
        /// </summary>
        /// <param name="AliasName"></param>
        /// <returns>Returns a Chip model of the instance that matches the chip found in the Library.</returns>
        public static Chip GetChipByAlias(string AliasName)
        {
            Chip selectedChip = null;
            selectedChip = ChipLibrary.Find(x => x.Alias.ToUpper().Contains(AliasName.ToUpper()));
            if (selectedChip == null)
                return null;
            else
                return selectedChip;
        }

        /// <summary>
        /// This method searches the chip library to see if the Alias matches the user's specification.
        /// </summary>
        /// <param name="ChipName"></param>
        /// <returns>Returns true if an Alias was found, false if not.</returns>
        public static bool SearchAliases(string ChipName)
        {
            Chip selectedChip = null;
            selectedChip = ChipLibrary.Find(x => x.Alias.ToUpper().Contains(ChipName.ToUpper()));
            if (selectedChip == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// This method searches for an entire chip name to see if it 1-to-1 matches. 
        /// </summary>
        /// <param name="ChipName"></param>
        /// <returns>Returns True if the chip is identified, False if the chip is not identified.</returns>
        public static bool SearchChip(string ChipName)
        {
            Chip selectedChip = null;
            selectedChip = ChipLibrary.Find(x => x.Name.ToUpper() == ChipName.ToUpper());
            if (selectedChip == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// This method will return a built list of all chips matching the Lookup string criteria.  
        /// 
        /// Lookups, if not found, will obtain a list of chips containing the first 4 characters. It will also look up the 
        /// Aliases (Assnswrd for AssassinSword, for example.)
        /// </summary>
        /// <param name="lookupString">The string specified by the user to use for lookup.</param>
        /// <returns></returns>
        private string ObtainSuggestions(string lookupString)
        {
            string result = "";
            StringBuilder builtResult;
            //TODO: Finish this and implement it into the command. 
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
        /// <summary>
        /// To be called from the bot's initialization, this caches the ChipLibrary JSON.
        /// </summary>
        public static void InitialCache()
        {
            ChipLibrary = JsonConvert.DeserializeObject<List<Chip>>(File.ReadAllText(BotTools.GetSettingString(BotTools.ConfigurationEntries.ChipLibraryFileLocation)));
            Console.WriteLine("Chip Library Initialized.");
        }
    }
}
