﻿using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using ducker.Attributes;

namespace ducker.Commands.AdministrationModule
{
    public partial class AdministrationModule
    {
        /// <summary>
        /// Command to change bot activity
        /// </summary>
        /// <param name="msg">The context that the command belongs to</param>
        /// <param name="activityType">Bot activity tyre</param>
        [Command("activity"),
         Description("Change bot activity"),
         RequireAdmin]
        public async Task ActivityCommand(CommandContext msg, [RemainingText] string activityType)
        {
            if (activityType == "streaming")
            {
                await msg.Client.UpdateStatusAsync(new DiscordActivity
                {
                    ActivityType = ActivityType.Streaming,
                    Name = "with ducks |  -help",
                    StreamUrl = "https://www.twitch.tv/itakash1"
                });
                await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
                {
                    Description = "Activity changed to streaming type",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.MainEmbedColor
                });
            }
            else if (activityType == "playing")
            {
                await msg.Client.UpdateStatusAsync(new DiscordActivity
                {
                    ActivityType = ActivityType.Playing,
                    Name = "with ducks | -help"
                });
                await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
                {
                    Description = "Activity changed to playing type",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.MainEmbedColor
                });
            }
            else
            {
                await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
                {
                    Title = $"Missing argument",
                    Description = $"**Usage:** `-activity <type>`\nPossible types: `playing`, `streaming`",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                });
            }
        }
    }
}