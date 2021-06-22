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

using System.Net;

/// <summary>
/// This Module supplements the Event Module.
/// </summary>

namespace ColonelBot_v4.Modules
{
    
    public class NewMoonModule : ModuleBase<SocketCommandContext>
    {
        //NewMoon will operate logically with 2 cycles. The cycles can be opened and closed by a TO.
        //If a user registers, it should be done with one command.
        //  If the user hasn't registered for either Cycle 1 or 2 (If they have, reply with which cycle they're in)
        //      If Cycle 1 is open, the registration is placed there.
        //      Otherwise, place the registration in Cycle 2.


        private bool NewMoonEnabled = false; //NewMoon is Disabled by default.

        static List<NewMoonParticipant> Cycle1Participants = new List<NewMoonParticipant>();
        static List<NewMoonParticipant> Cycle2Participants = new List<NewMoonParticipant>();

        [Command("newmoon getavatars")]
        [RequireContext(ContextType.Guild)]
        public async Task GetAllNewMoonParticipantAvatarsAsync()
        {
            if (IsEventOrganizer(Context.User as IGuildUser, Context.Guild))
            {
                //pain peko -mmx
                CleanupCache();
                await Context.Guild.DownloadUsersAsync();
                List<SocketGuildUser> users = Context.Guild.Users.ToList<SocketGuildUser>();
                List<string> MoonbattlerAvatarURLs = new List<string>();
                List<string> MoonbattlerUsernames = new List<string>();
                WebClient client = new WebClient();
                foreach (var item in users)
                {
                    if (item.Roles.Contains(RoleModule.GetRole("MOON BATTLER", Context.Guild)))
                    {
                        MoonbattlerAvatarURLs.Add(item.GetAvatarUrl());
                        MoonbattlerUsernames.Add($"{item.Username} - {item.Nickname}");
                        
                    }

                }
                for (int i = 0; i < MoonbattlerAvatarURLs.Count; i++)
                {
                    await ReplyAsync($"Downloading {MoonbattlerAvatarURLs[i]}");
                    try
                    {
                        await client.DownloadFileTaskAsync(new Uri(MoonbattlerAvatarURLs[i]), $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Cache{Path.DirectorySeparatorChar}{MoonbattlerUsernames[i]}.png");

                    }
                    catch (Exception ex)
                    {
                        await ReplyAsync($"{ex.Message}");
                    }
                    
                }
                

                string ZIPTarget = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Cache{Path.DirectorySeparatorChar}MoonBattlerMugshots.zip";

                ZipFile.CreateFromDirectory($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Cache", $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}MoonbattlerMugshots.zip");

                await Context.User.SendFileAsync($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}MoonbattlerMugshots.zip", "");

                //string ThumbnailURL = usr.GetAvatarUrl();

            }
        }

        private void CleanupCache()
        {
            string Zipfolder = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}MoonbattlerMugshots.zip";
            string CacheDir = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Cache";
            if (File.Exists(Zipfolder))
                File.Delete(Zipfolder);
            Directory.Delete(CacheDir,true);
            Directory.CreateDirectory(CacheDir); //lol prof's gonna fuckin' kill meeeeeeee -MMX
        }

        /// <summary>
        /// Enables the NewMoon module.
        /// </summary>
        /// <returns></returns>
        [Command("newmoon open")]
        public async Task EnableNewMoonAsync()
        {
            if (IsEventOrganizer(Context.User as IGuildUser, Context.Guild))
            {

            }
        }

        /// <summary>
        /// Temporarily halts the NewMoon module.
        /// </summary>
        /// <returns></returns>
        [Command("newmoon close")]
        public async Task DisableNewMoonAsync()
        {
            if (IsEventOrganizer(Context.User as IGuildUser, Context.Guild))
            {

            }
        }

        [Command("newmoon close 1")]
        public async Task CloseCycle1Async()
        {
            dynamic EventConfiguration = JsonConvert.DeserializeObject(System.IO.File.ReadAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}NewMoon{Path.DirectorySeparatorChar}Config.json"));
            EventConfiguration.Cycle1OpenReg = false;
            File.WriteAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}NewMoon{Path.DirectorySeparatorChar}Config.json", JsonConvert.SerializeObject(EventConfiguration, Formatting.Indented));
            await ReplyAsync("Cycle 1 registration has been closed.");
        }

        //TODO: Implement Newmoon close 2, open 1, open 2. Update Save acceptance to see if Newmoon is active and implement setups accordingly. Implement registratnt listing.

        /// <summary>
        /// DMs the caller the New Moon registrations with the following spec:
        /// 
        /// 1. "[Registrant Name] (Discord Nickname)"
        /// 2. Folder 1
        /// 3. Folder 2
        /// 4. Navicust Setups
        /// 
        /// This method compiles all active registrations into a singular text file for easy parsing.
        /// </summary>
        /// <returns></returns>
        [Command("newmoon setups")]
        public async Task GetNewMoonRegistrantsAsync()
        {
            //PSUEDO:
            // obtain a list of setup files from the setups folder.
            // substring out the Discord ID for lookup
            // 
        }

        /// <summary>
        /// Registers the caller for the New Moon cycle that's currently active.
        /// </summary>
        [Command("newmoon join")]
        public async Task RegisterNewMoonAsync([Remainder]string NetbattlerName)
        {
            
            if (IsNewMoonActive == true) //Is Newmoon Active?
            {
                if (Cycle1Open) //Is the current cycle open?
                {
                    await AddNewMoonRoleAsync(Context.User as IGuildUser, RoleModule.GetRole("MOON BATTLER", Context.Guild)); //Add the MOON BATTLER role.
                    NewMoonParticipant newParticipant = new NewMoonParticipant(NetbattlerName.Replace('@', ' '), Context.User.Id, 1); //Create the Participant Entry based on the model. The Model creation automatically sets the location based on the cycle.
                    Cycle1Participants.Add(newParticipant);
                    WriteParticipantList();
                    await ReplyAsync("You have registered for NEW MOON. Your Cycle is the current one. !awoo");
                }
                else //The registration needs to go to the next upcoming cycle.
                {
                    await AddNewMoonRoleAsync(Context.User as IGuildUser, RoleModule.GetRole("MOON BATTLER", Context.Guild)); //Add the MOON BATTLER role.
                    NewMoonParticipant newParticipant = new NewMoonParticipant(NetbattlerName.Replace('@', ' '), Context.User.Id, 2); //Create the Participant Entry based on the model. The Model creation automatically sets the location based on the cycle.
                    Cycle2Participants.Add(newParticipant);
                    WriteParticipantList();
                    await ReplyAsync("You have registered for NEW MOON. Your Cycle is the upcoming one. !awoo");
                }

                SyncParticipantList();
            }
            
        }

        [Command("newmoon advance")]
        public async Task AdvanceNewmoonAsync()
        {

        }

        //========================Support Functions=====================

        private bool IsEventOrganizer(IGuildUser caller, SocketGuild TargetServer)
        {
            SocketRole role = BotTools.GetRole("Event Organizer", TargetServer);
            if (caller.RoleIds.Contains(role.Id))
                return true; //The user is an event organizer.
            else
                return false; //The user is not an event organizer.
        }

        private bool IsNewMoonActive
        {
            get
            {
                dynamic BotConfiguration = JsonConvert.DeserializeObject(System.IO.File.ReadAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}config.json"));
                if (BotConfiguration.NewMoonActive = true)
                    return true;
                else
                    return false;

            }
        }

        private bool Cycle1Open
        {
            get
            {
                dynamic EventConfiguration = JsonConvert.DeserializeObject(System.IO.File.ReadAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}NewMoon{Path.DirectorySeparatorChar}Config.json"));
                if (EventConfiguration.Cycle1OpenReg == true)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Obtains the Setup location on-disk for the specified user.
        /// </summary>
        /// <param name="DiscordID"></param>
        /// <returns></returns>
        public string GetSetupLocation(ulong DiscordID)
        {
            string result = "";
            if (ActiveCycle == 1)
            {
                for (int i = 0; i < Cycle1Participants.Count; i++)
                {
                    if (Cycle1Participants[i].UserID == DiscordID)
                        result = Cycle1Participants[i].SetupLocation;
                }
            }else
            {
                for (int i = 0; i < Cycle2Participants.Count; i++)
                {
                    if (Cycle2Participants[i].UserID == DiscordID)
                        result = Cycle2Participants[i].SetupLocation;
                }
            }
                
            return result;
        }

        public static bool IsParticipantRegistered(ulong userID, int CycleToCheck)
        {
            List<NewMoonParticipant> ParticipantList = new List<NewMoonParticipant>();

            switch (CycleToCheck)
            {
                case 1:
                    ParticipantList = Cycle1Participants;
                    break;
                case 2:
                    ParticipantList = Cycle2Participants;
                    break;
                default:
                    ParticipantList = Cycle1Participants; //Get the current active cycle
                    break;
            }

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
            Cycle1Participants = JsonConvert.DeserializeObject<List<NewMoonParticipant>>(File.ReadAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}NewMoon{Path.DirectorySeparatorChar}Cycle1Registration.json"));
            Cycle2Participants = JsonConvert.DeserializeObject<List<NewMoonParticipant>>(File.ReadAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}NewMoon{Path.DirectorySeparatorChar}Cycle2Registration.json"));
        }

        public static void WriteParticipantList()
        {
            File.WriteAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}NewMoon{Path.DirectorySeparatorChar}Cycle1Registration.json", JsonConvert.SerializeObject(Cycle1Participants, Formatting.Indented));
            File.WriteAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}NewMoon{Path.DirectorySeparatorChar}Cycle2Registration.json", JsonConvert.SerializeObject(Cycle2Participants,Formatting.Indented));

        }

        private async Task AddNewMoonRoleAsync(IGuildUser caller, SocketRole role)
        {
            if (caller.RoleIds.Contains(role.Id) == false)
                await caller.AddRoleAsync(role);
        }

        /// <summary>
        /// Fully backs up the current NewMoon configuration and setups for reversion.
        /// </summary>
        public static void BackupNewMoonConfiguration()
        {

        }

        /// <summary>
        /// Restores the configuration stored in backup, OVERWRITING THE CURRENT LIVE CONFIGURATION
        /// </summary>
        public static void RestoreNewMoonConfig()
        {
            
        }

        public int ActiveCycle
        {
            get
            {
                int result = 0;
                dynamic BotConfiguration = JsonConvert.DeserializeObject(System.IO.File.ReadAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}NewMoon{Path.DirectorySeparatorChar}Config.json"));
                if (BotConfiguration.CurrentCycleOpen = false) //The active cycle should be 2.
                    result = 2;
                else
                    result = 1; //The active cycle is 1.
                return result;
            }
        }

        //==!newmoon drop==
        //  If the current cycle is still open (Cycle Slot 1), drop. Otherwise, check to see if the caller is an event admin,
        //      otherwise, disallow the drop.

        //==!newmoon register==
        //  Check to see if the user is already in Cycle 1 or 2. If so, respond with which one they're in.
        //  If in neither,  grant them the NEW MOON role from NewMoonConfig.json

        //==!newmoon check==
        //  DM the user their currently registered cycle and their setup that's currently submitted, if any.

        //==!newmoon info==
        //  Replies with the current cycle's information.

        //==<Save File Acceptance>==

        //=================Admin Commands===================
        //==!newmoon cycle advance==
        //  Backs up the current Cycle Configuration.
        //  Adds all registrants in Cycle 1 to Cycle 2. 
        //  Sets Cycle 1 to equal the newly combined Cycle 2.
        //  Clears Cycle 2.
        //  Opens Cycle 1's registration.

        //==!newmoon cycle recall==
        //  Asks the user for confirmation with a Context configuration.
        //  If the user replies yes, replace both Cycle 1 and 2 with the current Backup, if any.
        //  If the user replies no, cancel the Recall process and replymessage with a confirmation.

        //==!newmoon cycle 1 url==
        //  Updates the URL for the setups document for Cycle 1.

        //==!newmoon cycle 2 url==
        //  Updates the URL for the setups document for Cycle 2.

        //==!newmoon cycle 1 open==
        //  Makes Registration and Event Setups available for Cycle 1.

        //==!newmoon cycle 1 close==
        //  Makes Registration and Event Setups unavailable for Cycle 1.

        //==!newmoon info==

        //TODO: Split out Registration open/close and Setup acceptance open/close in EventModule
    } 
}
