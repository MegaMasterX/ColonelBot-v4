using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

using Discord;
using Discord.Commands;

using ColonelBot_v4.Services;
using ColonelBot_v4.Tools;

namespace ColonelBot_v4.Modules
{
    public class MemeModule : ModuleBase<SocketCommandContext>
    {
        public ImageService ImageService { get; set; } //DI handles this.

        /*
        Example of a image command:

        [Command("eguchi")]
        public async Task EguchiAsync()
        {
            //TODO: Random image selection code to obtain the filename
            var stream = await ImageService.GetEguchiAsync(<Filename selected>);
            stream.Seek(0, SeekOrigin.Begin); //must seek to the beginning of the stream
            await Context.Channel.SendFileAsync(stream, <Filename Selected>);
        }

        */

        /// <summary>
        /// Shows a sad meme.
        /// </summary>
        [Command("sad")]
        public async Task SadMeme()
        {
            await ReplyAsync("sad");
        }

    }
}
