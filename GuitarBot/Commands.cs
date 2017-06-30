using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace GuitarBot {

    public class Commands : ModuleBase {

        [Command("Genre")]
        private async Task Genre(params string[] args) {
            var e = Context;
            if (Bot.rolesC.IsNull) {
                Bot.rolesC = new Roles(e.Guild);
            }
            var roles = Bot.rolesC;
            var user = e.User as IGuildUser;
            List<IRole> addedRoles = new List<IRole>();

            if (args.Length == 0) {
                throw new Exception("Usage: ?Genre {Rock | Jazz | Metal | Blues | Bass}");
            }

            foreach (var role in args) {
                switch (role.ToLower()) {
                    case "rock":
                        await user.AddRoleAsync(roles.rock);
                        addedRoles.Add(roles.rock);
                        break;

                    case "jazz":
                        await user.AddRoleAsync(roles.jazz);
                        addedRoles.Add(roles.jazz);
                        break;

                    case "metal":
                        await user.AddRoleAsync(roles.metal);
                        addedRoles.Add(roles.metal);
                        break;

                    case "blues":
                        await user.AddRoleAsync(roles.blues);
                        addedRoles.Add(roles.blues);
                        break;

                    case "bass":
                        await user.AddRoleAsync(roles.bass);
                        addedRoles.Add(roles.bass);
                        break;

                    default:
                        throw new Exception("Usage: ?Genre {Rock | Jazz | Metal | Blues | Bass}");
                }
            }

            await e.Guild.GetTextChannelAsync((ulong)330037628923805707).Result.SendMessageAsync(user.Mention + $" is now a {string.Join(", ", addedRoles.Select((x) => { return x.Name; }))}");
        }

        [Command("Test")]
        private async Task Test(SocketGuildUser user) {
            var e = Context;

            EmbedBuilder embd = new EmbedBuilder();
            EmbedFieldBuilder User = new EmbedFieldBuilder() {
                Name = user.Nickname ?? user.Username,
                Value = user.Username
            };
            EmbedFieldBuilder Genres = new EmbedFieldBuilder() {
                Name = "Genres",
                Value = string.Join("\n", user.Roles.Intersect(Bot.genreRoles))
            };

            embd.AddField(User);
            embd.AddField(Genres);
            embd.ThumbnailUrl = user.GetAvatarUrl();
            embd.Color = new Color(255, 15, 15);
            embd.Title = user.Username;

            embd.WithCurrentTimestamp();
            await e.Channel.SendMessageAsync("", embed: embd);
        }
    }
}