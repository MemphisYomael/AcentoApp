using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;
using Microsoft.Extensions.Logging;
using MyStudentsApp.MVVM.Models.SkiaControls;
using MyStudentsApp.MVVM.ViewModels;
using MyStudentsApp.MVVM.Views;
using MyStudentsApp.MVVM.Views.Formularios;
using MyStudentsApp.Services.Notifications;
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

            // Servicios
            builder.Services.AddScoped<ChatService>();
            builder.Services.AddSingleton<IFormFactor, FormFactor>();
            builder.Services.AddScoped<IGestionUsuarioServiceApp, GestionUsuariosServiceApp>();
            builder.Services.AddSingleton<IOneSignalMauiService, OneSignalMauiService>();
            builder.Services.AddSingleton<INotificationNavigationService, NotificationNavigationService>();
            builder.Services.AddScoped<INotificationDeviceService, NotificationDeviceService>();

            // *** NUEVO: Servicio de autorización ***
            builder.Services.AddSingleton<AuthorizationService>();

            // ViewModels principales
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<CrearEstudianteViewModel>();
            builder.Services.AddTransient<ListadoDeEstudiantesViewModel>();
            builder.Services.AddTransient<CrearProfesorViewModel>();
            builder.Services.AddTransient<ChatZoneViewModel>();
            builder.Services.AddTransient<ListadoDeProfesoresViewModel>();
            builder.Services.AddTransient<DashboardViewModel>();
            builder.Services.AddTransient<ConversacionesViewModel>();
            builder.Services.AddTransient<TareasViewModel>();

            // ViewModels de gestión
            builder.Services.AddTransient<GestionProfesoresViewModel>();
            builder.Services.AddTransient<GestionEstudiantesViewModel>();
            builder.Services.AddTransient<VinculacionesViewModel>();
            builder.Services.AddTransient<CrearCursoViewModel>();



            // Views principales
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<LoginView>();
            builder.Services.AddTransient<ListadoEstudiantesProfesores>();
            builder.Services.AddTransient<ListadoDeProfesores>();
            builder.Services.AddTransient<ChatZoneView>();
            builder.Services.AddTransient<DashboardView>();
            builder.Services.AddTransient<ConversacionesView>();
            builder.Services.AddTransient<TareasView>();

            // Views de gestión
            builder.Services.AddTransient<GestionEstudiantesView>();
            builder.Services.AddTransient<GestionProfesoresView>();
            builder.Services.AddTransient<VinculacionesView>();

            // Formularios
            builder.Services.AddTransient<CrearEstudianteView>();
            builder.Services.AddTransient<CrearProfesorView>();
            builder.Services.AddTransient<CrearCursoView>();

            // *** NUEVO: Registrar AppMainShell con dependencias ***
            builder.Services.AddTransient<AppMainShell>();


            //Blazor WebView(comentado -no se usa en producción)
            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
