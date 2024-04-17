using System.Diagnostics;

namespace Tgs.GameInterface.EndorphinaService;

public class DiagnosticsInterceptorOptions
{
	public ActivitySource InterceptorActivitySource { get; set; } =
		new ActivitySource($"Tgs.GameInterface.EndorphinaService.DiagnosticsInterceptor");
}
