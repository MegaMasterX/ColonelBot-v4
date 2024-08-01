using Discord;
using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColonelBot_v4.Tools;
using ColonelBot_v4.Modules;

public class SupporterEnabledAttribute : PreconditionAttribute
{
    public override async Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context, ICommandInfo commandInfo, IServiceProvider services)
    {

        if (RoleModule.UserHasRole("Supporter", context.User as IGuildUser, context.Guild))
            return await Task.FromResult(PreconditionResult.FromSuccess());
        else
            if (RoleModule.UserHasRole("Moderator", context.User as IGuildUser, context.Guild))
                return await Task.FromResult(PreconditionResult.FromSuccess());
            else
                return await Task.FromResult(PreconditionResult.FromError("You must be a Moderator or Supporter to use this command."));
    }
}
