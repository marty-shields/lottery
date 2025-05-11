using System.ComponentModel.DataAnnotations;

namespace Lottery.Core.Configuration;

public class PrizeConfiguration
{
    public const string SectionName = "Prize";
    [Range(0.01, 0.99)]
    public required decimal SecondPrizeWinnerCountPercentage { get; set; }
    [Range(0.01, 0.99)]
    public required decimal ThirdPrizeWinnerCountPercentage { get; set; }
    [Range(0.01, 0.99)]
    public required decimal GrandPrizePayoutPercentage { get; set; }
    [Range(0.01, 0.99)]
    public required decimal SecondPrizePayoutPercentage { get; set; }
    [Range(0.01, 0.99)]
    public required decimal ThirdPrizePayoutPercentage { get; set; }
}
