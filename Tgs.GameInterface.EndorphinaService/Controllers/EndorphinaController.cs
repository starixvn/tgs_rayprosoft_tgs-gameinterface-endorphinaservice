using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Resources;
using Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.EndorphinaService.Controllers;

[ApiController]
[Route("endor")]
public class EndorphinaController : ControllerBase
{
	/// <summary>
	/// Players the create asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://test.endorphina.network",
	///             "merchantId": "1494",
	///             "secretKey": "9BC9E97FFCD74B16B87BD6FA82C8C2AF",
	///             "username": "4ucmqplxb029"
	///         }
	/// </remarks>
	[HttpPost("player/create", Name = "PlayerCreate")]
	public ValueTask PlayerCreateAsync(
		[FromServices] ICommandService<PlayerCreateCommand> service,
		PlayerCreateCommand source)
		=> service.ExecuteAsync(
			command: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Funds the out asynchronous.(Tgs -> Gp)
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://test.endorphina.network",
	///             "merchantId": "1494",
	///             "secretKey": "9BC9E97FFCD74B16B87BD6FA82C8C2AF",
	///             "username": "4ucmqplxb029",
	///             "currency": "CNY",
	///             "refTransactionCode": "a0f49ce0-2c1d-4f50-9fc5-58b610a30bf5",
	///             "amount": 1000
	///         }
	/// </remarks>
	[HttpPost("fund/out", Name = "FundOut")]
	public ValueTask<FundResult> FundOutAsync(
		[FromServices] IGetService<FundOutQuery, FundResult> service,
		FundOutQuery source)
		=> service.GetAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Funds the in asynchronous.(GP -> Tgs)
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://test.endorphina.network",
	///             "merchantId": "1494",
	///             "secretKey": "9BC9E97FFCD74B16B87BD6FA82C8C2AF",
	///             "username": "4ucmqplxb029",
	///             "currency": "CNY",
	///             "refTransactionCode": "a0f49ce0-2c1d-4f50-9fc5-58b610a30bf5",
	///             "amount": 1000
	///         }
	/// </remarks>
	[HttpPost("fund/in", Name = "FundIn")]
	public ValueTask<FundResult> FundInAsync(
		[FromServices] IGetService<FundInQuery, FundResult> service,
		FundInQuery source)
		=> service.GetAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Funds the status asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://test.endorphina.network",
	///             "merchantId": "1494",
	///             "secretKey": "9BC9E97FFCD74B16B87BD6FA82C8C2AF",
	///             "transactionId": "1676958531344"
	///         }
	/// </remarks>
	[HttpPost("fund/Status", Name = "FundStatus")]
	public ValueTask<bool> FundStatusAsync(
		[FromServices] IGetService<FundStatusQuery, bool> service,
		FundStatusQuery source)
		=> service.GetAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Players the balance asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://test.endorphina.network",
	///             "merchantId": "1494",
	///             "secretKey": "9BC9E97FFCD74B16B87BD6FA82C8C2AF",
	///             "username": "4ucmqplxb029",
	///             "currency": "CNY"
	///         }
	/// </remarks>
	[HttpPost("player/balance", Name = "PlayerBalance")]
	public ValueTask<PlayerBalanceResult> PlayerBalanceAsync(
		[FromServices] IGetService<PlayerBalanceQuery, PlayerBalanceResult> service,
		PlayerBalanceQuery source)
		=> service.GetAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Games the URL asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://test.endorphina.network",
	///             "merchantId": "1494",
	///             "secretKey": "9BC9E97FFCD74B16B87BD6FA82C8C2AF",
	///             "gameId": "endorphina2_Rabbits",
	///             "currency": "CNY",
	///             "username": "4ucmqplxb029",
	///             "language": "zh_Hant"
	///         }
	/// </remarks>
	[HttpPost("gameurl")]
	public ValueTask<string?> GameUrlAsync(
		[FromServices] IGetService<GameUrlQuery, string?> service,
		GameUrlQuery source)
		=> service.GetAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Players the logout asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://test.endorphina.network",
	///             "merchantId": "1494",
	///             "secretKey": "9BC9E97FFCD74B16B87BD6FA82C8C2AF",
	///             "username": "4ucmqplxb029"
	///         }
	/// </remarks>
	[HttpPost("player/logout", Name = "PlayerLogout")]
	public ValueTask PlayerLogoutAsync(
		[FromServices] ICommandService<PlayerLogoutCommand> service,
		PlayerLogoutCommand source)
		=> service.ExecuteAsync(
			command: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Games the session statistics asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://test.endorphina.network",
	///             "merchantId": "1494",
	///             "secretKey": "9BC9E97FFCD74B16B87BD6FA82C8C2AF",
	///             "token": "AA8BB60F5A134264BE838CF6EBA7C8F1"
	///         }
	/// </remarks>
	[HttpPost("gameSessionStatistics")]
	public ValueTask<GameSessionStatisticsInfo> GameSessionStatisticsAsync(
		[FromServices] IGetService<GameSessionStatisticsQuery, GameSessionStatisticsInfo> service,
		GameSessionStatisticsQuery source)
		=> service.GetAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Games the session information asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://test.endorphina.network",
	///             "merchantId": "1494",
	///             "secretKey": "9BC9E97FFCD74B16B87BD6FA82C8C2AF",
	///             "token": "AA8BB60F5A134264BE838CF6EBA7C8F1"
	///         }
	/// </remarks>
	[HttpPost("gameSessionInfo")]
	public ValueTask<GameSessionInfo> GameSessionInfoAsync(
		[FromServices] IGetService<GameSessionInfoQuery, GameSessionInfo> service,
		GameSessionInfoQuery source)
		=> service.GetAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Games the history settled asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://test.endorphina.network",
	///             "merchantId": "1494",
	///             "secretKey": "9BC9E97FFCD74B16B87BD6FA82C8C2AF",
	///             "token": "AA8BB60F5A134264BE838CF6EBA7C8F1"
	///         }
	/// </remarks>
	[HttpPost("game/history/settled", Name = "GameHistorySettled")]
	public ValueTask<GameHistorySettledResult> GameHistorySettledAsync(
		[FromServices] IGetService<GameHistorySettledQuery, GameHistorySettledResult> service,
		GameHistorySettledQuery source)
		=> service.GetAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);
}
