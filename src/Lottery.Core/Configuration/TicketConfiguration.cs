using System.ComponentModel.DataAnnotations;

namespace Lottery.Core.Configuration;

public class TicketConfiguration : IValidatableObject
{
    public const string SectionName = "Ticket";
    public required int MaximumTicketsPerPlayer { get; set; }
    public required int MinimumTicketsPerPlayer { get; set; }
    public required decimal TicketPrice { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (MaximumTicketsPerPlayer < MinimumTicketsPerPlayer)
        {
            yield return new ValidationResult(
                $"MaxTicketsPerUser ({MaximumTicketsPerPlayer}) cannot be less than MinimumTicketPerUser ({MinimumTicketsPerPlayer}).",
                [nameof(MaximumTicketsPerPlayer), nameof(MinimumTicketsPerPlayer)]);
        }

        if (MinimumTicketsPerPlayer < 1)
        {
            yield return new ValidationResult(
                $"MinimumTicketPerUser ({MinimumTicketsPerPlayer}) cannot be less than 1.",
                [nameof(MinimumTicketsPerPlayer)]);
        }

        if (TicketPrice < 1)
        {
            yield return new ValidationResult(
                $"TicketPrice ({TicketPrice}) cannot be negative.",
                [nameof(TicketPrice)]);
        }
    }
}