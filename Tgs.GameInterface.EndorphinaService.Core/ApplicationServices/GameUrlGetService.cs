using Tgs.ServiceUtility;

namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;

internal class GameUrlGetService(IGameApiClient client) : IGetService<GameUrlQuery, string?>
{
	public async ValueTask<string?> GetAsync(GameUrlQuery query, CancellationToken cancellationToken = default)
	{
		var result = await client.RequestSessionTokenAsync(
				query.ApiUrl,
				query.MerchantId,
				query.SecretKey,
				query.GameId,
				query.Currency,
				query.Username,
				cancellationToken).ConfigureAwait(false);

		var token = result.Token;

		var sign = EndorphinaHasher.ComputeHash($"{token}{query.SecretKey}");

		return $"{query.ApiUrl}icasino/sessions/launch?token={token}&sign={sign}&lang={query.Language}";
	}
}
