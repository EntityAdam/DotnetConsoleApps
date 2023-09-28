using Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<DoWorkAndStopService>()
                .AddTransient<IKeyvaultClient, KeyvaultClient>();
    });

await host.RunConsoleAsync();


return Environment.ExitCode;