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

/// <summary>
/// This Module supplements the Event Module.
/// </summary>

namespace ColonelBot_v4.Modules
{
    class WeeklyEventModule : ModuleBase<SocketCommandContext>
    {
        //NewMoon will operate logically with 2 cycles. The cycles can be opened and closed by a TO.
        //If a user registers, it should be done with one command.
        //  If the user hasn't registered for either Cycle 1 or 2 (If they have, reply with which cycle they're in)
        //      If Cycle 1 is open, the registration is placed there.
        //      Otherwise, place the registration in Cycle 2.




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
        [Command("newmoon register")]
        public async Task RegisterNewMoonAsync([Remainder]string NetbattlerName)
        {

        }

        //==!newmoon drop==
        //  If the current cycle is still open (Cycle Slot 1), drop. Otherwise, check to see if the caller is an event admin,
        //      otherwise, disallow the drop.

        //==!newmoon register==
        //  Check to see if the user is already in Cycle 1 or 2. If so, respond with which one they're in.
        //  If in neither,  grant them the NEW MOON role from NewMoonConfig.json


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
