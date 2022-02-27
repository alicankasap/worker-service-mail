using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerMail
{
    public class Worker : BackgroundService
    {
        LogHelper logHelper = new LogHelper();
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            logHelper.LogToFile("Worker service started.");
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await DoWork(cancellationToken);
            }
        }

        public async Task DoWork(CancellationToken cancellationToken)
        {
            logHelper.SendMail();
            await Task.Delay(60 * 1000, cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            logHelper.LogToFile("Worker service stopped.");
            return base.StopAsync(cancellationToken);
        }
    }
}
