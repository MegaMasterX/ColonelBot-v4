using System;
using System.Collections.Generic;
using System.Text;

//This Model is for Battlechips for JSON lookup. 

namespace ColonelBot_v4.Models
{
    class Chip
    {
        public string Name { get; set; }        //This chip's name.
        public string Element { get; set; }     //Element of the chip.
        public string MB { get; set; }          //Megabyte value of the chip. Should contain "MB" at the end.
        public string ATK { get; set; }         //Attack value of the chip. If set to 0, the lookup code should hide this value.
        public string Codes { get; set; }       //Codes that the chip is available in.
        public string Description { get; set; } //In-game description of the chip.
        public string Image { get; set; }       //URL for the chip image (N1GP Hosting?)
        public string MoreDetails { get; set; } //The Wiki URL or guide link to obtain more infomration (UNINSTALL PLEASE?)
        public string Alias { get; set; }
    }
}
