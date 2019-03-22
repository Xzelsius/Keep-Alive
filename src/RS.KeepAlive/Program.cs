// Copyright (c) Raphael Strotz. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace RS.KeepAlive
{
    internal class Program
    {
        public static Task Main(string[] args)
        {
            Console.WriteLine("Keep-Alive Utility (c) Raphael Strotz");
            Console.WriteLine("-------------------------------------");

            if (args == null || args.Length == 0)
            {
                return PrintHelp();
            }

            return new HostBuilder()
                .ConfigureHostConfiguration(
                    builder =>
                    {
                        builder.AddEnvironmentVariables(prefix: "KA_");
                        builder.AddCommandLine(args);
                    })
                .ConfigureAppConfiguration(
                    builder =>
                    {
                        builder.AddEnvironmentVariables(prefix: "KA_");
                        builder.AddCommandLine(args, SwitchMap);
                    })
                .ConfigureServices(
                    services =>
                    {
                        services.AddLogging();
                        services.AddHostedService<KeepAliveService>();
                    })
                .ConfigureLogging(
                    builder =>
                    {
                        builder.AddDebug();
                        builder.AddConsole();
                    })
                .RunConsoleAsync();
        }

        private static IDictionary<string, string> SwitchMap => new Dictionary<string, string>
        {
            {"-u", "url"},
            {"-i", "interval"}
        };

        private static Task PrintHelp()
        {
            Console.WriteLine("");
            Console.WriteLine("Usage: dotnet RS.KeepAlive.dll [options]");
            Console.WriteLine("");
            Console.WriteLine("Options:");
            Console.WriteLine("  -u|--url        URL(s) that are called regularly");
            Console.WriteLine("                  separated with semicolon");
            Console.WriteLine("  -i|--interval   Interval in which the URL(s) are called");
            Console.WriteLine("");

            return Task.CompletedTask;
        }
    }
}
