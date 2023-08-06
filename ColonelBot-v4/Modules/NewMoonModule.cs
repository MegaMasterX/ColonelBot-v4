﻿using System;
using System.Threading;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Text;
using Discord.Interactions;
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
    [Group("newmoon-admin", "Commands in association with MOON events."), EventOrganizerEnabled]
    public class NewMoonModule : InteractionModuleBase<SocketInteractionContext>
    {

        [MessageCommand("addmoon"), RequireContext(ContextType.Guild), EventOrganizerEnabled]
        public async Task AddMoon(IMessage msg)
        {
            
            await RespondAsync($"<:BarylMeh:297934727682326540> Beginning to add MOON BATTLERS. This may take a moment due to Discord rate limiting.");
            var role = RoleModule.GetRole("MOON BATTLER", Context.Guild);
                
            foreach (var item in msg.MentionedUserIds)
            {
                SocketGuildUser target = Context.Guild.GetUser(item);
                await target.AddRoleAsync(role);
                await RespondAsync($"Added Moon role to: {target.Username.Replace('@', ' ')}");
            }
            

        }

        [SlashCommand("nomoon", "Event Command. Removes Moon Battler from all current Moon Users."), RequireContext(ContextType.Guild), EventOrganizerEnabled]
        [RequireContext(ContextType.Guild)]
        public async Task NoMoon()
        {
            if (IsEventOrganizer(Context.User as IGuildUser, Context.Guild))
            {

                await ReplyAsync($"<:BarylMeh:297934727682326540> Removing MOON BATTLER from <<ALL>> users. This may take a moment due to Discord rate limiting.");
                var role = RoleModule.GetRole("MOON BATTLER", Context.Guild);
                foreach (var item in role.Members)
                {
                    SocketGuildUser target = Context.Guild.GetUser(item.Id);
                    await target.RemoveRoleAsync(role);
                    await RespondAsync($"Removed Moon role from: {target.Username.Replace('@', ' ')}");
                }
            }
            else
                await RespondAsync($"You are not authorized to eradicate all of the MOON BATTLERS.");

        }

        [SlashCommand("getavatars", "Event Organizers Only. Obtains all user avatars."), EventOrganizerEnabled]
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
                await RespondAsync("Downloading user avatars. This may take a moment.");
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
				await RespondAsync($"{ex.Message} On Image {MoonbattlerAvatarURLs[i]} by user{MoonbattlerUsernames[i]}. {counter} Attempts left.");
				Thread.Sleep(1000);
				if(counter == 0) {
					retry = false;
					counter =3;
				}
			    }
		     }while(retry);
                    
                }

                await RespondAsync("Download completed. You have email.");
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
            return true; //We're returning here because the Attribute will handle the permission checks.
        }


        private async Task AddNewMoonRoleAsync(IGuildUser caller, SocketRole role)
        {
            if (caller.RoleIds.Contains(role.Id) == false)
                await caller.AddRoleAsync(role);
        }

  
    } 
}
