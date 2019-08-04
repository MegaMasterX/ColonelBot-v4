using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ColonelBot_v4.Models
{
    public class NewMoonParticipant
    {
        public string NetbattlerName { get; set; }  //User-specified Netbattler Name
        public ulong UserID { get; set; }           //User's Discord ID
        public string SetupLocation { get; set; }   //File on-disk for the user's setup

        public int CurrentCycle { get; set; }       //The Cycle the current registrant is in.

        public NewMoonParticipant(string netbattlerName, ulong discordID, int cycle)
        {
            CurrentCycle = cycle;
            NetbattlerName = netbattlerName;
            UserID = discordID;
            if (cycle == 1)
            {
                SetupLocation = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}NewMoon{Path.DirectorySeparatorChar}Cycle 1{Path.DirectorySeparatorChar}{UserID}.txt";
                File.WriteAllText(SetupLocation, "");
            }
            else
            {
                SetupLocation = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}NewMoon{Path.DirectorySeparatorChar}Cycle 2{Path.DirectorySeparatorChar}{UserID}.txt";
                File.WriteAllText(SetupLocation, "");
            }
                
        }

    }
}
