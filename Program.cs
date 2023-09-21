
using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Discord.Commands;
using System.Net.Http;

namespace TemplateBot
{
    class Program
    {
        static void Main(string[] args)
            => new Program().StartAsync().GetAwaiter().GetResult();
        private CommandHandler _handler;
        private DiscordSocketClient _client;
        ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<HttpClient>()
                .BuildServiceProvider();
        }
        public async Task StartAsync()
        {

                using (ServiceProvider services = ConfigureServices())
                {
                    var client = services.GetRequiredService<DiscordSocketClient>();

                    // Tokens should be considered secret data, and never hard-coded.
                    Log("Logging in...", ConsoleColor.Green);
                    await client.LoginAsync(TokenType.Bot, "################################################");
                    Log("Starting bot...", ConsoleColor.Green);
                    await client.StartAsync();
                    await services.GetRequiredService<CommandHandler>().InitializeAsync();
                    await Task.Delay(-1);
                }
        }
        public static async Task _client_GuildAvailable(SocketGuild arg)
        {
          
                await Log(arg.Name + " Connected!", ConsoleColor.Green);
        }
        public static async Task Log(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(DateTime.Now +" : " + message, color);
            Console.ResetColor();
        }

    }
}