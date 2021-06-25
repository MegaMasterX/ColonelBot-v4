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

        [Command("available"), Alias("atb","🦞"), RequireContext(ContextType.Guild)]
        public async Task GoATB()
        {
            var caller = Context.User as IGuildUser;
            //filter @ to prevent @Everyone calls and use Nickname, if available.
            var username = (caller.Nickname ?? caller.Username).Replace("@", "(at)"); 
            var role = GetRole("Available to Battle", Context.Guild);
            await AddATB(caller, role);
            await ReplyAsync($"You are now Available to Battle, {username}");
        }

        [Command("atm"), Alias("lmb","legs","moontime","🌙","🌑","🌕"), RequireContext(ContextType.Guild)]
        public async Task ItsMoonTimeAsync()
        {
            //Method to moon-up the caller.
            var caller = Context.User as IGuildUser;
            var username = (caller.Nickname ?? caller.Username).Replace("@", "(at)"); 
            var role = GetRole("Leg's MOON BATTLE!", Context.Guild);
            await AddMoonBattle(caller, role);
            await ReplyAsync($"You are now ready to MOON BATTLE, {username}!");

        }

        [Command("unmoon"), Alias("unm"), RequireContext(ContextType.Guild)]
        public async Task UnmoonAsync()
        {
            var caller = Context.User as IGuildUser;
            var username = (caller.Nickname ?? caller.Username).Replace("@", "(at)"); 
            var role = GetRole("Leg's MOON BATTLE!", Context.Guild);
            await RemoveMoonbattle(caller, role);
            await ReplyAsync("You are no longer available to MOON BATTLE.");
        }

        
        [Command("unavailable"), Alias("unav","notatb","🦐","unatb","unavail", "<:shrimpy:595465516286738463>"), RequireContext(ContextType.Guild)]
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

        public static SocketRole GetRole(string RoleName, SocketGuild guild)
        {
            
            var role = guild.Roles.SingleOrDefault(r => r.Name.ToUpper() == RoleName.ToUpper());
            return role;
        }
    }
}
