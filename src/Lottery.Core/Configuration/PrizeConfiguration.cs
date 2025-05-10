using System.ComponentModel.DataAnnotations;

namespace Lottery.Core.Configuration;

public class PrizeConfiguration
{
    public const string SectionName = "Prize";
    [Required]
    [Range(0, 1, ErrorMessage = "SecondPrizeWinnerCountPercentage must be between 0 and 1.")]
    public required decimal SecondPrizeWinnerCountPercentage { get; set; }
    [Required]
    [Range(0, 1, ErrorMessage = "ThirdPrizeWinnerCountPercentage must be between 0 and 1.")]
    public required decimal ThirdPrizeWinnerCountPercentage { get; set; }
    [Required]
    [Range(0, 1, ErrorMessage = "GrandPrizePayoutPercentage must be between 0 and 1.")]
    public required decimal GrandPrizePayoutPercentage { get; set; }
    [Required]
    [Range(0, 1, ErrorMessage = "SecondPrizePayoutPercentage must be between 0 and 1.")]
    public required decimal SecondPrizePayoutPercentage { get; set; }
    [Required]
    [Range(0, 1, ErrorMessage = "ThirdPrizePayoutPercentage must be between 0 and 1.")]
    public required decimal ThirdPrizePayoutPercentage { get; set; }
}
