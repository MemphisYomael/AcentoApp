using MyStudentsApp.Services;
using MyStudentsApp.Services.Notifications;

namespace MyStudentsApp;

public partial class AppMainShell : Shell
{
    private readonly AuthorizationService _authService;
    private readonly IGestionUsuarioServiceApp _gestionService;
    private readonly INotificationNavigationService _notificationNavigationService;

    public AppMainShell(
        AuthorizationService authService,
        IGestionUsuarioServiceApp gestionService,
        INotificationNavigationService notificationNavigationService)
    {
        InitializeComponent();

        _authService = authService;
        _gestionService = gestionService;
        _notificationNavigationService = notificationNavigationService;

        // Registrar rutas para navegación programática
        RegisterRoutes();

        // Suscribirse a cambios de autenticación
        _authService.OnAuthenticationChanged += OnAuthenticationChanged;

        // Configurar visibilidad inicial
        ConfigureVisibility();

        _notificationNavigationService.MarkShellReady();
    }

    private void RegisterRoutes()
    {
        // Rutas adicionales
        Routing.RegisterRoute("chatZone", typeof(MVVM.Views.ChatZoneView));
        Routing.RegisterRoute("gestionEstudiantes", typeof(MVVM.Views.GestionEstudiantesView));
        Routing.RegisterRoute("gestionProfesores", typeof(MVVM.Views.GestionProfesoresView));
        Routing.RegisterRoute("vinculaciones", typeof(MVVM.Views.VinculacionesView));
    }

    private void OnAuthenticationChanged(object? sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            ConfigureVisibility();
        });
    }

    private void ConfigureVisibility()
    {
        // Login - Solo visible cuando NO está autenticado
        SetFlyoutItemVisibility("LoginFlyoutItem", !_authService.IsAuthenticated);

        // Dashboard - Visible solo cuando está autenticado
        SetFlyoutItemVisibility("DashboardFlyoutItem", _authService.IsAuthenticated);

        // Gestión de Usuarios
        if (_authService.IsStudent)
        {
            // Estudiantes: Solo ven listado de profesores (para chat)
            SetFlyoutItemVisibility("GestionUsuariosFlyoutItem", true);
            SetShellContentVisibility("ListadoProfesoresContent", true);

            // Ocultar todo lo relacionado con estudiantes
            SetTabVisibility("EstudiantesTab", false);
            SetTabVisibility("ProfesoresTab", true);
            SetShellContentVisibility("CrearProfesorContent", false);
            SetShellContentVisibility("GestionProfesoresContent", false);
        }
        else if (_authService.IsTeacher)
        {
            SetFlyoutItemVisibility("GestionUsuariosFlyoutItem", true);

            if (_authService.IsAdministrative)
            {
                // Administrativos: Acceso completo excepto listado de profesores
                SetTabVisibility("EstudiantesTab", true);
                SetShellContentVisibility("ListadoEstudiantesContent", true);
                SetShellContentVisibility("CrearEstudianteContent", true);
                SetShellContentVisibility("GestionEstudiantesContent", true);

                SetTabVisibility("ProfesoresTab", true);
                SetShellContentVisibility("ListadoProfesoresContent", false); // Administrativos NO ven listado de profesores para chat
                SetShellContentVisibility("CrearProfesorContent", true);
                SetShellContentVisibility("GestionProfesoresContent", true);
            }
            else
            {
                // Profesores regulares: Solo listado de estudiantes (para chat)
                SetTabVisibility("EstudiantesTab", true);
                SetShellContentVisibility("ListadoEstudiantesContent", true);
                SetShellContentVisibility("CrearEstudianteContent", false);
                SetShellContentVisibility("GestionEstudiantesContent", false);

                // Ocultar completamente la pestaña de profesores
                SetTabVisibility("ProfesoresTab", false);
            }
        }
        else
        {
            SetFlyoutItemVisibility("GestionUsuariosFlyoutItem", false);
        }

        // Configuración - Solo administrativos
        SetFlyoutItemVisibility("ConfiguracionFlyoutItem", _authService.IsAdministrative);
    }

    private void SetFlyoutItemVisibility(string name, bool isVisible)
    {
        var item = this.FindByName<FlyoutItem>(name);
        if (item != null)
        {
            item.FlyoutItemIsVisible = isVisible;
        }
    }

    private void SetTabVisibility(string name, bool isVisible)
    {
        var tab = this.FindByName<Tab>(name);
        if (tab != null)
        {
            tab.IsVisible = isVisible;
        }
    }

    private void SetShellContentVisibility(string name, bool isVisible)
    {
        var content = this.FindByName<ShellContent>(name);
        if (content != null)
        {
            content.IsVisible = isVisible;
        }
    }

    private void MenuItem_Clicked(object sender, EventArgs e)
    {
        titulo.Text = "Acento";
        titulo.FontSize = 20;
        titulo.FontFamily = "text";
        Shell.Current.FlyoutIsPresented = false;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _authService.OnAuthenticationChanged -= OnAuthenticationChanged;
    }
}
