using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

/// <summary>
/// Creates a selfcontained timer that pings the caller and user when the timer elapses.
/// </summary>
namespace ColonelBot_v4.Models
{
    public class PauseTimer
    {
        private IUser pingCaller, pingTarget;
        SocketCommandContext dContext;
        Timer pTimer;

        /// <summary>
        /// Constructor: Creates a timer with the specified seconds that will ping both the caller and target when the timer expires.
        /// </summary>
        /// <param name="Seconds">Number of seconds for the timer. Will be calculated by the constructor</param>
        /// <param name="caller">The SocketGuildUser that called the command.</param>
        /// <param name="target">The SocketGuildUser that will be pinged as well.</param>
        /// <param name="Context">The SocketCommandContext containing the chat channel and other tomfoolery.</param>
        public PauseTimer (int Seconds, IUser caller, IUser target, SocketCommandContext Context)
        {
            //Specify the ping targets.
            pingCaller = caller;
            pingTarget = target;

            ConstructTimer(Seconds);
        }

        /// <summary>
        /// Constructor Overload: Called when the user calls a timer without pinging another @ user.
        /// </summary>
        /// <param name="Seconds"></param>
        /// <param name="caller"></param>
        /// <param name="Context"></param>
        public PauseTimer(int Seconds, IUser caller, SocketCommandContext Context)
        {
            pingCaller = caller;

            ConstructTimer(Seconds);
        }

        private void ConstructTimer(int seconds)
        {
            pTimer = new Timer()
            {
                Interval = seconds * 1000,
                AutoReset = false
            };
            pTimer.Elapsed += PauseTimerElapsed;
            pTimer.Start();
        }

        private async void PauseTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (pingTarget != null)
                await dContext.Channel.SendMessageAsync($":alarm_clock: Your timer has elapsed, @{pingCaller.Id}, @{pingTarget.Id}!");
            else
                await dContext.Channel.SendMessageAsync($":alarm_clock: Your timer has elapsed, @{pingCaller.Id}!");

            DisposeTimer();
        }

        /// <summary>
        /// Disposes the timer after its elapsed.
        /// </summary>
        private void DisposeTimer()
        {
            pTimer.Stop();
            pTimer.Elapsed -= PauseTimerElapsed;
            pTimer.Dispose();
            pTimer = null;
        }
    }
}
