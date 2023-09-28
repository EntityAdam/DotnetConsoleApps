using Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var hostBuilder = Host.CreateApplicationBuilder(args);

hostBuilder.Services.AddHostedService<DoWorkAndStopService>()
                    .AddTransient<IKeyvaultClient, KeyvaultClient>();

var host = hostBuilder.Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();

logger.LogTrace("Program: LogTrace");
logger.LogDebug("Program: LogDebug");
logger.LogInformation("Program: LogInformation");
logger.LogWarning("Program: LogWarning");
logger.LogError("Program: LogError");
logger.LogCritical("Program: LogCritical");

await host.RunAsync();