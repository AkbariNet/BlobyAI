using Microsoft.Extensions.Logging;

namespace BlobyAI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("7Awesome-Free-Solid-900.otf", "7Awesome");
                    fonts.AddFont("7Awesome-Free-Regular-400.otf", "R7Awesome");
                    fonts.AddFont("7Awesome-Brand-Regular-400.otf", "BR7Awesome");
                    fonts.AddFont("7Awesome-Brand-Regular-400.otf", "BR7Awesome");
                    fonts.AddFont("Vazirmatn-Thin.ttf", "VazirmatnThin");
                    fonts.AddFont("Vazirmatn-ExtraLight.ttf", "VazirmatnExtraLight");
                    fonts.AddFont("Vazirmatn-Light.ttf", "VazirmatnLight");
                    fonts.AddFont("Vazirmatn-Regular.ttf", "VazirmatnRegular");
                    fonts.AddFont("Vazirmatn-Medium.ttf", "VazirmatnMedium");
                    fonts.AddFont("Vazirmatn-SemiBold.ttf", "VazirmatnSemiBold");
                    fonts.AddFont("Vazirmatn-Bold.ttf", "VazirmatnBold");
                    fonts.AddFont("Vazirmatn-ExtraBold.ttf", "VazirmatnExtraBold");
                    fonts.AddFont("Vazirmatn-Black.ttf", "VazirmatnBlack");
                    fonts.AddFont("Vazirmatn.ttf", "Vazirmatn");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
