using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class EurandomSetupAddModal : IModal
{
    public string Title => "Add Eurandom Setup(s)";
    [InputLabel("Please provide the setup URLs to add.")]
    [ModalTextInput("setup_string", placeholder: "Feed me URLs!")]
    public string Setups { get; set; }

    
}

