using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace GuitarBot {

    public class Bot {
        public static Roles rolesC;
        public DiscordSocketClient client;
        public CommandService commands;
        //public DependencyMap

        private readonly string token;
        private string commandPrefix = "?";

        public static IEnumerable<SocketRole> genreRoles;

        public Bot(string _token) {
            token = _token;
            SetupBot().Wait();
        }

        private async Task SetupBot() {
            var config = new DiscordSocketConfig() {
                LogLevel = LogSeverity.Debug
            };

            client = new DiscordSocketClient(config);
            commands = new CommandService();

            await SetUpEventHandlers();
            await SetUpCommands();
            await Login();

            while (client.ConnectionState != ConnectionState.Connected) {
                await Task.Delay(100);
            }

            rolesC = new Roles(client.Guilds.First());
            await SetUpRoles();

            await Task.Delay(-1);
        }

        private async Task SetUpRoles() {
            genreRoles = new List<SocketRole>() {
                (SocketRole)rolesC.bass,
                (SocketRole)rolesC.blues,
                (SocketRole)rolesC.jazz,
                (SocketRole)rolesC.metal,
                (SocketRole)rolesC.rock
            };
        }

        private async Task SetUpCommands() {
            await commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private async Task SetUpEventHandlers() {
            client.MessageReceived += Client_MessageReceived;
            client.UserLeft += Client_UserLeft;
            client.ChannelDestroyed += Client_ChannelDestroyed;
            client.UserJoined += Client_UserJoined;
            client.Log += Client_Log;
        }

        private async Task Client_UserJoined(SocketGuildUser arg) {
            var e = arg;

            if (e.Roles.Intersect(genreRoles).Any()) {
                return;
            }

            var dmChannel = await e.CreateDMChannelAsync();
            await dmChannel.SendMessageAsync(e.Mention + ", please tell me your main instrument/genre, so I can add your correct role!");
            await dmChannel.SendMessageAsync("You can do ?genre anytime in the server to add one.");
            await dmChannel.CloseAsync();
        }

        private async Task Client_Log(LogMessage e) {
            switch (e.Severity) {
                case LogSeverity.Critical:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;

                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;

                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;

                case LogSeverity.Verbose:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;

                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }
            Console.WriteLine(e.Message);
            Console.ResetColor();
        }

        private async Task Client_ChannelDestroyed(SocketChannel arg) {
            throw new NotImplementedException();
        }

        private async Task Client_UserLeft(SocketGuildUser arg) {
            throw new NotImplementedException();
        }

        private async Task Client_MessageReceived(SocketMessage arg) {
            var e = arg as SocketUserMessage;
            int argPos = 0;

            if (e.HasStringPrefix("?", ref argPos)) {
                var context = new CommandContext(client, e);
                var result = await commands.ExecuteAsync(context, argPos);
                if (!result.IsSuccess) {
                    await e.Channel.SendMessageAsync(result.ErrorReason);
                }
            }
        }

        private async Task Login() {
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
        }
    }
}