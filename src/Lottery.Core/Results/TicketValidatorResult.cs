namespace Lottery.Core.Results;

public record TicketValidatorResult(bool IsValid, IEnumerable<string> Errors)
{
    public static TicketValidatorResult Valid() => new(true, []);
    public static TicketValidatorResult Invalid(params string[] errors) => new(false, errors);
}