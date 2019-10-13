using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ConfServiceMonolithPublicApi.DI;
using System.Threading.Tasks;
using Alerting;
using System.Threading;
using System;

namespace ConfServiceMonolithPublicApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private ILogger  Logger {get;set;}

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHttpClient();
            services.AddAlerting();
            services.AddAirly();
        }

        public void Configure(IApplicationBuilder app, IHostApplicationLifetime hostApplicationLifetime, IWebHostEnvironment env, ILoggerFactory loggerFactory, IAlertChecker alertChecker)
        {
            loggerFactory.AddFile("Logs/weatherapi-{Date}.txt");
            Logger = loggerFactory.CreateLogger<Startup>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            hostApplicationLifetime.ApplicationStarted.Register(
                () => StartAlerting(alertChecker, hostApplicationLifetime.ApplicationStopping));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void StartAlerting(IAlertChecker alertChecker, CancellationToken cancellationToken)
        {
            alertChecker.OnTemperatureTooLowAlert += AlertCheckingScheduler_OnTemperatureTooLowAlert;

            new Task(() =>
            {
                Logger.LogInformation("Starting alert detection.");
                while (!cancellationToken.IsCancellationRequested)
                {
                    alertChecker.CheckAndRaiseAlertIfNeeded(cancellationToken);
                    WaitForNextCycle(cancellationToken, 360);
                }
            }, TaskCreationOptions.LongRunning).Start();
        }
        private void WaitForNextCycle(CancellationToken cancellationToken, int secondsToWait)
        {
            Logger.LogInformation($"Checking alert finished. Waiting for {secondsToWait}s.");
            try
            {
                Task.Delay(secondsToWait * 1000).Wait(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                Logger.LogDebug("Cancelling waiting for next alert cycle");
            }
        }
        private void AlertCheckingScheduler_OnTemperatureTooLowAlert(TemperatureTooLowEventArgs args)
        {
            Logger.LogInformation($"Temperature alert raised {args}");
        }
    }
}
