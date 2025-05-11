using System;

namespace Lottery.Core.Interfaces.Prizes;

public interface IPrizeWinnerPickerFactory
{
    /// <summary>
    /// Gets the prize winner picker based on the type specified
    /// </summary>
    /// <param name="winnerPickerType">The type of winner picker to get</param>
    /// <returns>An instance of IPrizeWinnerPicker that is based on the specified type</returns>
    IPrizeWinnerPicker GetPrizeWinnerPicker(WinnerPickerType winnerPickerType);
}

public enum WinnerPickerType
{
    GrandPrize,
    SecondTier,
    ThirdTier
}