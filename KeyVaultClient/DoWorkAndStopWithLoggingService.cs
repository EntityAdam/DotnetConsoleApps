using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    public class DoWorkAndStopWithLoggingService : IHostedService
    {
        private readonly IKeyvaultClient keyvaultClient;
        private readonly IHostEnvironment environment;
        private readonly IHostApplicationLifetime hostApplicationLifetime;
        private readonly ILogger logger;
        private TelemetryClient telemetryClient;

        public DoWorkAndStopWithLoggingService(IKeyvaultClient service, IHostEnvironment environment, IHostApplicationLifetime hostApplicationLifetime, ILogger<DoWorkAndStopWithLoggingService> logger, TelemetryClient telemetryClient)
        {
            this.keyvaultClient = service;
            this.environment = environment;
            this.hostApplicationLifetime = hostApplicationLifetime;
            this.logger = logger;
            this.telemetryClient = telemetryClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (telemetryClient.StartOperation<RequestTelemetry>("operation"))
            {
                logger.LogInformation("> Starting Application");
                logger.LogInformation($"Environment: {environment.EnvironmentName}");

                await DoWork(cancellationToken);

                logger.LogInformation("> Completed Work: Stopping Application");

                await Task.Delay(5000, cancellationToken);
                await telemetryClient.FlushAsync(cancellationToken);

                hostApplicationLifetime.StopApplication();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("> Stopping Application");

            return Task.CompletedTask;
        }

        private async Task DoWork(CancellationToken cancellationToken)
        {
            logger.LogInformation("> Application is doing work");

            await keyvaultClient.FetchConnectionStringsFromKeyvault();
        }
    }
}
