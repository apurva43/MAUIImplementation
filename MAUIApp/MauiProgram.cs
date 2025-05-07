using Microcharts.Maui;
using Microsoft.Extensions.Logging;


namespace MAUIApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMicrocharts() // Register Microcharts
            .UseSentry(options => {
                // The DSN is the only required setting.
                options.Dsn = "https://3e7b6efa6f9b8d473fadf3a164045b59@o4509281703428096.ingest.us.sentry.io/4509281705721856";
                options.Debug = true;
                options.TracesSampleRate = 1.0; // capture all performance traces
                //options.EnableUnobservedTaskExceptionTracking = true;
                //options.EnableAppDomainUnhandledExceptionCapture = true;
                //options.EnableUncaughtExceptionHandlerIntegration = true;

                // Optional: attach logs and device context
                options.AttachStacktrace = true;
                options.SendDefaultPii = true;
            })
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

