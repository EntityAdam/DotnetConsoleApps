using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    public class DoWorkAndStopService : IHostedService
    {
        private readonly IKeyvaultClient keyvaultClient;
        private readonly IHostEnvironment environment;
        private readonly IHostApplicationLifetime hostApplicationLifetime;

        public DoWorkAndStopService(IKeyvaultClient service, IHostEnvironment environment, IHostApplicationLifetime hostApplicationLifetime)
        {
            this.keyvaultClient = service;
            this.environment = environment;
            this.hostApplicationLifetime = hostApplicationLifetime;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("> Starting Application");
            Console.WriteLine($"Environment: {environment.EnvironmentName}");

            await DoWork(cancellationToken);
            
            Console.WriteLine("> Completed Work: Stopping Application");
            
            hostApplicationLifetime.StopApplication();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("> Stopping Application");

            return Task.CompletedTask;
        }

        private async Task DoWork(CancellationToken cancellationToken)
        {
            Console.WriteLine("> Application is doing work");

            await keyvaultClient.FetchConnectionStringsFromKeyvault();
        }
    }
}
