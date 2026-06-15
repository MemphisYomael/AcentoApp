using MyStudentsApp.DbContext;
using MyStudentsApp.Services.Notifications;
using System.Diagnostics;

namespace MyStudentsApp
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            try
            {
                // Capturar excepciones no manejadas
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

                InitializeComponent();

                Services = serviceProvider;

                var oneSignalService = Services.GetRequiredService<IOneSignalMauiService>();
                oneSignalService.InitializeAsync().GetAwaiter().GetResult();

                Debug.WriteLine("[APP] ✅ App inicializada correctamente");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[APP] ❌ Error en constructor: {ex.Message}");
                Debug.WriteLine($"[APP] StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public IServiceProvider Services { get; }

        private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            Debug.WriteLine($"[APP] ❌ UnobservedTaskException: {e.Exception.Message}");
            Debug.WriteLine($"[APP] StackTrace: {e.Exception.StackTrace}");
            e.SetObserved(); // Marcar como observada para evitar crash
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                Debug.WriteLine($"[APP] ❌ UnhandledException: {ex.Message}");
                Debug.WriteLine($"[APP] StackTrace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"[APP] ❌ InnerException: {ex.InnerException.Message}");
                    Debug.WriteLine($"[APP] InnerStackTrace: {ex.InnerException.StackTrace}");
                }
            }
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            try
            {
                Debug.WriteLine("[APP] 🪟 Creando ventana...");

                // Obtener AppMainShell del service provider con sus dependencias inyectadas
                var appMainShell = Services.GetRequiredService<AppMainShell>();

                Debug.WriteLine("[APP] ✅ Ventana creada correctamente");
                return new Window(appMainShell);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[APP] ❌ Error creando ventana: {ex.Message}");
                Debug.WriteLine($"[APP] StackTrace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
