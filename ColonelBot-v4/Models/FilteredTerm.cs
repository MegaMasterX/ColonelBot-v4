using System;
using System.Collections.Generic;
using System.Text;


/// <summary>
/// This is meant to be a term that has a specific "warning level" and used in tandem with the FilterTool.
/// </summary>
namespace ColonelBot_v4.Models
{
    [Serializable]
    public class FilteredTerm
    {
        public string FilteredWord;
        public FilterLevel ToleranceLevel;
        public enum FilterLevel
        {
            Low,            //This term is basically placeholder.
            Warn,           //This term will place a warning in the Bot's log channel.
            High            //This term will be instantly deleted and moderators warned via ping.
        }
    }
}
