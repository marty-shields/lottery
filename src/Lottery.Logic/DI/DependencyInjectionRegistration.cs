using Lottery.Core.Interfaces.Players;
using Lottery.Core.Interfaces.Prizes;
using Lottery.Core.Interfaces.Random;
using Lottery.Core.Interfaces.Tickets;
using Lottery.Logic.Players;
using Lottery.Logic.Prizes;
using Lottery.Logic.Prizes.WinnerPickers;
using Lottery.Logic.Random;
using Lottery.Logic.Tickets;
using Lottery.Logic.Tickets.TicketValidators;

using Microsoft.Extensions.DependencyInjection;

namespace Lottery.Logic.DI;

public static class DependencyInjectionRegistration
{
    public static void RegisterLogicDependencies(this IServiceCollection services)
    {
        services.AddTransient<IRandomNumberGenerator, RandomNumberGenerator>();
        services.AddSingleton<IPlayerGenerator, PlayerGenerator>();
        services.AddSingleton<IPlayerFactory, PlayerFactory>();
        services.AddSingleton<ITicketGenerator, TicketGenerator>();
        services.AddSingleton<IPrizeGenerator, PrizeGenerator>();
        services.RegisterTicketValidation();
        services.RegisterWinnerPickers();
    }

    private static void RegisterWinnerPickers(this IServiceCollection services)
    {

        services.AddSingleton<IPrizeWinnerPicker, GrandPrizeWinnerPicker>();
        services.AddSingleton<IPrizeWinnerPicker, SecondTierWinnerPicker>();
        services.AddSingleton<IPrizeWinnerPicker, ThirdTierWinnerPicker>();
        services.AddSingleton<IPrizeWinnerPickerFactory, PrizeWinnerPickerFactory>();
    }

    private static void RegisterTicketValidation(this IServiceCollection services)
    {
        services.AddSingleton<ITicketValidator, MaximumTicketCountForPlayerValidator>();
        services.AddSingleton<ITicketValidator, MinimumTicketCountForPlayerValidator>();
        services.AddSingleton<ITicketValidatorService, TicketValidatorService>();
    }
}
