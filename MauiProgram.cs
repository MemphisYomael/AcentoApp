using Acento.MVVM.Models.SKIA;
using CommunityToolkit.Maui.Markup;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using SkiaSharp.Views.Maui.Handlers;

namespace Acento
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                 .UseMauiApp<App>()
                 .UseMauiCommunityToolkitMarkup()
                 .UseSkiaSharp()
                .ConfigureMauiHandlers(handlers =>
                {
                    handlers.AddHandler<CirculoAvatar, SKCanvasViewHandler>();
                    handlers.AddHandler<AlertaLuzParpadeanteControlSkia, SKCanvasViewHandler>();
                    handlers.AddHandler<MensajeControlSkia, SKCanvasViewHandler>();
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
