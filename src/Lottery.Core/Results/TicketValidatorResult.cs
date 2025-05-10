namespace Lottery.Core.Results;

public record TicketValidatorResult(bool IsValid, IEnumerable<string> Errors)
{
    public static TicketValidatorResult Valid() => new(true, Enumerable.Empty<string>());
    public static TicketValidatorResult Invalid(params string[] errors) => new(false, errors);
    public static TicketValidatorResult Invalid(IEnumerable<string> errors) => new(false, errors);
}