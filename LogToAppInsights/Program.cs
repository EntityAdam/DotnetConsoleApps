using Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


var hostBuilder = Host.CreateApplicationBuilder(args);

hostBuilder.Services.AddHostedService<DoWorkAndStopWithLoggingService>()
                    .AddTransient<IKeyvaultClient, KeyvaultClient>()
                    .AddApplicationInsightsTelemetryWorkerService();

var host = hostBuilder.Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();

logger.LogDebug($"Program: LogDebug ");
logger.LogInformation("Program: LogInformation");
logger.LogWarning("Program: LogWarning");
logger.LogError("Program: LogError");
logger.LogCritical("Program: LogCritical");

await host.RunAsync();

//some light reading: https://learn.microsoft.com/en-us/azure/azure-monitor/app/console
//                    https://learn.microsoft.com/en-us/azure/azure-monitor/app/worker-service