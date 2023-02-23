namespace Tgs.GameInterface.EndorphinaService.Core.Models;
public record GameSessionInfoResult
{
    public string Id { get; init; } = default!;
    public string Username { get; init; } = default!;
    public string GameName { get; init; } = default!;
    public string Currency { get; init; } = default!;
    public string AccountType { get; init; } = default!;
    public DateTimeOffset Created { get; init; }
    public DateTimeOffset Closed { get; init; }
    public string ErrorsPending { get; init; } = default!;
}
