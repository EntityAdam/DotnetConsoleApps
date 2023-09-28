using Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var hostBuilder = Host.CreateApplicationBuilder(args);

hostBuilder.Services.AddHostedService<DoWorkAndStopService>()
                    .AddTransient<IKeyvaultClient, KeyvaultClient>();

hostBuilder.Build().RunAsync();

//some *light* reading: https://github.com/dotnet/runtime/issues/61634#issuecomment-1033038101





