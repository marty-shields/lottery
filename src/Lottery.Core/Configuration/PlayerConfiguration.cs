using System.ComponentModel.DataAnnotations;

namespace Lottery.Core.Configuration;

public class PlayerConfiguration : IValidatableObject
{
    public const string SectionName = "Player";
    [Required]
    public required int MaximumPlayersAllowed { get; set; }
    [Required]
    public required int MinimumPlayersAllowed { get; set; }
    [Required]
    public required int StartingBalance { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (MaximumPlayersAllowed < MinimumPlayersAllowed)
        {
            yield return new ValidationResult(
                $"MaxPlayers ({MaximumPlayersAllowed}) cannot be less than MinPlayers ({MinimumPlayersAllowed}).",
                [nameof(MaximumPlayersAllowed), nameof(MinimumPlayersAllowed)]);
        }

        if (MinimumPlayersAllowed < 1)
        {
            yield return new ValidationResult(
                $"MinPlayers ({MinimumPlayersAllowed}) cannot be less than 1.",
                [nameof(MinimumPlayersAllowed)]);
        }

        if (StartingBalance < 1)
        {
            yield return new ValidationResult(
                $"StartingBalance ({StartingBalance}) cannot be negative.",
                [nameof(StartingBalance)]);
        }
    }
}