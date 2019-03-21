// Copyright (c) Raphael Strotz. All rights reserved.

using System;
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
                        builder.AddCommandLine(args, KeepAliveService.SwitchMap);
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
                        builder.AddConsole();
                    })
                .RunConsoleAsync();
        }
    }
}
