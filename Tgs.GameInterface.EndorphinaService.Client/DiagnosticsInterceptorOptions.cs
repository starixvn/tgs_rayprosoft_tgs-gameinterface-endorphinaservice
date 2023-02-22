using System.Diagnostics;

namespace Tgs.GameInterface.EndorphinaService.Client;

public class DiagnosticsInterceptorOptions
{
    public ActivitySource InterceptorActivitySource { get; set; } =
        new ActivitySource($"Tgs.GameInterface.EndorphinaService.Client.DiagnosticsInterceptor");
}
