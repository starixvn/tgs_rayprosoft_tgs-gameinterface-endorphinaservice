namespace Tgs.GameInterface.EndorphinaService.Core.Models;
public record RequestSessionTokenResult
{
    public string? Username { get; init; }
    public string? Provider { get; init; }
    public string? Game { get; init; }
    public string? Token { get; init; }
}
