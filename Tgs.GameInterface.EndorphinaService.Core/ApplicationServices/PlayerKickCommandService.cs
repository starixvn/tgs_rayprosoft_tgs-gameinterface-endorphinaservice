using Tgs.ServiceUtility;

namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;

internal class PlayerKickCommandService : ICommandService<PlayerKickCommand>
{
    private readonly IGameApiClient _client;

    public PlayerKickCommandService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask ExecuteAsync(PlayerKickCommand command, CancellationToken cancellationToken = default)
    {
        _ = await _client
            .KickPlayerAsync(command.ApiUrl, command.MerchantId, command.SecretKey, command.Username, cancellationToken)
            .ConfigureAwait(false);

        return;
    }
}
