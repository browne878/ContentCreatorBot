using CCbot.Models;
using CCbot.Services;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCbot.Commands
{
    using System.Text.RegularExpressions;

    public class RconCommands : BaseCommandModule
    {
        private readonly DiscordClient Bot;
        private readonly Config Config;
        private readonly DatabaseManager DatabaseManager;
        private readonly FileService FileManager;
        private readonly ArkCommandManager ACM;
        public RconCommands(DiscordClient bot, Config config, FileService fileManager, ArkCommandManager acm, DatabaseManager databaseManager)
        {
            Bot = bot;
            Config = config;
            FileManager = fileManager;
            DatabaseManager = databaseManager;
            ACM = acm;
            Config = fileManager.GetConfig();
        }

        //Rcon Communication
        
        [Command("lookupDiscord")]
        [Description("finds owner of a steam ID")]
        public async Task LookupDiscord(CommandContext _ctx, ulong _steamId)
        {
            if (_ctx.Channel.Id != Config.DiscordOptions.CommandsChannel) return;

            string discordId = DatabaseManager.GetDiscordId(_steamId);

            if (discordId == "" || discordId == "0")
            {
                await _ctx.RespondAsync("User has not linked their account ingame.");
                return;
            }

            DiscordUser foundUser = await Bot.GetUserAsync(ulong.Parse(discordId));

            DiscordEmbedBuilder userEmbed = new DiscordEmbedBuilder
            {
                Title = foundUser.Username + "#" + foundUser.Discriminator,
                Color = DiscordColor.Red
            };

            userEmbed.WithThumbnail(foundUser.AvatarUrl);

            await _ctx.RespondAsync(userEmbed);
        }

        [Command("lookupSteam")]
        [Description("finds owner of a steam ID")]
        public async Task LookupSteam(CommandContext _ctx)
        {
            if (_ctx.Channel.Id != Config.DiscordOptions.CommandsChannel) return;

            IReadOnlyList<DiscordUser> user = _ctx.Message.MentionedUsers;

            List<string> steamId = user.Select(discordUser => DatabaseManager.GetSteamId(discordUser.Id)).ToList();

            var loopPos = 0;

            foreach (string id in steamId)
            {
                if (id == "" || id == "0")
                {
                    await _ctx.RespondAsync("User has not linked their account ingame.");
                    return;
                }

                DiscordEmbedBuilder userEmbed = new DiscordEmbedBuilder
                {
                    Title = user[loopPos].Username + "#" + user[loopPos].Discriminator,
                    Color = DiscordColor.Red,
                    Description = $"Steam ID = {id}"
                };

                userEmbed.WithThumbnail(user[loopPos].AvatarUrl);

                await _ctx.RespondAsync(userEmbed);

                loopPos++;
            }
        }
    }
}
