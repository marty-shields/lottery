using Lottery.Core.Interfaces.Prizes;

namespace Lottery.Logic.Prizes;

public class PrizeWinnerPickerFactory : IPrizeWinnerPickerFactory
{
    private readonly IEnumerable<IPrizeWinnerPicker> _prizeWinnerPickers;

    public PrizeWinnerPickerFactory(IEnumerable<IPrizeWinnerPicker> prizeWinnerPickers)
    {
        _prizeWinnerPickers = prizeWinnerPickers;
    }

    public IPrizeWinnerPicker GetPrizeWinnerPicker(WinnerPickerType winnerPickerType)
    {
        return _prizeWinnerPickers.First(picker => picker.Type.Equals(winnerPickerType));
    }
}
