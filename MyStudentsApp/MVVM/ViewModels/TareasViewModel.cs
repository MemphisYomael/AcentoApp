using System.Collections.ObjectModel;
using System.Windows.Input;
using MyStudentsApp.Services;
using MyStudentsApp.Shared.DTOShared;
using PropertyChanged;

namespace MyStudentsApp.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class SelectableEstudianteItem
    {
        public EstudianteResponseDTO Estudiante { get; set; } = new();
        public bool IsSelected { get; set; } = true;
        public string Nombre => $"{Estudiante.nombres} {Estudiante.apellidos}".Trim();
        public string Curso => Estudiante.curso ?? "Sin curso";
    }

    [AddINotifyPropertyChangedInterface]
    public class TareasViewModel
    {
        private readonly IGestionUsuarioServiceApp _gestionUsuarioService;
        private readonly AuthorizationService _authorizationService;

        public bool IsBusy { get; set; }
        public string? StatusMessage { get; set; }
        public bool IsStudent => _authorizationService.IsStudent;
        public bool IsTeacher => !_authorizationService.IsStudent;

        public ObservableCollection<TareaResponseDTO> Tareas { get; set; } = new();
        public ObservableCollection<CursoResponseDTO> Cursos { get; set; } = new();
        public ObservableCollection<SelectableEstudianteItem> EstudiantesCurso { get; set; } = new();
        public ObservableCollection<EntregaTareaResponseDTO> EntregasTarea { get; set; } = new();
        public ObservableCollection<EntregaTareaResponseDTO> EntregasEstudiante { get; set; } = new();

        public CursoResponseDTO? CursoSeleccionado { get; set; }
        public TareaResponseDTO? TareaSeleccionada { get; set; }
        public string NuevaTareaTitulo { get; set; } = string.Empty;
        public string NuevaTareaDescripcion { get; set; } = string.Empty;
        public DateTime NuevaTareaFecha { get; set; } = DateTime.Today.AddDays(7);
        public string EntregaTitulo { get; set; } = string.Empty;
        public string EntregaDescripcion { get; set; } = string.Empty;
        public FileResult? ArchivoSeleccionado { get; set; }
        public string ArchivoSeleccionadoNombre => ArchivoSeleccionado?.FileName ?? "Sin archivo";

        public ICommand CargarCommand { get; }
        public ICommand SeleccionarCursoCommand { get; }
        public ICommand CrearTareaCommand { get; }
        public ICommand SeleccionarTareaCommand { get; }
        public ICommand SeleccionarArchivoCommand { get; }
        public ICommand CrearEntregaCommand { get; }
        public ICommand CalificarEntregaCommand { get; }
        public ICommand EnviarRecordatoriosCommand { get; }

        public TareasViewModel(IGestionUsuarioServiceApp gestionUsuarioService, AuthorizationService authorizationService)
        {
            _gestionUsuarioService = gestionUsuarioService;
            _authorizationService = authorizationService;

            CargarCommand = new Command(async () => await LoadAsync());
            SeleccionarCursoCommand = new Command<CursoResponseDTO>(async c => await SeleccionarCursoAsync(c));
            CrearTareaCommand = new Command(async () => await CrearTareaAsync());
            SeleccionarTareaCommand = new Command<TareaResponseDTO>(async t => await SeleccionarTareaAsync(t));
            SeleccionarArchivoCommand = new Command(async () => await SeleccionarArchivoAsync());
            CrearEntregaCommand = new Command(async () => await CrearEntregaAsync());
            CalificarEntregaCommand = new Command<EntregaTareaResponseDTO>(async e => await CalificarEntregaAsync(e));
            EnviarRecordatoriosCommand = new Command(async () => await EnviarRecordatoriosAsync());
        }

        public async Task LoadAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                StatusMessage = null;

                Tareas.Clear();
                EntregasTarea.Clear();
                EntregasEstudiante.Clear();

                if (IsStudent)
                {
                    var tareas = await _gestionUsuarioService.ObtenerTareasDeEstudiante();
                    foreach (var tarea in tareas.OrderBy(t => t.fechaEntrega))
                    {
                        Tareas.Add(tarea);
                    }

                    var entregas = await _gestionUsuarioService.ObtenerEntregasDelEstudiante();
                    foreach (var entrega in entregas.OrderByDescending(e => e.FechaCreacion))
                    {
                        EntregasEstudiante.Add(entrega);
                    }
                }
                else
                {
                    Cursos.Clear();
                    var cursos = await _gestionUsuarioService.ObtenerCursosAsync();
                    foreach (var curso in cursos.OrderBy(c => c.nombre))
                    {
                        Cursos.Add(curso);
                    }

                    var tareas = await _gestionUsuarioService.ObtenerTareasDelProfesor();
                    foreach (var tarea in tareas.OrderByDescending(t => t.creada))
                    {
                        Tareas.Add(tarea);
                    }
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"No se pudieron cargar las tareas: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task SeleccionarCursoAsync(CursoResponseDTO? curso)
        {
            if (curso == null) return;

            CursoSeleccionado = curso;
            EstudiantesCurso.Clear();
            var estudiantes = await _gestionUsuarioService.ObtenerEstudiantesDeCurso(curso.cursoId);
            foreach (var estudiante in estudiantes.OrderBy(e => e.nombres))
            {
                EstudiantesCurso.Add(new SelectableEstudianteItem { Estudiante = estudiante, IsSelected = true });
            }
        }

        private async Task CrearTareaAsync()
        {
            try
            {
                if (CursoSeleccionado == null)
                {
                    StatusMessage = "Selecciona un curso.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(NuevaTareaTitulo) || string.IsNullOrWhiteSpace(NuevaTareaDescripcion))
                {
                    StatusMessage = "Completa título y descripción.";
                    return;
                }

                var estudiantesIds = EstudiantesCurso
                    .Where(e => e.IsSelected)
                    .Select(e => e.Estudiante.estudianteId)
                    .Distinct()
                    .ToList();

                if (!estudiantesIds.Any())
                {
                    StatusMessage = "Selecciona al menos un estudiante.";
                    return;
                }

                var tarea = await _gestionUsuarioService.CrearTareaAsync(new TareaRequestDTO
                {
                    Title = NuevaTareaTitulo,
                    Description = NuevaTareaDescripcion,
                    fechaEntrega = NuevaTareaFecha,
                    cursoId = CursoSeleccionado.cursoId
                });

                await _gestionUsuarioService.AsignarTareaAEstudiantes(tarea.HomeWorkId, estudiantesIds);
                NuevaTareaTitulo = string.Empty;
                NuevaTareaDescripcion = string.Empty;
                NuevaTareaFecha = DateTime.Today.AddDays(7);
                StatusMessage = "Tarea creada y asignada.";
                await LoadAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = $"No se pudo crear la tarea: {ex.Message}";
            }
        }

        private async Task SeleccionarTareaAsync(TareaResponseDTO? tarea)
        {
            if (tarea == null) return;

            TareaSeleccionada = tarea;
            EntregaTitulo = tarea.Title ?? string.Empty;
            EntregasTarea.Clear();

            if (IsTeacher)
            {
                var entregas = await _gestionUsuarioService.ObtenerEntregasDeTarea(tarea.HomeWorkId);
                foreach (var entrega in entregas.OrderByDescending(e => e.FechaCreacion))
                {
                    EntregasTarea.Add(entrega);
                }
            }
        }

        private async Task SeleccionarArchivoAsync()
        {
            try
            {
                ArchivoSeleccionado = await FilePicker.Default.PickAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = $"No se pudo seleccionar archivo: {ex.Message}";
            }
        }

        private async Task CrearEntregaAsync()
        {
            if (TareaSeleccionada == null)
            {
                StatusMessage = "Selecciona una tarea.";
                return;
            }

            if (string.IsNullOrWhiteSpace(EntregaTitulo))
            {
                StatusMessage = "Escribe un título para la entrega.";
                return;
            }

            var entrega = await _gestionUsuarioService.CrearEntregaAsync(
                TareaSeleccionada.HomeWorkId,
                EntregaTitulo,
                EntregaDescripcion,
                ArchivoSeleccionado);

            if (entrega == null)
            {
                StatusMessage = "No se pudo registrar la entrega. Puede que ya exista una entrega para esta tarea.";
                return;
            }

            EntregaDescripcion = string.Empty;
            ArchivoSeleccionado = null;
            StatusMessage = "Entrega registrada.";
            await LoadAsync();
        }

        private async Task CalificarEntregaAsync(EntregaTareaResponseDTO? entrega)
        {
            if (entrega == null) return;

            var calificacion = await Application.Current.MainPage.DisplayPromptAsync(
                "Calificar entrega",
                $"Calificación para {entrega.Estudiante?.nombres} {entrega.Estudiante?.apellidos}",
                "Guardar",
                "Cancelar",
                "Ej. 95/100");

            if (string.IsNullOrWhiteSpace(calificacion)) return;

            var comentarios = await Application.Current.MainPage.DisplayPromptAsync(
                "Comentario",
                "Retroalimentación para el estudiante",
                "Guardar",
                "Omitir",
                "Buen trabajo");

            var ok = await _gestionUsuarioService.CalificarEntregaAsync(entrega.EntregaTareaId, new CalificacionDTO
            {
                Calificacion = calificacion,
                Comentarios = comentarios
            });

            StatusMessage = ok ? "Entrega calificada." : "No se pudo calificar la entrega.";
            if (TareaSeleccionada != null)
            {
                await SeleccionarTareaAsync(TareaSeleccionada);
            }
        }

        private async Task EnviarRecordatoriosAsync()
        {
            var enviados = await _gestionUsuarioService.EnviarRecordatoriosVencimientoAsync();
            StatusMessage = enviados == 1
                ? "Se envió 1 recordatorio."
                : $"Se enviaron {enviados} recordatorios.";
        }
    }
}
