using MyStudentsApp.Services.Notifications;
using MyStudentsApp.Shared.DTOShared;
using MyStudentsApp.Shared.ModelsShared;

namespace MyStudentsApp.Services
{
    /// <summary>
    /// Servicio para gestionar la autenticación y autorización de usuarios
    /// </summary>
    public class AuthorizationService
    {
        private UsuarioResponseDto? _usuarioActual;
        private ProfesorResponseDTO? _profesorActual;

        public event EventHandler? OnAuthenticationChanged;

        /// <summary>
        /// Obtiene o establece el usuario actual autenticado
        /// </summary>
        public UsuarioResponseDto? UsuarioActual
        {
            get => _usuarioActual;
            private set
            {
                _usuarioActual = value;
                OnAuthenticationChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Obtiene o establece el profesor actual (si el usuario es profesor)
        /// </summary>
        public ProfesorResponseDTO? ProfesorActual
        {
            get => _profesorActual;
            private set
            {
                _profesorActual = value;
                OnAuthenticationChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Verifica si hay un usuario autenticado
        /// </summary>
        public bool IsAuthenticated => UsuarioActual != null;

        /// <summary>
        /// Verifica si el usuario actual es un estudiante
        /// </summary>
        public bool IsStudent => UsuarioActual?.isStudent == true;

        /// <summary>
        /// Verifica si el usuario actual es un profesor
        /// </summary>
        public bool IsTeacher => (UsuarioActual?.isStudent == false && UsuarioActual?.teacherId != null) || IsDirector;

        /// <summary>
        /// Verifica si el usuario actual es un profesor con cargo administrativo
        /// </summary>
        public bool IsAdministrative => (IsTeacher && ProfesorActual?.isAdministrativo == true) || IsDirector;

        public bool IsDirector => UsuarioActual?.isStudent == false && UsuarioActual?.Profesor == null && UsuarioActual?.studentId == null && UsuarioActual?.teacherId == null;

        /// <summary>
        /// Actualiza la información del usuario autenticado
        /// </summary>
        public async Task UpdateCurrentUser(IGestionUsuarioServiceApp service)
        {
            try
            {
                var usuario = await service.GetUsuarioActual();
                UsuarioActual = usuario;

                // Si es profesor, obtener información adicional
                if (usuario != null && !usuario.isStudent && usuario.teacherId.HasValue)
                {
                    var profesor = await service.ObtenerProfesorPorIdAsync(usuario.teacherId.Value.ToString());
                    ProfesorActual = profesor;
                }
                else
                {
                    ProfesorActual = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar usuario actual: {ex.Message}");
                ClearAuthentication();
            }
        }

        /// <summary>
        /// Limpia la información de autenticación
        /// </summary>
        public void ClearAuthentication()
        {
            UsuarioActual = null;
            ProfesorActual = null;
            Preferences.Remove("token");
            Preferences.Remove("userName");
            Preferences.Remove("userId");
            Preferences.Remove("password");
            Preferences.Remove("email");
        }

        /// <summary>
        /// Cierra sesión local y sincroniza el estado con OneSignal y la API.
        /// </summary>
        public async Task LogoutAsync(
            INotificationDeviceService notificationDeviceService,
            IOneSignalMauiService oneSignalMauiService)
        {
            try
            {
                await notificationDeviceService.DesactivarDispositivoActualAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al desactivar el dispositivo: {ex.Message}");
            }

            try
            {
                await oneSignalMauiService.LogoutAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cerrar sesión en OneSignal: {ex.Message}");
            }

            ClearAuthentication();
        }

        /// <summary>
        /// Verifica si el usuario tiene acceso a una pantalla específica
        /// </summary>
        public bool CanAccessScreen(string screenName)
        {
            if (!IsAuthenticated)
            {
                return screenName == "Login";
            }

            return screenName switch
            {
                // Login solo visible cuando NO está autenticado
                "Login" => false,

                // Dashboard visible para todos los usuarios autenticados
                "Dashboard" => true,

                // Estudiantes solo pueden ver el listado de profesores (para chat)
                "ListadoDeProfesores" => IsStudent,

                // Estudiantes NO pueden ver gestión de estudiantes
                "ListadoEstudiantesProfesores" => IsTeacher,
                "CrearEstudiante" => IsAdministrative,
                "GestionEstudiantesView" => IsAdministrative,

                // Profesores NO pueden ver el listado de profesores (para chat)
                // Solo administrativos pueden gestionar profesores
                "CrearProfesor" => IsAdministrative,
                "GestionProfesoresView" => IsAdministrative,

                // Solo administrativos pueden gestionar cursos
                "CrearCurso" => IsAdministrative,
                "VinculacionesView" => IsAdministrative,

                // Tareas accesibles para todos
                "Tareas" => true,

                _ => false
            };
        }

        /// <summary>
        /// Verifica si se debe mostrar un grupo de navegación completo
        /// </summary>
        public bool CanAccessNavigationGroup(string groupName)
        {
            if (!IsAuthenticated) return false;

            return groupName switch
            {
                "Dashboard" => true,
                "GestionUsuarios" => IsTeacher,
                "Configuracion" => IsAdministrative,
                _ => false
            };
        }
    }
}
