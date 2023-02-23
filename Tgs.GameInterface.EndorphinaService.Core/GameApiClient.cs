using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;
using Tgs.GameInterface.EndorphinaService.Core.Exceptions;
using Tgs.GameInterface.EndorphinaService.Core.Models;
using Tgs.GameInterface.SkywindService.Core.Models;

namespace Tgs.GameInterface.EndorphinaService.Core;

internal class GameApiClient : IGameApiClient
{
    private readonly HttpClient _client;
    private readonly ILogger<GameApiClient> _logger;

    public GameApiClient(
        HttpClient client,
        ILogger<GameApiClient> logger)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async ValueTask PlayerRegistrationAsync(Uri apiUrl, string merchantId, string secretKey, string username, CancellationToken cancellationToken = default)
    {
        var url = new Uri(apiUrl, $"/api/casino/player/{username}");

        (bool IsSuccess, HttpStatusCode StatusCode, string ResponseMessage) = await PostJsonAsync(
                url,
                merchantId,
                secretKey,
                new Dictionary<string, string>(),
                "PlayerRegistration",
                cancellationToken).ConfigureAwait(false);

        if (!IsSuccess)
        {
            _logger.LogError("LogOn:{logOn} ApiUrl:{url} StatusCode:{statusCode} Response:{responseMessage}",
               DateTime.Now.ToString("o"),
               url.ToString(),
               $"{StatusCode}({(int)StatusCode})",
               ResponseMessage);

            var errorCodeResponse = JsonSerializer.Deserialize<ErrorCodeResponse>(ResponseMessage);

            if (errorCodeResponse is null)
                throw new NetworkException(StatusCode, ResponseMessage);

            if (StatusCode is not HttpStatusCode.Conflict && errorCodeResponse.Code is not EndorphinaErrorCode.ALREADY_EXIST_EXCEPTION)
                throw new GameProviderResponseErrorException(ResponseMessage);
        }
    }

    public async ValueTask<DepositResult?> DepositAsync(Uri apiUrl, string merchantId, string secretKey, string username, string refTransactionCode, decimal amount, string currency, CancellationToken cancellationToken = default)
    {
        var url = new Uri(apiUrl, $"/api/casino/deposit/{username}");

        var parameters = new Dictionary<string, string>
        {
            ["amount"] = amount.ToString(),
            ["currency"] = currency,
        };

        (bool IsSuccess, HttpStatusCode StatusCode, string ResponseMessage) = await PostFormAsync(
            url,
            merchantId,
            secretKey,
            parameters,
            "Deposit",
            cancellationToken).ConfigureAwait(false);

        if (!IsSuccess)
        {
            _logger.LogError("LogOn:{logOn} ApiUrl:{url} StatusCode:{statusCode} Response:{responseMessage}",
               DateTime.Now.ToString("o"),
               url.ToString(),
               $"{StatusCode}({(int)StatusCode})",
               ResponseMessage);

            var errorCodeResponse = JsonSerializer.Deserialize<ErrorCodeResponse>(ResponseMessage);

            if (errorCodeResponse is null)
                throw new NetworkException(StatusCode, ResponseMessage);

            if (StatusCode is HttpStatusCode.NotFound && errorCodeResponse.Code is EndorphinaErrorCode.NOT_FOUND_EXCEPTION)
                return null;

            if (StatusCode is HttpStatusCode.Forbidden
                && (errorCodeResponse.Code is EndorphinaErrorCode.ILLEGAL_ARGUMENT_EXCEPTION
                    or EndorphinaErrorCode.INSUFFICIENT_FUNDS_EXCEPTION
                    or EndorphinaErrorCode.INVALID_CURRENCY_EXCEPTION))
                return null;
            else
                throw new GameProviderResponseErrorException(ResponseMessage);
        }

        var jsonNode = JsonNode.Parse(ResponseMessage);

        var transactionId = jsonNode!["transactionId"]!.ToString();

        var balance = decimal.Parse(jsonNode!["amount"]!.ToString());

        return new DepositResult(refTransactionCode, transactionId, balance);
    }

    public async ValueTask<WithdrawResult?> WithdrawAsync(Uri apiUrl, string merchantId, string secretKey, string username, string refTransactionCode, decimal amount, string currency, CancellationToken cancellationToken = default)
    {
        var url = new Uri(apiUrl, $"/api/casino/withdraw/{username}");

        var parameters = new Dictionary<string, string>
        {
            ["amount"] = amount.ToString(),
            ["currency"] = currency,
        };

        (bool IsSuccess, HttpStatusCode StatusCode, string ResponseMessage) = await PostFormAsync(
            url,
            merchantId,
            secretKey,
            parameters,
            "Withdraw",
            cancellationToken).ConfigureAwait(false);

        if (!IsSuccess)
        {
            _logger.LogError("LogOn:{logOn} ApiUrl:{url} StatusCode:{statusCode} Response:{responseMessage}",
               DateTime.Now.ToString("o"),
               url.ToString(),
               $"{StatusCode}({(int)StatusCode})",
               ResponseMessage);

            var errorCodeResponse = JsonSerializer.Deserialize<ErrorCodeResponse>(ResponseMessage);

            if (errorCodeResponse is null)
                throw new NetworkException(StatusCode, ResponseMessage);

            if (StatusCode is HttpStatusCode.NotFound && errorCodeResponse.Code is EndorphinaErrorCode.NOT_FOUND_EXCEPTION)
                return null;

            if (StatusCode is HttpStatusCode.Forbidden
                && (errorCodeResponse.Code is EndorphinaErrorCode.ILLEGAL_ARGUMENT_EXCEPTION
                    or EndorphinaErrorCode.INSUFFICIENT_FUNDS_EXCEPTION
                    or EndorphinaErrorCode.INVALID_CURRENCY_EXCEPTION))
                return null;
            else
                throw new GameProviderResponseErrorException(ResponseMessage);
        }

        var jsonNode = JsonNode.Parse(ResponseMessage);

        var transactionId = jsonNode!["transactionId"]!.ToString();

        var balance = decimal.Parse(jsonNode!["amount"]!.ToString());

        return new WithdrawResult(refTransactionCode, transactionId, balance);
    }

    public async ValueTask<bool> SinglePaymentAsync(Uri apiUrl, string merchantId, string secretKey, string transactionId, CancellationToken cancellationToken = default)
    {
        var url = new Uri(apiUrl, $"/api/casino/payments/{transactionId}");

        (bool IsSuccess, HttpStatusCode StatusCode, string ResponseMessage) = await GetAsync(
            url,
            merchantId,
            secretKey,
            new Dictionary<string, string>(),
            "SinglePayment",
            cancellationToken).ConfigureAwait(false);

        if (!IsSuccess)
        {
            _logger.LogError("LogOn:{logOn} ApiUrl:{url} StatusCode:{statusCode} Response:{responseMessage}",
               DateTime.Now.ToString("o"),
               url.ToString(),
               $"{StatusCode}({(int)StatusCode})",
               ResponseMessage);

            var errorCodeResponse = JsonSerializer.Deserialize<ErrorCodeResponse>(ResponseMessage);

            if (errorCodeResponse is null)
                throw new NetworkException(StatusCode, ResponseMessage);

            if (StatusCode is HttpStatusCode.NotFound && errorCodeResponse.Code is EndorphinaErrorCode.NOT_FOUND_EXCEPTION)
                return false;
            else
                throw new GameProviderResponseErrorException(ResponseMessage);
        }

        return true;
    }

    public async ValueTask<PlayerBalanceResult> PlayerAccountsInfoAsync(Uri apiUrl, string merchantId, string secretKey, string username, string currency, CancellationToken cancellationToken = default)
    {
        var url = new Uri(apiUrl, $"/api/casino/player/{username}/accounts");

        (bool IsSuccess, HttpStatusCode StatusCode, string ResponseMessage) = await GetAsync(
            url,
            merchantId,
            secretKey,
            new Dictionary<string, string>(),
            "PlayerAccountsInfo",
            cancellationToken).ConfigureAwait(false);

        if (!IsSuccess)
        {
            _logger.LogError("LogOn:{logOn} ApiUrl:{url} StatusCode:{statusCode} Response:{responseMessage}",
               DateTime.Now.ToString("o"),
               url.ToString(),
               $"{StatusCode}({(int)StatusCode})",
               ResponseMessage);

            var errorCodeResponse = JsonSerializer.Deserialize<ErrorCodeResponse>(ResponseMessage);

            if (errorCodeResponse is null)
                throw new NetworkException(StatusCode, ResponseMessage);

            if (StatusCode is HttpStatusCode.NotFound && errorCodeResponse.Code is EndorphinaErrorCode.NOT_FOUND_EXCEPTION)
                return new PlayerBalanceResult(IsPlayerExist: false, Balance: null);
        }

        var jsonNode = JsonNode.Parse(ResponseMessage)!;

        var accountsInfo = jsonNode.AsArray();

        if (accountsInfo is null || !accountsInfo.Any())
            return new PlayerBalanceResult(IsPlayerExist: true, Balance: null);

        var results = accountsInfo.Select(x => new PlayerAccountsInfoResult()
        {
            Balance = decimal.Parse(x!["balance"]!.ToString()),
            Currency = x!["currency"]!.ToString()
        }) ?? Enumerable.Empty<PlayerAccountsInfoResult>();

        if (!results.Any())
            return new PlayerBalanceResult(IsPlayerExist: true, Balance: null);

        var accountInfo = results.FirstOrDefault(x => x.Currency == currency);

        if (accountInfo == null)
            return new PlayerBalanceResult(IsPlayerExist: true, Balance: null);

        return new PlayerBalanceResult(true, accountInfo.Balance);
    }

    public async ValueTask<RequestSessionTokenResult> RequestSessionTokenAsync(Uri apiUrl, string merchantId, string secretKey, string gameId, string currency, string username, CancellationToken cancellationToken = default)
    {
        var url = new Uri(apiUrl, $"/api/casino/launch/{username}");

        var parameters = new Dictionary<string, string>
        {
            ["provider"] = "ENDORPHINA",
            ["game"] = gameId,
            ["currency"] = currency,
        };

        (bool IsSuccess, HttpStatusCode StatusCode, string ResponseMessage) = await PostFormAsync(
            url,
            merchantId,
            secretKey,
            parameters,
            "RequestSessionToken",
            cancellationToken).ConfigureAwait(false);

        if (!IsSuccess)
        {
            _logger.LogError("LogOn:{logOn} ApiUrl:{url} StatusCode:{statusCode} Response:{responseMessage}",
               DateTime.Now.ToString("o"),
               url.ToString(),
               $"{StatusCode}({(int)StatusCode})",
               ResponseMessage);

            var errorCodeResponse = JsonSerializer.Deserialize<ErrorCodeResponse>(ResponseMessage);

            if (errorCodeResponse is null)
                throw new NetworkException(StatusCode, ResponseMessage);

            if (StatusCode is HttpStatusCode.NotFound && errorCodeResponse.Code is EndorphinaErrorCode.NOT_FOUND_EXCEPTION)
                return new RequestSessionTokenResult
                {
                    Username = null,
                    Provider = null,
                    Game = null,
                    Token = null,
                };

            if (StatusCode is HttpStatusCode.Forbidden && errorCodeResponse.Code is EndorphinaErrorCode.ILLEGAL_ARGUMENT_EXCEPTION)
                return new RequestSessionTokenResult
                {
                    Username = null,
                    Provider = null,
                    Game = null,
                    Token = null,
                };

            throw new GameProviderResponseErrorException(ResponseMessage);
        }

        var jsonNode = JsonNode.Parse(ResponseMessage);

        var token = jsonNode!["token"]!.ToString();
        var provider = jsonNode!["provider"]!.ToString();
        var game = jsonNode!["game"]!.ToString();

        return new RequestSessionTokenResult
        {
            Username = username,
            Provider = provider,
            Game = game,
            Token = token,
        };
    }

    public async ValueTask<bool> KickPlayerAsync(Uri apiUrl, string merchantId, string secretKey, string username, CancellationToken cancellationToken = default)
    {
        var url = new Uri(apiUrl, $"/api/casino/kick/{username}");

        (bool IsSuccess, HttpStatusCode StatusCode, string ResponseMessage) = await PostJsonAsync(
            url,
            merchantId,
            secretKey,
            new Dictionary<string, string>(),
            "KickPlayer",
            cancellationToken).ConfigureAwait(false);

        if (!IsSuccess)
        {
            _logger.LogError("LogOn:{logOn} ApiUrl:{url} StatusCode:{statusCode} Response:{responseMessage}",
               DateTime.Now.ToString("o"),
               url.ToString(),
               $"{StatusCode}({(int)StatusCode})",
               ResponseMessage);

            var errorCodeResponse = JsonSerializer.Deserialize<ErrorCodeResponse>(ResponseMessage);

            if (errorCodeResponse is null)
                throw new NetworkException(StatusCode, ResponseMessage);

            if (errorCodeResponse.Code is not EndorphinaErrorCode.NOT_FOUND_EXCEPTION)
                throw new GameProviderResponseErrorException(ResponseMessage);
        }

        var jsonNode = JsonNode.Parse(ResponseMessage);

        return jsonNode!["active"]!.GetValue<bool>();
    }

    public async ValueTask<GameSessionStatisticsResult> GameSessionStatisticsAsync(Uri apiUrl, string merchantId, string secretKey, string token, CancellationToken cancellationToken = default)
    {
        var url = new Uri(apiUrl, $"/api/casino/stats/{token}");

        (bool IsSuccess, HttpStatusCode StatusCode, string ResponseMessage) = await GetAsync(
            url,
            merchantId,
            secretKey,
            new Dictionary<string, string>(),
            "GameSessionStatistics",
            cancellationToken).ConfigureAwait(false);

        if (!IsSuccess)
        {
            _logger.LogError("LogOn:{logOn} ApiUrl:{url} StatusCode:{statusCode} Response:{responseMessage}",
               DateTime.Now.ToString("o"),
               url.ToString(),
               $"{StatusCode}({(int)StatusCode})",
               ResponseMessage);

            var errorCodeResponse = JsonSerializer.Deserialize<ErrorCodeResponse>(ResponseMessage);

            if (errorCodeResponse is null)
                throw new NetworkException(StatusCode, ResponseMessage);

            throw new GameProviderResponseErrorException(ResponseMessage);
        }

        var jsonNode = JsonNode.Parse(ResponseMessage);

        return new GameSessionStatisticsResult
        {
            Bet = decimal.Parse(jsonNode!["bet"]!.ToString()),
            Win = decimal.Parse(jsonNode!["win"]!.ToString()),
            Games = jsonNode!["games"]!.GetValue<int>(),
            JackpotWin = decimal.Parse(jsonNode!["jackpotWin"]!.ToString()),
            Jackpots = jsonNode!["jackpots"]!.GetValue<int>(),
            Deposit = decimal.Parse(jsonNode!["deposit"]!.ToString()),
            Withdraw = decimal.Parse(jsonNode!["withdraw"]!.ToString()),
            ErrorsPending = jsonNode!["errorsPending"]!.GetValue<int>()
        };
    }

    public async ValueTask<GameSessionInfoResult> GameSessionInfoAsync(Uri apiUrl, string merchantId, string secretKey, string token, CancellationToken cancellationToken = default)
    {
        var url = new Uri(apiUrl, $"/api/casino/sessions/{token}");

        (bool IsSuccess, HttpStatusCode StatusCode, string ResponseMessage) = await GetAsync(
            url,
            merchantId,
            secretKey,
            new Dictionary<string, string>(),
            "GameSessionInfo",
            cancellationToken).ConfigureAwait(false);

        if (!IsSuccess)
        {
            _logger.LogError("LogOn:{logOn} ApiUrl:{url} StatusCode:{statusCode} Response:{responseMessage}",
               DateTime.Now.ToString("o"),
               url.ToString(),
               $"{StatusCode}({(int)StatusCode})",
               ResponseMessage);

            var errorCodeResponse = JsonSerializer.Deserialize<ErrorCodeResponse>(ResponseMessage);

            if (errorCodeResponse is null)
                throw new NetworkException(StatusCode, ResponseMessage);

            throw new GameProviderResponseErrorException(ResponseMessage);
        }

        var jsonNode = JsonNode.Parse(ResponseMessage);

        var jsonGame = jsonNode!["game"]!.AsObject();
        var gameName = jsonGame!["name"]!.ToString();

        return new GameSessionInfoResult
        {
            Id = jsonNode!["id"]!.ToString(),
            Username = jsonNode!["username"]!.ToString(),
            GameName = gameName,
            Currency = jsonNode!["currency"]!.ToString(),
            AccountType = jsonNode!["accountType"]!.ToString(),
            Created = DateTimeOffset.Parse(jsonNode!["created"]!.ToString()),
            Closed = DateTimeOffset.Parse(jsonNode!["closed"]!.ToString()),
            ErrorsPending = jsonNode!["errorsPending"]!.ToString()
        };
    }

    private async ValueTask<(bool IsSuccess, HttpStatusCode StatusCode, string ResponseMessage)> GetAsync(Uri apiUrl, string merchantId, string secretKey, IEnumerable<KeyValuePair<string, string>> parameters, string logPrefix, CancellationToken cancellationToken = default)
    {
        logPrefix = $"{nameof(GameApiClient)}.{logPrefix}".ToLower();

        var sw = Stopwatch.StartNew();

        var transactionId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var requestTime = DateTime.UtcNow.ToString("HHmmssddMMyyyy");

        var signature = EndorphinaHasher.ComputeHash($"{merchantId}{transactionId}{requestTime}{secretKey}");

        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

        var content = new StringContent(JsonSerializer.Serialize(parameters), Encoding.UTF8, MediaTypeNames.Application.Json);

        HttpResponseMessage? httpResponseMessage;

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, apiUrl)
        {
            Content = content
        };

        requestMessage.Headers.Add("X-Casino-Merchant-Id", merchantId);
        requestMessage.Headers.Add("X-Casino-Transaction-Id", transactionId.ToString());
        requestMessage.Headers.Add("X-Casino-Timestamp", requestTime);
        requestMessage.Headers.Add("X-Casino-Signature", signature);

        httpResponseMessage = await _client.SendAsync(requestMessage, cancellationToken);

        sw.Stop();

        var responseMessage = default(string);

        using (var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result))
        {
            responseMessage = await sr.ReadToEndAsync().ConfigureAwait(false) ?? string.Empty;
        }

        var apiLogEvent = new GameApiLogEvent
        {
            Path = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
            Method = httpResponseMessage.RequestMessage?.Method.ToString().NoLongerThan(8) ?? string.Empty,
            Prefix = logPrefix,
            ServerName = Environment.MachineName.NoLongerThan(128),
            RequestHeaders = httpResponseMessage.RequestMessage?.Headers + httpResponseMessage.RequestMessage?.Content?.Headers?.ToString(),
            RequestContent = (await content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)).NoLongerThan(4096),
            ResponseHeaders = httpResponseMessage.Headers.ToString(),
            ResponseContent = responseMessage.NoLongerThan(4096),
            ResponseCode = (short)httpResponseMessage.StatusCode,
            ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
            RecordedOn = DateTimeOffset.UtcNow
        };

        _logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

        return (httpResponseMessage.IsSuccessStatusCode, httpResponseMessage.StatusCode, responseMessage);
    }

    private async ValueTask<(bool IsSuccess, HttpStatusCode StatusCode, string ResponseMessage)> PostJsonAsync(Uri apiUrl, string merchantId, string secretKey, IEnumerable<KeyValuePair<string, string>> parameters, string logPrefix, CancellationToken cancellationToken = default)
    {
        logPrefix = $"{nameof(GameApiClient)}.{logPrefix}".ToLower();

        var sw = Stopwatch.StartNew();

        var transactionId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var requestTime = DateTime.UtcNow.ToString("HHmmssddMMyyyy");

        var signature = EndorphinaHasher.ComputeHash($"{merchantId}{transactionId}{requestTime}{secretKey}");

        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

        var content = new StringContent(JsonSerializer.Serialize(parameters), Encoding.UTF8, MediaTypeNames.Application.Json);

        content.Headers.Add("X-Casino-Merchant-Id", merchantId);
        content.Headers.Add("X-Casino-Transaction-Id", transactionId.ToString());
        content.Headers.Add("X-Casino-Timestamp", requestTime);
        content.Headers.Add("X-Casino-Signature", signature);

        var httpResponseMessage = await _client.PostAsync(apiUrl, content, cancellationToken);

        sw.Stop();

        var responseMessage = default(string);

        using (var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result))
        {
            responseMessage = await sr.ReadToEndAsync().ConfigureAwait(false) ?? string.Empty;
        }

        var apiLogEvent = new GameApiLogEvent
        {
            Path = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
            Method = httpResponseMessage.RequestMessage?.Method.ToString().NoLongerThan(8) ?? string.Empty,
            Prefix = logPrefix,
            ServerName = Environment.MachineName.NoLongerThan(128),
            RequestHeaders = httpResponseMessage.RequestMessage?.Headers + httpResponseMessage.RequestMessage?.Content?.Headers?.ToString(),
            RequestContent = (await content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)).NoLongerThan(4096),
            ResponseHeaders = httpResponseMessage.Headers.ToString(),
            ResponseContent = responseMessage.NoLongerThan(4096),
            ResponseCode = (short)httpResponseMessage.StatusCode,
            ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
            RecordedOn = DateTimeOffset.UtcNow
        };

        _logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

        return (httpResponseMessage.IsSuccessStatusCode, httpResponseMessage.StatusCode, responseMessage);
    }

    private async ValueTask<(bool IsSuccess, HttpStatusCode StatusCode, string ResponseMessage)> PostFormAsync(Uri apiUrl, string merchantId, string secretKey, IEnumerable<KeyValuePair<string, string>> parameters, string logPrefix, CancellationToken cancellationToken = default)
    {
        logPrefix = $"{nameof(GameApiClient)}.{logPrefix}".ToLower();

        var sw = Stopwatch.StartNew();

        var transactionId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var requestTime = DateTime.UtcNow.ToString("HHmmssddMMyyyy");

        var signature = EndorphinaHasher.ComputeHash($"{merchantId}{transactionId}{requestTime}{secretKey}");

        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

        var content = new FormUrlEncodedContent(parameters);

        content.Headers.Add("X-Casino-Merchant-Id", merchantId);
        content.Headers.Add("X-Casino-Transaction-Id", transactionId.ToString());
        content.Headers.Add("X-Casino-Timestamp", requestTime);
        content.Headers.Add("X-Casino-Signature", signature);

        var httpResponseMessage = await _client.PostAsync(apiUrl, content, cancellationToken);

        sw.Stop();

        var responseMessage = default(string);

        using (var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result))
        {
            responseMessage = await sr.ReadToEndAsync().ConfigureAwait(false) ?? string.Empty;
        }

        var apiLogEvent = new GameApiLogEvent
        {
            Path = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
            Method = httpResponseMessage.RequestMessage?.Method.ToString().NoLongerThan(8) ?? string.Empty,
            Prefix = logPrefix,
            ServerName = Environment.MachineName.NoLongerThan(128),
            RequestHeaders = httpResponseMessage.RequestMessage?.Headers + httpResponseMessage.RequestMessage?.Content?.Headers?.ToString(),
            RequestContent = (await content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)).NoLongerThan(4096),
            ResponseHeaders = httpResponseMessage.Headers.ToString(),
            ResponseContent = responseMessage.NoLongerThan(4096),
            ResponseCode = (short)httpResponseMessage.StatusCode,
            ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
            RecordedOn = DateTimeOffset.UtcNow
        };

        _logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

        return (httpResponseMessage.IsSuccessStatusCode, httpResponseMessage.StatusCode, responseMessage);
    }
}
