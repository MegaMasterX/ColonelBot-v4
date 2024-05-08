using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Discord;
using Discord.Interactions;
using System.Threading.Tasks;
using ColonelBot_v4.Models;
using ColonelBot_v4.Tools;
using Newtonsoft.Json;
using Discord.Interactions;
using System.Linq;
using Microsoft.Extensions.Options;

namespace ColonelBot_v4.Modules
{
    public class LookupModule : InteractionModuleBase<SocketInteractionContext>
    {

        public InteractionService commands { get; set; }
        private InteractionHandler _handler;
        public LookupModule(InteractionHandler handler)
        {
            _handler = handler;
            
            Console.WriteLine("Modcard Library Initailized.");
        }

        [Group("lookup", "Searches for a specific chip or patch card. If not found, a fuzzy search is done.")]
        public class LookupSpecificGame : InteractionModuleBase<SocketInteractionContext>
        {
            [SlashCommand("chip", "Looks up a chip with the specified name")]
            public async Task LookupDefaultAsync([Autocomplete(typeof(ChipLookupAutoCompleteHandler))] string ChipName)
            {//!lookup <chipname> - 
                Chip locatedChip;
		
		    if (ChipName.Length == 1) {
		        await RespondAsync(ObtainCodeResults(ChipName));
		    }else if (TryFindChipByName(ChipName, out locatedChip))
                {
                    await RespondAsync("", embed: EmbedTool.ChipEmbed(locatedChip));
                }else if (TryFindChipByAlias(ChipName, out locatedChip))
                {
                    await RespondAsync("", embed: EmbedTool.ChipEmbed(locatedChip));
                }else
                    await RespondAsync(ObtainSuggestions(ChipName));
            }

            [SlashCommand("code", "Lookup all chips with the specified code.")] //!lookup code <specified>
            public async Task LookupChipsByCode(string code)
            {
                await RespondAsync(ObtainCodeResults(code)); //You don't have to worry about trimming here, the lookup method only gets the first char.
            }

            [SlashCommand("patchcard", "Looks up a Patch Card")]
            public async Task LookupPatchCardAsync(string PatchcardName)
            {
                PatchCard selectedCard;
                if (TryFindCardByName(PatchcardName, out selectedCard))
                    await RespondAsync("", embed: EmbedTool.ModcardEmbed(selectedCard));
                else if (TryFindCardByAlias(PatchcardName, out selectedCard))
                    await RespondAsync("", embed: EmbedTool.ModcardEmbed(selectedCard));
                else
                    await RespondAsync(ObtainSuggestions(PatchcardName));
            }

            public static bool TryFindCardByName(string cardName, out PatchCard selectedCard)
            {
                selectedCard = PatchCardLibrary.Instance.Library.Find(x => x.Name.ToUpperInvariant().Equals(cardName.ToUpperInvariant()));
                return selectedCard != null;
            }

            public static bool TryFindCardByAlias(string cardName, out PatchCard selectedCard)
            {
                selectedCard = PatchCardLibrary.Instance.Library.Find(x => x.Alias.ToUpperInvariant().Equals(cardName.ToUpperInvariant()));
                return selectedCard != null;
            }

            public static string ObtainSuggestions(string lookupString)
            {
                string result = "Patch Card not found. Perhaps you meant: ";
                string Criteria = lookupString.Remove(3);
                List<PatchCard> SearchResults = PatchCardLibrary.Instance.Library.FindAll(x => x.Name.ToUpper().StartsWith(Criteria.ToUpper()));
                foreach (PatchCard item in SearchResults)
                {
                    result += $"{item.Name}     ";
                }

                if (result == "Patch Card not found. Perhaps you meant: ")
                {
                    List<PatchCard> AliasResults = PatchCardLibrary.Instance.Library.FindAll(x => x.Alias.ToUpper().Contains(Criteria.ToUpper()));
                    foreach (PatchCard item in AliasResults)
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

         public static bool TryFindChipByName(string chipName, out Chip selectedChip)
         {
            selectedChip = ChipLibrary.Instance.Library.Find(x => x.Name.ToUpperInvariant().Contains(chipName.ToUpperInvariant()));
            return selectedChip != null;
         }

        /// <summary>
        /// Returns the Chip type of the specified chip, assuming it's been verified and found with SearchChip.
        /// </summary>
        /// <param name="ChipName"></param>
        /// <returns>Returns a Chip model instance that matches the chip that was located in the Library.</returns>
        public static Chip GetChipByName(string ChipName)
        {
           
            Chip selectedChip = null;
            selectedChip = ChipLibrary.Instance.Library.Find(x => x.Name.ToUpper() == ChipName.ToUpper());
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
        const string lookupFailure = "Chip not found.";
        public static bool TryFindChipByAlias(string chipName, out Chip selectedChip)
        {
            selectedChip = ChipLibrary.Instance.Library.Find(x => x.Alias.ToUpperInvariant().Contains(chipName.ToUpperInvariant()));
            return selectedChip != null;
        }

        
        

        /// <summary>
        /// Returns all chips that contain the specified chip code. 
        /// </summary>
        /// <param name="CodeLookup"></param>
        /// <returns></returns>
        const string NoCodeFound = "No chips could be found with the specified code. Please try again with a proper letter.";
        private static string ObtainCodeResults(string CodeLookup)
        {
            string result = "";
            StringBuilder Results = new StringBuilder();
            List<Chip> CodeResults = new List<Chip>();
            CodeResults = ChipLibrary.Instance.Library.FindAll(x => x.Codes.Contains(CodeLookup.ToUpper().ToCharArray()[0]));
            foreach (Chip item in CodeResults)
            {
                result += $"{item.Name}   ";
            }
            if (result == "")
                result = NoCodeFound;
            return result;
        }
        /// <summary>
        /// To be called from the bot's initialization, this caches the ChipLibrary JSON.
        /// </summary>
        public static void InitialCache()
        {
            
        }
    }

    public class ChipLookupAutoCompleteHandler : AutocompleteHandler
    {
        public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
        {
            var interactionData = context.Interaction.Data;
            string targetOption = ((Discord.WebSocket.SocketAutocompleteInteractionData)context.Interaction.Data).Options.Last().Value.ToString();


            string[] results = ObtainSuggestions(targetOption);
            if (results.Count() != 0)
            {
                List<AutocompleteResult> acResults = new List<AutocompleteResult>();
                foreach (var suggestion in results)
                {
                    acResults.Add(new AutocompleteResult(suggestion, suggestion));
                }

                return AutocompletionResult.FromSuccess(acResults.Take(25)); //Discord limiatation.

            }
            else
            {
                return AutocompletionResult.FromError(InteractionCommandError.Unsuccessful, "Chip not found.");
            }
        }

        /// <summary>
        /// This method will return a built list of all chips matching the Lookup string criteria.  
        /// 
        /// Lookups, if not found, will obtain a list of chips containing the first 4 characters. It will also look up the 
        /// Aliases (Assnswrd for AssassinSword, for example.)
        /// </summary>
        /// <param name="lookupString">The string specified by the user to use for lookup.</param>
        /// <returns></returns>
        public static string[] ObtainSuggestions(string lookupString)
        {
            List<string> results = new List<string>();
            
            string Criteria = lookupString;
            string verOrClass = lookupString;

            if (lookupString.Length > 4){
                Criteria = lookupString.Remove(3);
            }
            if (lookupString.Length > 2){
                verOrClass = lookupString.Remove(0, lookupString.Length - 2); //This trims all but the last 2 chars to obtain the version or class of chip (2, EX, SP etc)
            }
            
            List<Chip> FindResults = ChipLibrary.Instance.Library.FindAll(x => x.Name.ToUpper().StartsWith(Criteria.ToUpper()));
            
            foreach (Chip chp in FindResults)
            {
                results.Add(chp.Name); //+= $"{chp.Name}   ";

            }

            if (results.Count() == 0)
            {// The fuzzy search did not return a result, check the aliases.
                List<Chip> AliasResults = ChipLibrary.Instance.Library.FindAll(x => x.Alias.ToUpper().Contains(Criteria.ToUpper()));
                foreach (Chip item in AliasResults)
                {
                    //The chips' aliases will be seperated with ", "
                    string[] Aliases = item.Alias.Split(',');
                    for (int i = 0; i < Aliases.Length; i++)
                    {
                        if (Aliases[i].StartsWith(Criteria.ToUpper()))
                            results.Add(item.Name); // += $"{item.Name}   ";
                    }
                }
            }
            if (results.Count() == 0)
            {// No fuzzy results were found in the aliases either. Change the return string.
                //An error will be returned by the autocomplete component.
            }
            return results.ToArray();
        }
    }
}
