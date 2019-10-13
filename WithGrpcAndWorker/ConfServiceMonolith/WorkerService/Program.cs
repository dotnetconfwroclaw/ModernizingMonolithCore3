using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirlyInterface;
using ConfServiceMonolithPublicApi.DI;
using GrpcClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddAlerting();

                    services.AddSingleton<IMeasurementsProvider, AirlyGrpcClient>();    
                });
    }
}
