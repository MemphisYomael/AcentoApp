using System.Collections.ObjectModel;
using System.Windows.Input;
using MyStudentsApp.Services;
using PropertyChanged;

namespace MyStudentsApp.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class DashboardViewModel
    {
        private readonly IGestionUsuarioServiceApp _gestionUsuarioService;
        private readonly AuthorizationService _authorizationService;

        public string Titulo { get; set; } = "Inicio";
        public string Subtitulo { get; set; } = "Resumen de actividad";
        public bool IsBusy { get; set; }
        public string? ErrorMessage { get; set; }
        public int MensajesSinLeer { get; set; }
        public int TareasPendientes { get; set; }
        public int EntregasPorCalificar { get; set; }
        public int ProximosVencimientos { get; set; }
        public int EstudiantesActivos { get; set; }
        public int EntregasCalificadas { get; set; }
        public ObservableCollection<string> Alertas { get; set; } = new();
        public ICommand CargarCommand { get; }

        public DashboardViewModel(IGestionUsuarioServiceApp gestionUsuarioService, AuthorizationService authorizationService)
        {
            _gestionUsuarioService = gestionUsuarioService;
            _authorizationService = authorizationService;
            CargarCommand = new Command(async () => await LoadAsync());
        }

        public async Task LoadAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                ErrorMessage = null;
                Alertas.Clear();

                var conversaciones = await _gestionUsuarioService.ObtenerConversacionesAsync();
                MensajesSinLeer = conversaciones.Sum(c => c.unreadCount);

                if (_authorizationService.IsStudent)
                {
                    Titulo = "Mi actividad";
                    Subtitulo = "Mensajes, tareas y calificaciones";

                    var tareas = await _gestionUsuarioService.ObtenerTareasDeEstudiante();
                    var entregas = await _gestionUsuarioService.ObtenerEntregasDelEstudiante();
                    var tareasEntregadas = entregas.Select(e => e.HomeWorkId).ToHashSet();

                    TareasPendientes = tareas.Count(t => !tareasEntregadas.Contains(t.HomeWorkId));
                    EntregasCalificadas = entregas.Count(e => e.EstaCalificada);
                    EntregasPorCalificar = 0;
                    EstudiantesActivos = 0;
                    ProximosVencimientos = tareas.Count(t => t.fechaEntrega >= DateTime.Now && t.fechaEntrega <= DateTime.Now.AddDays(7));

                    foreach (var tarea in tareas.Where(t => t.fechaEntrega >= DateTime.Now).OrderBy(t => t.fechaEntrega).Take(3))
                    {
                        Alertas.Add($"{tarea.Title} vence el {tarea.fechaEntrega:g}");
                    }
                }
                else
                {
                    Titulo = "Panel del profesor";
                    Subtitulo = "Seguimiento de mensajes, tareas y entregas";

                    var tareas = await _gestionUsuarioService.ObtenerTareasDelProfesor();
                    TareasPendientes = tareas.Count;
                    ProximosVencimientos = tareas.Count(t => t.fechaEntrega >= DateTime.Now && t.fechaEntrega <= DateTime.Now.AddDays(7));
                    EstudiantesActivos = tareas
                        .SelectMany(t => t.Students ?? new List<Shared.DTOShared.EstudianteResponseDTO>())
                        .Select(e => e.estudianteId)
                        .Distinct()
                        .Count();

                    var entregasPorCalificar = 0;
                    foreach (var tarea in tareas.Take(25))
                    {
                        var entregas = await _gestionUsuarioService.ObtenerEntregasDeTarea(tarea.HomeWorkId);
                        entregasPorCalificar += entregas.Count(e => !e.EstaCalificada);
                    }
                    EntregasPorCalificar = entregasPorCalificar;
                    EntregasCalificadas = 0;

                    foreach (var tarea in tareas.OrderBy(t => t.fechaEntrega).Take(3))
                    {
                        Alertas.Add($"{tarea.Title} - {tarea.Curso?.nombre ?? "Sin curso"}");
                    }
                }

                if (!Alertas.Any())
                {
                    Alertas.Add("No hay pendientes inmediatos.");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"No se pudo cargar el resumen: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
