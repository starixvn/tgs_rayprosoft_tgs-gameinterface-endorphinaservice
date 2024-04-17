namespace Tgs.GameInterface.EndorphinaService.ViewModels;

public record PlayerBalanceResultViewModel(
	bool IsPlayerExist,
	decimal? Balance);
