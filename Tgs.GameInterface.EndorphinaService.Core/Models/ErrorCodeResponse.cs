using System.Text.Json.Serialization;

namespace Tgs.GameInterface.EndorphinaService.Core.Models;

internal class ErrorCodeResponse
{
	[JsonPropertyName("code")]
	public string Code { get; set; } = default!;

	[JsonPropertyName("message")]
	public string Message { get; set; } = default!;
}
