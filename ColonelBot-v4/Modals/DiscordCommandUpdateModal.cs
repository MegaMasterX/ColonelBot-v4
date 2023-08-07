using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DiscordCommandUpdateModal : IModal
{
    public string Title => "Update ColonelBot Command";
    [InputLabel("Which command are you updating?")]
    [ModalTextInput("commandTarget", placeholder: "/sad", minLength:1)]
    public string CommandName { get; set; }

    [InputLabel("What should the response be?")]
    [ModalTextInput("commandResponse", Discord.TextInputStyle.Paragraph, placeholder: "is sad", minLength:1)]
    public string NewCommandValue { get; set; }
}

