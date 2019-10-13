using System;
using System.Threading;
using System.Threading.Tasks;
using Alerting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IAlertChecker alertChecker;

        public Worker(ILogger<Worker> logger, IAlertChecker alertChecker)
        {
            _logger = logger;
            this.alertChecker = alertChecker;
            alertChecker.OnTemperatureTooLowAlert += AlertCheckingScheduler_OnTemperatureTooLowAlert;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker starting");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker stopping");
            return base.StopAsync(cancellationToken);
        }
        public override void Dispose()
        {
            _logger.LogInformation("Worker disposing");
            base.Dispose();
        }
        private void AlertCheckingScheduler_OnTemperatureTooLowAlert(TemperatureTooLowEventArgs args)
        {
            _logger.LogInformation($"Temperature alert raised {args}");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                alertChecker.CheckAndRaiseAlertIfNeeded(stoppingToken);
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}

