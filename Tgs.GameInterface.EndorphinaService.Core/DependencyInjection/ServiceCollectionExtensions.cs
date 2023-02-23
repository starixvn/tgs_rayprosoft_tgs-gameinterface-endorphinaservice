using System.Reflection;
using Tgs.GameInterface.EndorphinaService.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEndorphinaServiceCore(
        this IServiceCollection services)
        => services
            .AddHttpClient<IGameApiClient, GameApiClient>()
            .Services
            .RegisterAllServiceUtilityTypes(
                Assembly.GetExecutingAssembly(),
                ServiceLifetime.Transient);
}