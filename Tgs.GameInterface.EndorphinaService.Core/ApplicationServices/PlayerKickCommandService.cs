using Tgs.ServiceUtility;

namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;

internal class PlayerKickCommandService(IGameApiClient client) : ICommandService<PlayerKickCommand>
{
	public async ValueTask ExecuteAsync(PlayerKickCommand command, CancellationToken cancellationToken = default)
	{
		_ = await client
			.KickPlayerAsync(command.ApiUrl, command.MerchantId, command.SecretKey, command.Username, cancellationToken)
			.ConfigureAwait(false);

		return;
	}
}
