using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

/// <summary>
/// This is not meant to tie in to the Discord Commands, but rather meant to manage persistent data used by the bot overall.
/// </summary>
namespace ColonelBot_v4.Modules
{
    public class DataManager
    {
        public Timer globalTimer;                               //A per-second timer tick.
        public Timer quoteCooldownTimer;                        //Per-second timer strictly for Quote cooldowns.

        List<QuoteCooldownEntry> ActiveQuoteCooldowns;

    }
}
