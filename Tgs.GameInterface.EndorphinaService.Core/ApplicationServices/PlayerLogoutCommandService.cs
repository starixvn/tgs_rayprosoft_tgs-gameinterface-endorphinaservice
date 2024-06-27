using Tgs.ServiceUtility;

namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;

internal class PlayerLogoutCommandService(IGameApiClient client) : ICommandService<PlayerLogoutCommand>
{
	public async ValueTask ExecuteAsync(PlayerLogoutCommand command, CancellationToken cancellationToken = default)
	{
		_ = await client
			.KickPlayerAsync(command.ApiUrl, command.MerchantId, command.SecretKey, command.Username, cancellationToken)
			.ConfigureAwait(false);

		return;
	}
}
