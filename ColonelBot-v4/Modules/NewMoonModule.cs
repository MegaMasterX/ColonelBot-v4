using System;
using System.Threading;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Text;
using Discord.Commands;
using Discord;
using Discord.WebSocket;

using ColonelBot_v4.Tools;
using ColonelBot_v4.Models;

using Newtonsoft.Json;

using System.Net;

/// <summary>
/// This Module supplements the Event Module.
/// </summary>

namespace ColonelBot_v4.Modules
{
    
    public class NewMoonModule : ModuleBase<SocketCommandContext>
    {

        [Command("addmoon")]
        [RequireContext(ContextType.Guild)]
        public async Task AddMoon([Remainder] string msg)
        {
            await ReplyAsync($"<:BarylMeh:297934727682326540> Beginning to add MOON BATTLERS. This may take a moment due to Discord rate limiting.");
            var role = RoleModule.GetRole("MOON BATTLER", Context.Guild);
            foreach (var item in Context.Message.MentionedUsers)
            {
                SocketGuildUser target = Context.Guild.GetUser(item.Id);
                await target.AddRoleAsync(role);
                await ReplyAsync($"Added Moon role to: {target.Username.Replace('@', ' ')}");
            }

        }

        [Command("nomoon")]
        [RequireContext(ContextType.Guild)]
        public async Task NoMoon()
        {
            await ReplyAsync($"<:BarylMeh:297934727682326540> Removing MOON BATTLER from <<ALL>> users. This may take a moment due to Discord rate limiting.");
            var role = RoleModule.GetRole("MOON BATTLER", Context.Guild);
            foreach (var item in role.Members)
            {
                SocketGuildUser target = Context.Guild.GetUser(item.Id);
                await target.RemoveRoleAsync(role);
                await ReplyAsync($"Removed Moon role from: {target.Username.Replace('@', ' ')}");
            }

        }

        [Command("newmoon getavatars", RunMode=RunMode.Async)]
        [RequireContext(ContextType.Guild)]
        public async Task GetAllNewMoonParticipantAvatarsAsync()
        {
            if (IsEventOrganizer(Context.User as IGuildUser, Context.Guild))
            {
                //pain peko -mmx
                CleanupCache();
                await Context.Guild.DownloadUsersAsync();
                List<SocketGuildUser> users = Context.Guild.Users.ToList<SocketGuildUser>();
                List<string> MoonbattlerAvatarURLs = new List<string>();
                List<string> MoonbattlerUsernames = new List<string>();
                WebClient client = new WebClient();
                foreach (var item in users)
                {
                    if (item.Roles.Contains(RoleModule.GetRole("MOON BATTLER", Context.Guild)))
                    {
                        MoonbattlerAvatarURLs.Add(item.GetAvatarUrl());
                        MoonbattlerUsernames.Add($"{item.Username} - {item.Nickname}");
                        
                    }

                }
                await ReplyAsync("Downloading user avatars. This may take a moment.");
                for (int i = 0; i < MoonbattlerAvatarURLs.Count; i++)
                {
		     bool retry = false;
		     int counter = 3;
		     do{
                     // Try to catch all failures
			    retry = false;
			    try
			    {
				client.DownloadFile(new Uri(MoonbattlerAvatarURLs[i]), $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Cache{Path.DirectorySeparatorChar}{MoonbattlerUsernames[i]}.png");

			    }
			    catch (Exception ex)
			    {
				counter--;
				retry = true;
				await ReplyAsync($"{ex.Message} On Image {MoonbattlerAvatarURLs[i]} by user{MoonbattlerUsernames[i]}. {counter} Attempts left.");
				Thread.Sleep(1000);
				if(counter == 0) {
					retry = false;
					counter =3;
				}
			    }
		     }while(retry);
                    
                }

                await ReplyAsync("Download completed. You have email.");
                string ZIPTarget = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Cache{Path.DirectorySeparatorChar}MoonBattlerMugshots.zip";

                ZipFile.CreateFromDirectory($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Cache", $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}MoonbattlerMugshots.zip");

                await Context.User.SendFileAsync($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}MoonbattlerMugshots.zip", "");

                //string ThumbnailURL = usr.GetAvatarUrl();

            }
        }

        private void CleanupCache()
        {
            string Zipfolder = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}MoonbattlerMugshots.zip";
            string CacheDir = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Cache";
            if (File.Exists(Zipfolder))
                File.Delete(Zipfolder);
            Directory.Delete(CacheDir,true);
            Directory.CreateDirectory(CacheDir); //lol prof's gonna fuckin' kill meeeeeeee -MMX
        }



        //========================Support Functions=====================

        private bool IsEventOrganizer(IGuildUser caller, SocketGuild TargetServer)
        {
            SocketRole role = BotTools.GetRole("Event Organizer", TargetServer);
            if (caller.RoleIds.Contains(role.Id))
                return true; //The user is an event organizer.
            else
                return false; //The user is not an event organizer.
        }


        private async Task AddNewMoonRoleAsync(IGuildUser caller, SocketRole role)
        {
            if (caller.RoleIds.Contains(role.Id) == false)
                await caller.AddRoleAsync(role);
        }

  
    } 
}
