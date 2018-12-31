using System;
using System.Collections.Generic;
using System.Text;

//This model is the baseline for the configured event, not necessarily any Challonge API model.

namespace ColonelBot_v4.Models
{
    public class Event
    {
        public string EventName { get; set; }       //Name of the event.
        public string Description { get; set; }     //The paragraph outlining the event details.
        public string RulesURL { get; set; }        //URL showing the event's rules, usually a pastebin.
        public string StartDate { get; set; }       //Date string for when the tournament broadcast is. 
        public bool RegistrationOpen { get; set; }  //Flag to see if the event is or isn't accepting registration.
        public bool AcceptingSetups { get; set; }   //*Flag to accept save files for direct ColonelBot tracking.
        public ulong EventOrganizer { get; set; }    //Discord ID of the event organizer.
        //N1GP Online Portal Properties
        public string PortalURL { get; set; }       //Future - N1GP Online Portal URL
        public string PortalKey { get; set; }       //API Key for the Online Portal for REST

        public Event(ulong Organizer, string eventName)
        {
            EventOrganizer = Organizer;
            EventName = eventName;
        }
    }
}



/*  AcceptingSetups - This is for allowing users to upload their save file to Discord for ColonelBot directly
 *      to pull down the setup from the save file into a directory. 
 */
