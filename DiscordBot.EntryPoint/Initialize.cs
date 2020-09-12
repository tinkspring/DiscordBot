﻿using Discord;
using Discord.WebSocket;
using DiscordBot.BotCommands;
using DiscordBot.BotCommands.Commands.Ping;
using DiscordBot.BotCommands.Commands.Poll;
using DiscordBot.BotCommands.Commands.Purge;
using DiscordBot.DataAccess;
using DiscordBot.EntryPoint.CommandExecution;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DiscordBot.EntryPoint
{
    public class Initialize
    {
        public static void InitializeDatabase(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddDbContext<BotDbContext>(options =>
            {
                options.UseSqlite(hostContext.Configuration["ConnectionString"]);
            });
        }
        
        public static void InitializeDiscordClient(HostBuilderContext hostContext, IServiceCollection services)
        {
            var discordClient = new DiscordSocketClient();
            
            discordClient.LoginAsync(TokenType.Bot, hostContext.Configuration["DiscordApiKey"]).GetAwaiter().GetResult();
            discordClient.StartAsync().GetAwaiter().GetResult();
            
            services.AddSingleton(discordClient);

        }
        public static void RegisterServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddSingleton<ICommandHandler, CommandHandler>();

            services.AddSingleton<IPollRepository, PollRepository>();
            
            services.AddSingleton<ICommand, PingCommand>();
            services.AddSingleton<ICommand, PurgeCommand>();
            services.AddSingleton<ICommand, PollCommand>();
        }

        public static void RegisterWorker(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddHostedService<BotWorker>();
        }
    }
}