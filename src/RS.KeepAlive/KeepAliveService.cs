// Copyright (c) Raphael Strotz. All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace RS.KeepAlive
{
    internal class KeepAliveService : BackgroundService
    {
        private const int Intermission = 1 * 1000;

        private readonly ILogger<KeepAliveService> _logger;
        private readonly KeepAliveOptions _options;

        public KeepAliveService(ILogger<KeepAliveService> logger, IOptions<KeepAliveOptions> optionsAccessor)
        {
            _logger = logger;
            _options = optionsAccessor.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_options.Targets == null || _options.Targets.Length == 0) return;

            // Initial delay (give the application some startup time)
            await Task.Delay(Intermission, stoppingToken);

            DateTime? lastExecution = null;
            while (!stoppingToken.IsCancellationRequested)
            {
                if (DateTime.Now.AddMinutes(_options.Interval * -1) < lastExecution)
                {
                    await Task.Delay(Intermission, stoppingToken);
                    continue;
                }

                var workers = _options.Targets
                    .Select(
                        async target =>
                        {
                            await Work(target, stoppingToken);
                        });

                await Task.WhenAny(
                    Task.WhenAll(workers.ToArray()),
                    Task.Delay(Timeout.Infinite, stoppingToken));

                lastExecution = DateTime.Now;
            }
        }

        private async Task Work(string url, CancellationToken stoppingToken)
        {
            using (_logger.BeginScope($"Worker for '{url}'"))
            {
                try
                {
                    var watch = Stopwatch.StartNew();
                    using (var client = new HttpClient())
                    {
                        var response = await client.GetAsync(url, stoppingToken);
                        _logger.LogInformation("Status: {0} ({1}) Duration: {2}", response.StatusCode, (int) response.StatusCode, watch.Elapsed);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error occurred");
                }
            }
        }
    }
}
