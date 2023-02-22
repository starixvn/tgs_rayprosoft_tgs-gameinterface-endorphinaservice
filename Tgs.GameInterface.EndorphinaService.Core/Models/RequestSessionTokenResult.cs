namespace Tgs.GameInterface.EndorphinaService.Core.Models;
public record RequestSessionTokenResult
{
    public string Username { get; init; } = default!;
    public string Provider { get; init; } = default!;
    public string Game { get; init; } = default!;
    public string Token { get; init; } = default!;
}