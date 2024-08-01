using Discord;
using Discord.Interactions;
using System;
using System.Threading.Tasks;
using ColonelBot_v4.Modules;

internal class DeveloperRequiredAttribute : PreconditionAttribute
{
    public override async Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context, ICommandInfo commandInfo, IServiceProvider services)
    {
        if (RoleModule.UserHasRole("Dev Colonel", context.User as IGuildUser, context.Guild))
            return await Task.FromResult(PreconditionResult.FromSuccess());
        else if (RoleModule.UserHasRole("Moderator", context.User as IGuildUser, context.Guild))
                return await Task.FromResult(PreconditionResult.FromSuccess());
            else
                return await Task.FromResult(PreconditionResult.FromError("You must be a approved Developer to use this command."));
    }
}