using System;
using System.Collections.Generic;
using System.Text;
using ColonelBot_v4.Tools;
namespace ColonelBot_v4.Models
{
    public class QuoteCooldownEntry
    {
        ulong DiscordID;
        int QuotesCalled;
        int CooldownSecondsRemaining;

        /// <summary>
        /// Returns True if the user still has remaining quotes in this rolling cooldown period.
        /// </summary>
        public bool AbleToQuote()
        {
            if (QuotesCalled != BotTools.GetQuoteLimit())
            {
                return true;
            }else
                return false;
        }

        public void TickCooldownDown()
        {
            CooldownSecondsRemaining--; //The removal of this Entry is handled on the per-second tick method.
        }
    }
}
