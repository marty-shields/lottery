namespace Lottery.Core.Results;

public record class TicketValidatorServiceResult(IEnumerable<TicketValidatorResult> Results)
{
    public bool IsValid => Results.All(x => x.IsValid);
    public IEnumerable<string> Errors => Results.SelectMany(x => x.Errors) ?? [];
}
