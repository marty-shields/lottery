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
        var results = new List<ValidationResult>();

        if (MaximumTicketsPerPlayer < MinimumTicketsPerPlayer)
        {
            results.Add(new ValidationResult(
                $"MaximumTicketsPerPlayer ({MaximumTicketsPerPlayer}) cannot be less than MinimumTicketsPerPlayer ({MinimumTicketsPerPlayer}).",
                [nameof(MaximumTicketsPerPlayer), nameof(MinimumTicketsPerPlayer)]));
        }

        if (MinimumTicketsPerPlayer < 1)
        {
            results.Add(new ValidationResult(
                $"MinimumTicketsPerPlayer ({MinimumTicketsPerPlayer}) cannot be less than 1.",
                [nameof(MinimumTicketsPerPlayer)]));
        }

        if (TicketPrice < 1)
        {
            results.Add(new ValidationResult(
                $"TicketPrice ({TicketPrice}) cannot be negative.",
                [nameof(TicketPrice)]));
        }

        return results;
    }
}