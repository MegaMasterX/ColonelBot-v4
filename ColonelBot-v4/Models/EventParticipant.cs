using System;
using System.Collections.Generic;
using System.Text;

//This Model is for users who are registering via Discord.  This should be considered "Legacy" when the Online Portal and/or Challonge
//  integration is completed. 

namespace ColonelBot_v4.Models
{
    class EventParticipant
    {
        public string NetbattlerName { get; set; }          //The user-specified Netbattler Name.
        public long UserID { get; set; }                    //Discord ID of the user.
        public string SetupLocation { get; set; }           //The on-disk folder location of the user's submitted setup.
        public bool SetupSubmitted { get; set; }            //Has the user's setup been submitted? For use with organizer notification tools.

    }
}
