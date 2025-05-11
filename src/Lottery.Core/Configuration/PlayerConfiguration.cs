using System.ComponentModel.DataAnnotations;

namespace Lottery.Core.Configuration;

public class PlayerConfiguration : IValidatableObject
{
    public const string SectionName = "Player";
    public required int MaximumPlayersAllowed { get; set; }
    public required int MinimumPlayersAllowed { get; set; }
    public required int StartingBalance { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();
        if (MaximumPlayersAllowed < MinimumPlayersAllowed)
        {
            results.Add(new ValidationResult(
                $"MaximumPlayersAllowed ({MaximumPlayersAllowed}) cannot be less than MinimumPlayersAllowed ({MinimumPlayersAllowed}).",
                [nameof(MaximumPlayersAllowed), nameof(MinimumPlayersAllowed)]));
        }

        if (MinimumPlayersAllowed < 1)
        {
            results.Add(new ValidationResult(
                $"MinimumPlayersAllowed ({MinimumPlayersAllowed}) cannot be less than 1.",
                [nameof(MinimumPlayersAllowed)]));
        }

        if (StartingBalance < 1)
        {
            results.Add(new ValidationResult(
                $"StartingBalance ({StartingBalance}) cannot be negative.",
                [nameof(StartingBalance)]));
        }
        return results;
    }
}