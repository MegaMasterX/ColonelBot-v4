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

        [Command("available"), Alias("atb"), RequireContext(ContextType.Guild)]
        public async Task GoATB()
        {
            var caller = Context.User as IGuildUser;
            var role = GetRole("Available to Battle", Context.Guild);
            if (!caller.RoleIds.Contains(role.Id))
            {//The user does not currently have the ATB Role.
                await caller.AddRoleAsync(role, null);
                await ReplyAsync("", false, EmbedTool.ChannelMessage("You are now Available to Battle.", Context.Client.CurrentUser));
            } else
            {//The user did !atb and already has the role.
                await ReplyAsync("", false, EmbedTool.ChannelMessage("You are already Available to Battle.", Context.Client.CurrentUser));
            }
        }

        [Command("unavailable"), Alias("unav"), RequireContext(ContextType.Guild)]
        public async Task RemoveATB()
        {
            var caller = Context.User as IGuildUser;
            var role = GetRole("AVAILABLE TO BATTLE", Context.Guild);
            if (caller.RoleIds.Contains(role.Id))
            {//The user has called Unavailable and has the ATB Role
                await caller.RemoveRoleAsync(role, null);
                await ReplyAsync("", false, EmbedTool.ChannelMessage("You are no longer Available to Battle.", Context.Client.CurrentUser));
            }else
            {//The user called Unavailable but does not have the ATB role.
                await ReplyAsync("", false, EmbedTool.ChannelMessage("You are not Available to Battle.", Context.Client.CurrentUser));
            }
        }

        [Command("license")]
        public async Task ToggleLicenseRole()
        {//Toggles the Netbattler role when called - adding it if it's not present or removing it if it is.
            var caller = Context.User as IGuildUser;
            var role = GetRole("Netbattler", Context.Guild);
            if (caller.RoleIds.Contains(role.Id))
            {//The caller already has the License role, remove it.
                await caller.RemoveRoleAsync(role, null);
                await ReplyAsync("", false, EmbedTool.ChannelMessage("Your Netbattler role has been removed.", Context.Client.CurrentUser));
            }else
            {//The caller does not have the License role, add it.
                await caller.AddRoleAsync(role, null);
                await ReplyAsync("", false, EmbedTool.ChannelMessage("You have been granted the Netbattler role.", Context.Client.CurrentUser));
            }
        }

        [Command("deckmaster")]
        public async Task ToggleDeckmasterRole()
        {//Toggles the Deckmaster Role
            var caller = Context.User as IGuildUser;
            var role = GetRole("Deckmaster", Context.Guild);
            if (caller.RoleIds.Contains(role.Id))
            {//The caller already has the Deckmaster role, remove it.
                await caller.RemoveRoleAsync(role, null);
                await ReplyAsync("", false, EmbedTool.ChannelMessage("Your Deckmaster role has been removed.", Context.Client.CurrentUser));
            }
            else
            {//The caller does not have the Deckmaster role, add it.
                await caller.AddRoleAsync(role, null);
                await ReplyAsync("", false, EmbedTool.ChannelMessage("You have been granted the Deckmaster role. This role is pingable.", Context.Client.CurrentUser));
            }
        }
           
        private void ToggleRole(IGuildUser caller, SocketRole role)
        {
            //TODO: Move the code for toggling roles in individual methods here, also build the Embed here and
            //      change this to return type Embed instead of being a Void. See Feature 12 in VSO. 
        }

        private SocketRole GetRole(string RoleName, SocketGuild guild)
        {
            
            var role = guild.Roles.SingleOrDefault(r => r.Name.ToUpper() == RoleName.ToUpper());
            return role;
        }
    }
}
