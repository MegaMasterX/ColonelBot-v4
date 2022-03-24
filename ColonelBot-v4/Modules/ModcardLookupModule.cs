using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using ColonelBot_v4.Models;
using System.Threading.Tasks;
using ColonelBot_v4.Tools;
using Newtonsoft.Json;
using System.IO;

namespace ColonelBot_v4.Modules
{
    public class ModcardLookupModule : ModuleBase<SocketCommandContext>
    {
        static List<ModCard> ModcardLibrary;

        public static void InitializeCache()
        {
            ModcardLibrary = JsonConvert.DeserializeObject<List<ModCard>>(File.ReadAllText(BotTools.GetSettingString(BotTools.ConfigurationEntries.ModcardLibraryFileLocation)));
            Console.WriteLine("Modcard Library Initailized.");
        }

        [Group("modcard")]
        public class LookupSpecificModcard : ModuleBase<SocketCommandContext>
        {
            [Command]
            public async Task LookupModcardAsync([Remainder] string remainder)
            {
                ModCard selectedCard;
                if (TryFindCardByName(remainder, out selectedCard))
                    await ReplyAsync("", embed: EmbedTool.ModcardEmbed(selectedCard));
                else if (TryFindCardByAlias(remainder, out selectedCard))
                    await ReplyAsync("", embed: EmbedTool.ModcardEmbed(selectedCard));
                else
                    await ReplyAsync(ObtainSuggestions(remainder));
            }

            public static bool TryFindCardByName(string cardName, out ModCard selectedCard)
            {
                selectedCard = ModcardLibrary.Find(x => x.Name.ToUpperInvariant().Equals(cardName.ToUpperInvariant()));
                return selectedCard != null;
            }

            public static bool TryFindCardByAlias(string cardName, out ModCard selectedCard)
            {
                selectedCard = ModcardLibrary.Find(x => x.Alias.ToUpperInvariant().Equals(cardName.ToUpperInvariant()));
                return selectedCard != null;
            }

            public static string ObtainSuggestions(string lookupString)
            {
                string result = "Modcard not found. Perhaps you meant: ";
                string Criteria = lookupString.Remove(3);
                List<ModCard> SearchResults = ModcardLibrary.FindAll(x => x.Name.ToUpper().StartsWith(Criteria.ToUpper()));
                foreach (ModCard item in SearchResults)
                {
                    result += $"{item.Name}     ";
                }

                if (result == "Modcard not found. Perhaps you meant: ")
                {
                    List<ModCard> AliasResults = ModcardLibrary.FindAll(x => x.Alias.ToUpper().Contains(Criteria.ToUpper()));
                    foreach (ModCard item in AliasResults)
                    {
                        string[] Aliases = item.Alias.Split(',');
                        for (int i = 0; i < Aliases.Length; i++)
                        {
                            if (Aliases[i].ToUpper().StartsWith(Criteria.ToUpper()))
                                result += $"{item.Name}    ";
                        }
                    }
                }

                return result;

            }

            
        }
    }
}
