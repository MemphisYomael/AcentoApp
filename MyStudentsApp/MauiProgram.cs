using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyStudentsApp.DbContext;
using MyStudentsApp.MVVM.Models.SkiaControls;
using MyStudentsApp.MVVM.ViewModels;
using MyStudentsApp.Services;
using MyStudentsApp.Shared.Services;
using SkiaSharp.Views.Maui.Controls.Hosting;
using SkiaSharp.Views.Maui.Handlers;

namespace MyStudentsApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitMarkup()
                .UseSkiaSharp()
                  .ConfigureMauiHandlers(handlers =>
                  {
                      handlers.AddHandler<NotificationBadgeView, SKCanvasViewHandler>();
                  })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("Lora-Regular.ttf", "text");
                });

            // Add device-specific services used by the MyStudentsApp.Shared project\
            builder.Services.AddSingleton<ChatService>();
            builder.Services.AddSingleton<IFormFactor, FormFactor>();
            builder.Services.AddScoped<IGestionUsuarioServiceApp, GestionUsuariosServiceApp>();
            builder.Services.AddScoped<LoginViewModel>();
            builder.Services.AddScoped<CrearEstudianteViewModel>();
            builder.Services.AddScoped<ListadoDeEstudiantesViewModel>();
            builder.Services.AddScoped<CrearProfesorViewModel>();
            builder.Services.AddScoped<ChatZoneViewModel>();


            builder.Services.AddMauiBlazorWebView();


#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
