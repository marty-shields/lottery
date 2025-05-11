using Lottery.Presentation.DI;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.SetupConfiguration();
builder.Services.RegisterRequiredDependencies();
using IHost host = builder.Build();
await host.StartAsync();

Lottery.Presentation.Lottery ticketOptions = host.Services.GetRequiredService<Lottery.Presentation.Lottery>();
ticketOptions.Run();
Console.Read();