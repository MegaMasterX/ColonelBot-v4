using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ConfirmResetCommandsModal : IModal
{
    public string Title => "Reset Info Command Responses";
    [InputLabel("Are you sure you want to reset all commands?")]
    [ModalTextInput("response", placeholder: "Type yes here to confirm.", minLength: 1)]
    public string confirmation { get; set; }
}

