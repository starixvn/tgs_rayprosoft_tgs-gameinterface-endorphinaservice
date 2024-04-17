using Tgs.ServiceUtility;

namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;

internal class PlayerCreateCommandService(IGameApiClient client) : ICommandService<PlayerCreateCommand>
{
	public ValueTask ExecuteAsync(PlayerCreateCommand command, CancellationToken cancellationToken = default)
		=> client.PlayerRegistrationAsync(
				command.ApiUrl,
				command.MerchantId,
				command.SecretKey,
				command.Username,
				cancellationToken);
}
