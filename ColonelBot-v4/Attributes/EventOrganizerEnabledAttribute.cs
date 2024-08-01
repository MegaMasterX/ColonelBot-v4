using Discord;
using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColonelBot_v4.Tools;
using ColonelBot_v4.Modules;

internal class EventOrganizerEnabledAttribute : PreconditionAttribute
    {
        public override async Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context, ICommandInfo commandInfo, IServiceProvider services)
        {

            if (RoleModule.UserHasRole("Event Organizer", context.User as IGuildUser, context.Guild))
                return await Task.FromResult(PreconditionResult.FromSuccess());
            else
                if (RoleModule.UserHasRole("Moderator", context.User as IGuildUser, context.Guild))
                return await Task.FromResult(PreconditionResult.FromSuccess());
            else
                return await Task.FromResult(PreconditionResult.FromError("You must be a Event Organizer to use this command."));
        }
    }
