namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;

public record PlayerBalanceResult(
	bool IsPlayerExist,
	decimal? Balance);
