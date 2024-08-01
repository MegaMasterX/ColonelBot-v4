using System;
using System.Collections.Generic;
using System.Text;

namespace ColonelBot_v4.Models
{
    [Serializable]
    public class PatchCard
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string MB { get; set; }
        public string PositiveAbilities { get; set; }
        public string NegativeAbilities { get; set; }
        public string MoreInformation { get; set; }
        public string Alias { get; set; }
    }
}
