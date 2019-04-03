// Copyright (c) Raphael Strotz. All rights reserved.

using System;
using System.IO;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace RS.KeepAlive
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            Console.WriteLine("Keep-Alive Utility (c) Raphael Strotz");
            Console.WriteLine("-------------------------------------");

            var app = new CommandLineApplication
            {
                Name = $"dotnet {Path.GetFileName(typeof(Program).Assembly.Location)}",
                ThrowOnUnexpectedArgument = true
            };

            app.HelpOption("-h|--help");
            var urlArgument = app.Argument<string>("<URL>", "URL(s) that are called regularly", multipleValues: true).IsRequired();
            var intervalOption = app.Option<int>("-i|--interval <INTERVAL>", "Interval in minutes in which the URL(s) are called", CommandOptionType.SingleValue);

            app.OnExecute(async () => await CreateHostBuilder(urlArgument, intervalOption).RunConsoleAsync());

            try
            {
                return app.Execute(args);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"Error occurred: {e.Message}");
                Console.ResetColor();

                return 1;
            }
        }

        private static IHostBuilder CreateHostBuilder(CommandArgument<string> urlArgument, CommandOption<int> intervalOption)
            => new HostBuilder()
                .ConfigureHostConfiguration(
                    builder =>
                    {
                        builder.AddEnvironmentVariables(prefix: "KA_");
                    })
                .ConfigureAppConfiguration(
                    builder =>
                    {
                        builder.AddEnvironmentVariables(prefix: "KA_");
                    })
                .ConfigureServices(
                    services =>
                    {
                        services.AddLogging();
                        services.Configure<KeepAliveOptions>(options =>
                        {
                            options.Targets = urlArgument.ParsedValues.ToArray();

                            if (intervalOption.ParsedValue == 0) return;
                            options.Interval = intervalOption.ParsedValue;
                        });
                        services.AddHostedService<KeepAliveService>();
                    })
                .ConfigureLogging(
                    builder =>
                    {
                        builder.AddDebug();
                        builder.AddConsole(options => options.IncludeScopes = true);
                    });
    }
}
