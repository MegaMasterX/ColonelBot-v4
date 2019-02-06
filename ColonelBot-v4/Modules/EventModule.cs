using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Text;
using Discord.Commands;
using Discord;
using Discord.WebSocket;

using ColonelBot_v4.Tools;
using ColonelBot_v4.Models;

using Newtonsoft.Json;

namespace ColonelBot_v4.Modules
{
    public class EventModule : ModuleBase<SocketCommandContext>
    {

        static List<EventParticipant> ParticipantList = new List<EventParticipant>();

        /// <summary>
        /// Checks to see if the user has already registered and, if not, adds them to the active event.
        /// </summary>
        /// <param name="NetbattlerName"></param>
        /// <returns></returns>
        [Command("event join")]
        public async Task JoinEventAsync([Remainder] string NetbattlerName)
        {
            SyncParticipantList();
            if (IsEventActive())
            {//Is the event active?
                if (GetActiveEvent().RegistrationOpen == true)
                {//Is registration open?
                    if (IsParticipantRegistered(Context.User.Id) == true)
                    {//The user is already registered.
                        await ReplyAsync("", embed: EmbedTool.CommandError($"You are already registered for the {GetActiveEvent().EventName} event."));
                    }
                    else
                    {
                        EventParticipant newParticipant = new EventParticipant(NetbattlerName.Replace('@', ' '), Context.User.Id);
                        ParticipantList.Add(newParticipant);
                        WriteParticipantList();
                        await ToggleRole(Context.User as IGuildUser, RoleModule.GetRole("Official Netbattler", Context.Guild));//Add the Official Netbattler role to the user.
                        await ReplyAsync("", embed: EmbedTool.ChannelMessage($"You have registered for the event {GetActiveEvent().EventName} sucessfully!"));
                    }
                }
                else //registration is closed.
                    await ReplyAsync("", embed: EmbedTool.CommandError($"{GetActiveEvent().EventName} is currently not accepting registrations."));
             
            }
            else
                await ReplyAsync("", embed: EmbedTool.CommandError("There currently isn't an active event."));
        }

        [Command("event update")]
        public async Task UpdateNetbattlerNameAsync([Remainder] string NewName)
        {
            SyncParticipantList();
            var caller = Context.User as IGuildUser;
            if (IsEventActive())
            {
                if (IsParticipantRegistered(caller.Id))
                {
                    GetParticipant(caller).NetbattlerName = NewName.Replace('@', ' ');
                    WriteParticipantList();
                    await ReplyAsync("Updated your Netbattler Name successfully.");
                }
                else
                    await ReplyAsync("", embed: EmbedTool.CommandError($"You are not registered for {GetActiveEvent().EventName}."));
            }
            else
                await ReplyAsync("", embed: EmbedTool.CommandError("There currently isn't an active event."));
            
        }

        [Command("event drop")]
        public async Task DropEventAsync()
        {
            SyncParticipantList();
            if (IsEventActive())
            {
                //We don't need to check for open registration to drop.
                var caller = Context.User as IGuildUser;
                if (IsParticipantRegistered(Context.User.Id) == true)
                {//The user is already registered.
                    ParticipantList.Remove(GetParticipant(caller));
                    WriteParticipantList();
                    await ToggleRole(Context.User as IGuildUser, RoleModule.GetRole("Official Netbattler", Context.Guild)); //Remove the Official Netbattler role.
                    await ReplyAsync("", embed: EmbedTool.ChannelMessage($"You have dropped from {GetActiveEvent().EventName}"));
                }
                else
                {
                    await ReplyAsync("", embed: EmbedTool.CommandError($"You are not registered for the {GetActiveEvent().EventName} event."));

                }
            }
            else
                await ReplyAsync("", embed: EmbedTool.CommandError("There currently isn't an active event."));
        }



        [Command("event info")]
        public async Task CallEventInfo()
        {
            if (IsEventActive())
                await ReplyAsync("", embed: EmbedTool.EventInfoEmbed(GetActiveEvent()));
        }

        //Event Administration

        [Command("event admin end")]
        public async Task CloseEventAsync()
        {//Closes the event and delets the Participant JSON and the Event JSON. 
            if (IsEventActive())
            {
                File.Delete($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Data{Path.DirectorySeparatorChar}Event.json");
                File.Delete($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Data{Path.DirectorySeparatorChar}Registration.json");
                Directory.Delete($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Setups");
                await ReplyAsync("", embed: EmbedTool.ChannelMessage("The Event has concluded."));
            }
        }

        [Command("event admin getsetups")]
        public async Task ObtainSetupsAsync()
        {
            if (IsEventOrganizer(Context.User as IGuildUser, Context.Guild))
            {

                string SourcePath = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Setups";
                string ZipTarget = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Cache{Path.DirectorySeparatorChar}ParticipantSetups.zip";
                ZipFile.CreateFromDirectory(SourcePath, ZipTarget);

                await Context.User.SendFileAsync($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Cache{Path.DirectorySeparatorChar}ParticipantSetups.zip", "");
                await ReplyAsync("You have e-mail.");
                File.Delete($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Cache{Path.DirectorySeparatorChar}ParticipantSetups.zip");
            }
            
        }

        [Command("event admin remind")]
        public async Task RemindPlayersAsync()
        {
            
        }

        [Command("event admin list")]
        public async Task EventListAsync()
        {
            //Builds a list of participants to send to the organizer.
            if (IsEventOrganizer(Context.User as IGuildUser, Context.Guild))
            {
                SyncParticipantList(); //Ensure the List is up to date.
                string Result = $"**{GetActiveEvent().EventName} Participant List**\n\n";

                for (int i = 0; i < ParticipantList.Count; i++)
                {
                    Result += $"{ParticipantList[i].NetbattlerName} ({ParticipantList[i].UserID}) - Setup Submitted: {ParticipantList[i].SetupSubmitted.ToString()}\n";
                }
                await Context.User.SendMessageAsync(Result);
            } else
                await ReplyAsync("", embed: EmbedTool.CommandError("You are not authorized to obtain the participant list."));
           
        }

        [Command("event create")]
        [RequireContext(ContextType.Guild)]
        public async Task CreateEventAsync([Remainder] string EventName)
        {
            // 1. Check to see if an event is already active.
            if (IsEventActive())
                await ReplyAsync("", embed: EmbedTool.CommandError("An event is already active and a new one cannot be created."));
            else
            {
                // 2. Check to see if the caller is an Event Organizer.
                var caller = Context.User as IGuildUser;
                if (IsEventOrganizer(caller, Context.Guild))
                {
                    // 3. Create the Event.
                    Event newEvent = new Event(Context.User.Id, EventName);
                    newEvent.Description = "Not yet configured.";
                    newEvent.RulesURL = "Not yet configured.";
                    newEvent.StartDate = "Not yet configured.";
                    newEvent.RegistrationOpen = false;
                    newEvent.AcceptingSetups = false;

                    // 4. Serialize the event into a JSON.
                    File.WriteAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Data{Path.DirectorySeparatorChar}Event.json", JsonConvert.SerializeObject(newEvent));
                    File.WriteAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Data{Path.DirectorySeparatorChar}Registration.json", JsonConvert.SerializeObject(ParticipantList));
                    Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Setups");
                    // 5. Confirm.
                    await ReplyAsync($"The event, {EventName}, has been created.");
                }
                else
                    await ReplyAsync("You are not authorized to create events.");
            }


        }

        [Command("event admin title")]
        public async Task UpdateEventTitleAsync([Remainder] string EventTitle)
        {
            if (IsEventActive() && IsEventOrganizer(Context.User as IGuildUser, Context.Guild))
            {//The event is active and the caller is an authorized user.
                UpdateEventData(EventFields.Title, EventTitle);
                await ReplyAsync($"The event title has been updated to {EventTitle}.");
            }
            else
                await ReplyAsync("", embed: EmbedTool.CommandError("There is not an active event or you are not authorized to make changes to the event."));

        }

        [Command("event admin rules")]
        public async Task UpdateEventRulesAsync([Remainder] string RulesURL)
        {
            if (IsEventActive() && IsEventOrganizer(Context.User as IGuildUser, Context.Guild))
            {
                UpdateEventData(EventFields.Rules, RulesURL);
                await ReplyAsync("The Event Rules URL has been updated.");
            }
            else
                await ReplyAsync("", embed: EmbedTool.CommandError("There is not an active event or you are not authorized to make changes to the event."));

        }

        [Command("event admin description")]
        public async Task UpdateEventDescriptionAsync([Remainder] string Description)
        {
            if (IsEventActive() && IsEventOrganizer(Context.User as IGuildUser, Context.Guild))
            {
                UpdateEventData(EventFields.Description, Description);
                await ReplyAsync("The Event Descripion has been updated.");
            }
            else
                await ReplyAsync("", embed: EmbedTool.CommandError("There is not an active event or you are not authorized to make changes to the event."));
            
        }

        [Command("event admin startdate")]
        public async Task UpdateEventDateAsync([Remainder] string Date)
        {
            if (IsEventActive() && IsEventOrganizer(Context.User as IGuildUser, Context.Guild))
            {
                UpdateEventData(EventFields.StartDate, Date);
                await ReplyAsync("The Event date has been upated.");
            }
            else
                await ReplyAsync("", embed: EmbedTool.CommandError("There is not an active event or you are not authorized to make changes to the event."));

        }

        [Command("event admin openreg")]
        public async Task OpenRegistrationAsync()
        {
            if (IsEventActive() && IsEventOrganizer(Context.User as IGuildUser, Context.Guild))
            {//Is the event active?
                Event activeEvent = GetActiveEvent();
                if (activeEvent.RegistrationOpen == false)
                {//Is registration closed?
                    UpdateEventData(EventFields.RegistrationOpen, "true");
                    await ReplyAsync("Registration is now open.");
                }
                else //No changes necessary.
                    await ReplyAsync("", embed: EmbedTool.CommandError("The event's registration is already open."));
            }
            else
                await ReplyAsync("", embed: EmbedTool.CommandError("There is not an active event or you are not authorized to make changes to the event."));

        }

        [Command("event admin closereg")]
        public async Task CloseRegistrationAsync()
        {
            if (IsEventActive() && IsEventOrganizer(Context.User as IGuildUser, Context.Guild))
            {//Is the event active?
                Event activeEvent = GetActiveEvent();
                if (activeEvent.RegistrationOpen == true)
                {//Is registration closed?
                    UpdateEventData(EventFields.RegistrationOpen, "false");
                    await ReplyAsync("Registration is now closed.");
                }
                else //No changes necessary.
                    await ReplyAsync("", embed: EmbedTool.CommandError("The event's registration is already closed."));
            }
            else
                await ReplyAsync("", embed: EmbedTool.CommandError("There is not an active event or you are not authorized to make changes to the event."));

        }

        //Support Tools

        enum EventFields
        {
            Title,
            Rules,
            Description,
            StartDate,
            RegistrationOpen
        }

        /// <summary>
        /// Updates the Event JSON and serializes it
        /// </summary>
        /// <param name="targetUpdate">Field in which to update</param>
        /// <param name="UpdatedInfo">Data to replace the field with</param>
        private void UpdateEventData(EventFields targetUpdate, string UpdatedInfo)
        {
            Event targetEvent = JsonConvert.DeserializeObject<Event>(File.ReadAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Data{Path.DirectorySeparatorChar}Event.json"));

            switch (targetUpdate)
            {
                case EventFields.Title:
                    targetEvent.EventName = UpdatedInfo;
                    break;
                case EventFields.Rules:
                    targetEvent.RulesURL = UpdatedInfo;
                    break;
                case EventFields.Description:
                    targetEvent.Description = UpdatedInfo;
                    break;
                case EventFields.StartDate:
                    targetEvent.StartDate = UpdatedInfo;
                    break;
                case EventFields.RegistrationOpen:
                    if (UpdatedInfo == "true")
                        targetEvent.RegistrationOpen = true;
                    else
                        targetEvent.RegistrationOpen = false;
                    break;
                default:
                    break;
            }

            File.WriteAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Data{Path.DirectorySeparatorChar}Event.json", JsonConvert.SerializeObject(targetEvent));


        }

        /// <summary>
        /// Using LINQ, Check to see if a user is registered.
        /// </summary>
        /// <param name="user"></param>
        public static bool IsParticipantRegistered(ulong userID)
        {
            bool result = false;
            for (int i = 0; i < ParticipantList.Count; i++)
            {
                if (ParticipantList[i].UserID == userID)
                {
                    result = true;
                    Console.WriteLine("Found. Netbattler name: " + ParticipantList[i].NetbattlerName);
                }
                    
            }

            return result;
        }

        private void SyncParticipantList()
        {
            ParticipantList = JsonConvert.DeserializeObject<List<EventParticipant>>(File.ReadAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Data{Path.DirectorySeparatorChar}Registration.json"));
        }

        public static void WriteParticipantList()
        {
            File.WriteAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Data{Path.DirectorySeparatorChar}Registration.json", JsonConvert.SerializeObject(ParticipantList));

        }

        private bool IsEventActive()
        {
            if (File.Exists($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Data{Path.DirectorySeparatorChar}Event.json"))
                return true;
            else
                return false;
        }

        private bool IsEventOrganizer(IGuildUser caller, SocketGuild TargetServer)
        {
            SocketRole role = BotTools.GetRole("Event Organizer", TargetServer);
            if (caller.RoleIds.Contains(role.Id))
                return true; //The user is an event organizer.
            else
                return false; //The user is not an event organizer.
        }

        private Event GetActiveEvent()
        {
            return JsonConvert.DeserializeObject<Event>(File.ReadAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Data{Path.DirectorySeparatorChar}Event.json"));
        }

        public static EventParticipant GetParticipant(IUser user)
        {
            return ParticipantList.Find(x => x.UserID == user.Id);
        }

        public static void MarkAsSubmitted(IUser user)
        {
            EventParticipant target = GetParticipant(user);
            target.SetupSubmitted = true;
            WriteParticipantList();
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
    }
}
