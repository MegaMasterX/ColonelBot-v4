using System;
using System.Threading.Tasks;
using System.Linq;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using ColonelBot_v4.Tools;

//V4 cleans up some redunant code.

namespace ColonelBot_v4.Modules
{
    public class RoleModule : ModuleBase<SocketCommandContext>
    {

        [Command("available"), Alias("atb","🦞","🐱","<:EguchiHype:596812876434374667>"), RequireContext(ContextType.Guild)]
        public async Task GoATB()
        {
            var caller = Context.User as IGuildUser;
            //filter @ to prevent @Everyone calls and use Nickname, if available.
            var username = (caller.Nickname ?? caller.Username).Replace("@", "(at)"); 
            var role = GetRole("Available to Battle", Context.Guild);
            await AddATB(caller, role);
            await ReplyAsync($"You are now Available to Battle, {username}");
        }

        [Command("atm"), Alias("lmb","legs","moontime","🌙","🌑","🌕","🌚","<:MegaLegPose:965794241727066112>","<:RegalRisen:972553613815717959>"), RequireContext(ContextType.Guild)]
        public async Task ItsMoonTimeAsync()
        {
            //Method to moon-up the caller.
            var caller = Context.User as IGuildUser;
            if (HasRole("MOON BATTLER", caller, Context.Guild))
            {
                var username = (caller.Nickname ?? caller.Username).Replace("@", "(at)"); 
                var role = GetRole("Leg's MOON BATTLE!", Context.Guild);
                await AddMoonBattle(caller, role);
                await ReplyAsync($"You are now ready to MOON BATTLE, {username}!");
            }else
                await ReplyAsync("You are not permitted to call this command.");
            

        }

        [Command("unmoon"), Alias("unm", "unlegs","🌞","☀️","<:MegaBedLegPose:965794604689530880>",,"<:RegalSleep:972553613815717959>"), RequireContext(ContextType.Guild)]
        public async Task UnmoonAsync()
        {
            var caller = Context.User as IGuildUser;
            if (HasRole("MOON BATTLER", caller, Context.Guild))
            {
                var username = (caller.Nickname ?? caller.Username).Replace("@", "(at)"); 
                var role = GetRole("Leg's MOON BATTLE!", Context.Guild);
                await RemoveMoonbattle(caller, role);
                await ReplyAsync("You are no longer available to MOON BATTLE.");
            }else
                await ReplyAsync("You are not permitted to call this command.");
        }

        
        [Command("unavailable"), Alias("unav","notatb","🦐","unatb","unavail", "<:shrimpy:595465516286738463>","🐦","🐔"), RequireContext(ContextType.Guild)]
        public async Task RemoveATB()
        {
            var caller = Context.User as IGuildUser;
            //filter @ to prevent @Everyone calls and use Nickname, if available.
            var username = (caller.Nickname ?? caller.Username).Replace("@", "(at)"); // prepare username
            var role = GetRole("AVAILABLE TO BATTLE", Context.Guild);
            await RemoveATB(caller, role);
            await ReplyAsync($"You are no longer Available to Battle, {username}");
        }

        [Command("linkcable"), Alias("legacybattler")]
        public async Task ToggleLegacy()
        {
            var caller = Context.User as IGuildUser;
            var role = GetRole("Legacy Battler", Context.Guild);
            await ToggleRole(caller, role);
        }

        [Command("license"), Alias("licence")]
        public async Task ToggleLicenseRole()
        {//Toggles the Netbattler role when called - adding it if it's not present or removing it if it is.
            var caller = Context.User as IGuildUser;
            var role = GetRole("Netbattler", Context.Guild);
            await ToggleRole(caller, role);
        }

        [Command("deckmaster")]
        public async Task ToggleDeckmasterRole()
        {//Toggles the Deckmaster Role
            var caller = Context.User as IGuildUser;
            var role = GetRole("Deckmaster", Context.Guild);
            await ToggleRole(caller, role);
        }
        
        [Group("moonrole")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        public class MoonRoleModule : ModuleBase<SocketCommandContext>
        {
            SocketRole moonrole = GetRole("MOON BATTLER", Context.Guild);

            [Command("add"), RequireContext(ContextType.Guild)]
            public async Task MoonRoleAdd([Remainder] string Mentions)
            {
                int option = 0;
                string status = MoonRoleShared(option, Mentions);
                await ReplyAsync(status);
            }

            [Command("remove"), Alias("del", "delete"), RequireContext(ContextType.Guild)]
            public async Task MoonRoleRemove([Remainder] string Mentions)
            {
                int choice = 1;
                string status = MoonRoleShared(option, Mentions);
                await ReplyAsync(status);
            }

            public async Task<string> MoonRoleShared(int choice, string param)
            {
                var userlist = ParseMoonList(param);

                // if I could get it to return the rejects, this is where I would construct a string with sections of the original parameter that did not return valid users.
                // potential reasons for failed matches could be:
                //  - typo/junk text
                //  - user is not in the server
                //  - I'm bad at writing code to parse strings
                string return1 = "";

                string return2 = IterateMoonList(choice, userlist);
                return return1 + return2;
            }


            public static List<SocketGuildUser> ParseMoonList(string param)
            {   // Iterate through the parameter and isolate each recognizable User Mention, return a table that can pass individual `IUser target` params
                // also return a table of string snippets that failed to represent a User 
                
                List<SocketGuildUser> valid = new List<SocketGuildUser>();
                List<string> rejects = new List<string>();
                List<ulong> IDs = new List<ulong>();

                // parse param string, populate IDs with results
                string dirty = param.Replace("@!","@")

                // need to get each instance of contiguous numbers proceeding "<@"
                // I tried working with string indexes but I only managed to stub my toe.
                // each instance of contiguous numbers should be added to IDs


                foreach (var id in IDs) {
                    var ii = Context.Guild.GetUser(id);
                    // we need to verify that this returned a real user ID
                    // and that the user is in the server
                    if (){
                        valid.Add(ii);
                    }
                    
                }

                //return new Tuple<struct,struct>(valid,rejects);
                return valid;
            }

            public async Task<string> IterateMoonList(int choice, SocketGuildUser users, SocketRole role)
            { // Iterate through the table of valid User Mentions
                int countcount = 0;
                int successcount = 0;
                int skipcount = 0;
                bool skipbool = false;
                string skipped = "";
                string results = "";
                string phrase1 = "";
                string phrase2 = "";
                if (choice == 0){
                    phrase1 = "applied role to";
                    phrase2 = "have";
                }else{
                    phrase1 = "removed role from";
                    phrase2 = "don't have";
                }
                // I'm probably not using the item type correctly yet. This is currently passing SocketGuildUser and the existing examples pass IUser
                foreach (var item in users)
                {
                    // I suspect there's some sort of rate limit that this loop needs to be mindful of. Currently I don't know what to do about that.
                    countcount += 1;
                    skipbool = false;
                    if (choice == 0){
                        if !(HasRole(role, item, Context.Guild)){
                            await item.AddRoleAsync(role, null);
                        }else{
                            skipbool = true;
                        }
                    }
                    else{
                        if (HasRole(role, item, Context.Guild)){
                            await item.RemoveRoleAsync(role, null);
                        }else{
                            skipbool = true;
                        }
                    }
                    if (skipbool == false){ successcount += 1;}
                    else{
                        skipcount += 1;
                        skipped += $"{item.Mention} ";
                    }
                }

                // Basic results, say how many usernames were recognized, how many users had their role status changed.
                results += $"Recognized {countcount} users, {phrase1} {successcount} users.\n"

                // If users were skipped, say how many and @mention the specific users.
                if (skipcount > 0){
                    results += $"{skipcount} users already {phrase2} the role: {skipped} \n";
                }

                return results;
            }
        }

        public async Task<Embed> ToggleRole(IGuildUser caller, SocketRole role)
        {
            string RoleResponseText = "";
            
            if (caller.RoleIds.Contains(role.Id))
            {//The caller already has the role, remove it.
                await caller.RemoveRoleAsync(role, null);
                RoleResponseText = "The " + role.Name + " role has been removed.";
            }
            else
            {//The caller does not have the role, add it.
                await caller.AddRoleAsync(role, null);
                RoleResponseText = "The " + role.Name + " role has been added.";
            }
            var embed = new EmbedBuilder
            {
                Color = new Color(0xffcf39)
            };

            embed.AddField("Role Updated", RoleResponseText);
            await ReplyAsync("", embed: embed.Build());
            return embed.Build();
        }

        private async Task<string> AddMoonBattle(IGuildUser caller, SocketRole role)
        {
            string ResponseText = "";
            if (caller.RoleIds.Contains(role.Id))
                ResponseText = "You are already ready to MOON BATTLE.";
            else
            {
                await caller.AddRoleAsync(role, null);
                ResponseText = "You are now ready to MOON BATTLE.";
            }
            return ResponseText;
        }


        private async Task<string> AddATB(IGuildUser caller, SocketRole role)
        {
            string ResponseText = "";
            if (caller.RoleIds.Contains(role.Id))
            { //The user already has ATB.
                ResponseText = "You are already Available to Battle.";   
            }else
            {
                await caller.AddRoleAsync(role, null);
                ResponseText = "You are now Available to Battle.";
            }
            
            return ResponseText; //Task 60
            
            
        }

        private async Task<string> RemoveMoonbattle(IGuildUser caller, SocketRole role)
        {
            string ResponseText = "";
            if (caller.RoleIds.Contains(role.Id))
            { 
                await caller.RemoveRoleAsync(role, null);
                ResponseText = "You are no longer ready to MOON BATTLE.";   
            }else
            {//The user isn't ATB and it can't be removed.
                ResponseText = "You are not available to MOOON BATTLE.";
            }

            return ResponseText; //Task 60
        }

        private async Task<string> RemoveATB(IGuildUser caller, SocketRole role)
        {
            string ResponseText = "";
            if (caller.RoleIds.Contains(role.Id))
            { 
                await caller.RemoveRoleAsync(role, null);
                ResponseText = "You are no longer Available to Battle.";   
            }else
            {//The user isn't ATB and it can't be removed.
                ResponseText = "You are not Available to Battle.";
            }

            return ResponseText; //Task 60
            
        }

        ///<summary>
        /// Checks to see if the Guild User has a role RoleName.
        ///</summary
        public static bool HasRole(string RoleName, IGuildUser caller, SocketGuild guild)
        {
            SocketRole role = GetRole(RoleName, guild);
            if (caller.RoleIds.Contains(role.Id))
                return true;
            else
                return false;
        }

        public static SocketRole GetRole(string RoleName, SocketGuild guild)
        {
            
            var role = guild.Roles.SingleOrDefault(r => r.Name.ToUpper() == RoleName.ToUpper());
            return role;
        }
    }
}
