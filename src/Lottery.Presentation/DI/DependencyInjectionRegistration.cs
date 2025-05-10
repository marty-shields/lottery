using Lottery.Core.Configuration;
using Lottery.Logic.DI;

using Microsoft.Extensions.DependencyInjection;

namespace Lottery.Presentation.DI;

public static class DependencyInjectionRegistration
{
    public static void RegisterRequiredDependencies(this IServiceCollection services)
    {
        services.RegisterLogicDependencies();
        services.AddSingleton<Lottery>();
    }

    public static void SetupConfiguration(this IServiceCollection services)
    {
        services.AddOptions<PlayerConfiguration>()
            .BindConfiguration(PlayerConfiguration.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddOptions<TicketConfiguration>()
            .BindConfiguration(TicketConfiguration.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}
