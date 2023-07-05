namespace Tgs.GameInterface.EndorphinaService.Core.Models;
public record PlayerAccountsInfoResult
{
    public decimal Balance { get; init; }
    public string Currency { get; init; } = default!;
}
