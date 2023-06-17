using System;
using System.Threading.Tasks;
using System.Linq;

using Discord;
using Discord.Interactions;
using Discord.WebSocket;

using ColonelBot_v4.Tools;

//V4 cleans up some redunant code.

namespace ColonelBot_v4.Modules
{
    public class RoleModule : InteractionModuleBase<SocketInteractionContext>
    {

        [SlashCommand("atb", "Marks you as Available to Battle!")]
        public async Task GoATB()
        {
            var caller = Context.User as IGuildUser;
            //filter @ to prevent @Everyone calls and use Nickname, if available.
            var username = (caller.Nickname ?? caller.Username).Replace("@", "(at)"); 
            var role = GetRole("Available to Battle", Context.Guild);
            await AddATB(caller, role);
            await RespondAsync($"You are now Available to Battle, {username}");
        }

        [SlashCommand("atm", "Marks you as Available to MOON BATTLE."), RequireContext(ContextType.Guild)]
        public async Task ItsMoonTimeAsync()
        {
            //Method to moon-up the caller.
            var caller = Context.User as IGuildUser;
            if (HasRole("MOON BATTLER", caller, Context.Guild))
            {
                var username = (caller.Nickname ?? caller.Username).Replace("@", "(at)"); 
                var role = GetRole("Leg's MOON BATTLE!", Context.Guild);
                await AddMoonBattle(caller, role);
                await RespondAsync($"You are now ready to MOON BATTLE, {username}!");
            }else
                await RespondAsync("You are not permitted to call this command.");
            

        }

        [SlashCommand("unmoon", "Removes your Available to MOON BATTLE role."), RequireContext(ContextType.Guild)]
        public async Task UnmoonAsync()
        {
            var caller = Context.User as IGuildUser;
            if (HasRole("MOON BATTLER", caller, Context.Guild))
            {
                var username = (caller.Nickname ?? caller.Username).Replace("@", "(at)"); 
                var role = GetRole("Leg's MOON BATTLE!", Context.Guild);
                await RemoveMoonbattle(caller, role);
                await RespondAsync("You are no longer available to MOON BATTLE.");
            }else
                await RespondAsync("You are not permitted to call this command.");
        }

        
        [SlashCommand("unav", "Removes you from Available to Battle!"), RequireContext(ContextType.Guild)]
        public async Task RemoveATB()
        {
            var caller = Context.User as IGuildUser;
            //filter @ to prevent @Everyone calls and use Nickname, if available.
            var username = (caller.Nickname ?? caller.Username).Replace("@", "(at)"); // prepare username
            var role = GetRole("AVAILABLE TO BATTLE", Context.Guild);
            await RemoveATB(caller, role);
            await RespondAsync($"You are no longer Available to Battle, {username}");
        }

        [SlashCommand("license", "Adds the License role to you, marking you as an official Netbattler!")]
        public async Task ToggleLicenseRole()
        {//Toggles the Netbattler role when called - adding it if it's not present or removing it if it is.
            var caller = Context.User as IGuildUser;
            var role = GetRole("Netbattler", Context.Guild);
            await ToggleRole(caller, role);
            await RespondAsync("Added License to you.");
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

        public static bool UserHasRole(ulong RoleID, IGuildUser caller, IGuild guild)
        {
            IRole role = GetRole(RoleID, guild);
            if (caller.RoleIds.Contains(role.Id))
                return true;
            else
                return false;
        }

        public static bool UserHasRole(string RoleName, IGuildUser caller, IGuild guild)
        {
            IRole role = GetRole(RoleName, guild);
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

        public static IRole GetRole(string RoleName, IGuild guild)
        {

            var role = guild.Roles.SingleOrDefault(r => r.Name.ToUpperInvariant() == RoleName.ToUpperInvariant());
            return role;

        }

        public static IRole GetRole(ulong RoleID, IGuild guild)
        {
            var role = guild.Roles.SingleOrDefault(r => r.Id == RoleID);
            return role;
        }

        //PROD_TODO: Clean up Legacy role commands after the module conversion is done.
    }
}
