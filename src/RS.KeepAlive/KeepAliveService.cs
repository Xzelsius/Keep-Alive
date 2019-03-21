// Copyright (c) Raphael Strotz. All rights reserved.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace RS.KeepAlive
{
    internal class KeepAliveService : BackgroundService
    {
        public static IDictionary<string, string> SwitchMap => new Dictionary<string, string>
        {
            {"-j", "json"}
        };

        protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.CompletedTask;
    }
}
