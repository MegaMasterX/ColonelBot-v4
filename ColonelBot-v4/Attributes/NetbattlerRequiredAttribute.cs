using Discord;
using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColonelBot_v4.Tools;
using ColonelBot_v4.Modules;

public class NetbattlerRequiredAttribute : PreconditionAttribute
{
    public override async Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context, ICommandInfo commandInfo, IServiceProvider services)
    {
        ulong NetbattlerRole = 319627760945463297;

        if (RoleModule.UserHasRole(NetbattlerRole, context.User as IGuildUser, context.Guild))
            return await Task.FromResult(PreconditionResult.FromSuccess());
        else
            return await Task.FromResult(PreconditionResult.FromError("This command is only usable by those with the Netbattler role. Please use /license to obtain the role."));
    }
}