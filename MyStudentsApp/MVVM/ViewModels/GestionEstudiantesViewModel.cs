using System.Collections.ObjectModel;
using System.Windows.Input;
using MyStudentsApp.Services;
using MyStudentsApp.Shared.DTOShared;
using PropertyChanged;

namespace MyStudentsApp.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class GestionEstudiantesViewModel
    {
        public string Titulo { get; set; } = "Gestión de Estudiantes";
        public ObservableCollection<EstudianteResponseDTO> Estudiantes { get; set; } = new();
        public ObservableCollection<EstudianteResponseDTO> EstudiantesFiltrados { get; set; } = new();
        public EstudianteResponseDTO? EstudianteSeleccionado { get; set; }
        public bool MostrarFormularioEdicion { get; set; }
        public bool EsNuevoEstudiante { get; set; }
        
        // Propiedades del formulario
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public DateTime Cumpleanos { get; set; } = DateTime.Now.AddYears(-10);
        public string? Contrasena { get; set; }

        private readonly IGestionUsuarioServiceApp _gestionUsuarioService;

        public ICommand BuscarCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand NuevoEstudianteCommand { get; set; }
        public ICommand GuardarCommand { get; set; }
        public ICommand CancelarCommand { get; set; }

        public GestionEstudiantesViewModel(IGestionUsuarioServiceApp gestionUsuarioService)
        {
            _gestionUsuarioService = gestionUsuarioService;
            
            BuscarCommand = new Command<string>(BuscarEstudiantes);
            EditarCommand = new Command<EstudianteResponseDTO>(EditarEstudiante);
            EliminarCommand = new Command<EstudianteResponseDTO>(async (e) => await EliminarEstudiante(e));
            GuardarCommand = new Command(async () => await GuardarEstudiante());
            CancelarCommand = new Command(CancelarEdicion);
            
            _ = CargarEstudiantes();
        }

        public async Task CargarEstudiantes()
        {
            try
            {
                var estudiantes = await _gestionUsuarioService.ObtenerEstudiantesAsync();
                Estudiantes.Clear();
                EstudiantesFiltrados.Clear();
                
                if (estudiantes != null)
                {
                    foreach (var estudiante in estudiantes)
                    {
                        Estudiantes.Add(estudiante);
                        EstudiantesFiltrados.Add(estudiante);
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error al cargar estudiantes: {ex.Message}", "OK");
            }
        }

        private void BuscarEstudiantes(string busqueda)
        {
            if (string.IsNullOrWhiteSpace(busqueda))
            {
                EstudiantesFiltrados.Clear();
                foreach (var estudiante in Estudiantes)
                {
                    EstudiantesFiltrados.Add(estudiante);
                }
                return;
            }

            var filtrados = Estudiantes.Where(e =>
                (!string.IsNullOrEmpty(e.nombres) && e.nombres.Contains(busqueda, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(e.apellidos) && e.apellidos.Contains(busqueda, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(e.curso) && e.curso.Contains(busqueda, StringComparison.OrdinalIgnoreCase))
            ).ToList();

            EstudiantesFiltrados.Clear();
            foreach (var estudiante in filtrados)
            {
                EstudiantesFiltrados.Add(estudiante);
            }
        }

        private void EditarEstudiante(EstudianteResponseDTO estudiante)
        {
            EstudianteSeleccionado = estudiante;
            Nombres = estudiante.nombres;
            Apellidos = estudiante.apellidos;
            Email = estudiante.Email;
            Telefono = estudiante.PhoneNumber;
            Cumpleanos = estudiante.cumpleanos;
            EsNuevoEstudiante = false;
            MostrarFormularioEdicion = true;
        }

        private async Task EliminarEstudiante(EstudianteResponseDTO estudiante)
        {
            if (estudiante == null) return;

            bool confirmar = await Application.Current.MainPage.DisplayAlert(
                "Confirmar eliminación",
                $"¿Está seguro de eliminar al estudiante {estudiante.nombres} {estudiante.apellidos}?",
                "Sí", "No");

            if (!confirmar) return;

            try
            {
                var resultado = await _gestionUsuarioService.EliminarEstudianteAsync(estudiante.estudianteId.ToString());
                if (resultado)
                {
                    Estudiantes.Remove(estudiante);
                    EstudiantesFiltrados.Remove(estudiante);
                    await Application.Current.MainPage.DisplayAlert("Éxito", "Estudiante eliminado correctamente", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se pudo eliminar el estudiante", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error al eliminar: {ex.Message}", "OK");
            }
        }

        private async Task GuardarEstudiante()
        {
            if (string.IsNullOrWhiteSpace(Nombres))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Complete los campos obligatorios", "OK");
                return;
            }

            try
            {
                var estudianteDto = new EstudianteCreacionDTO
                {
                    nombres = Nombres,
                    apellidos = Apellidos,
                    Email = Email,
                    PhoneNumber = Telefono,
                    cumpleanos = Cumpleanos,
                    contrasena = Contrasena ?? "Temp123!"
                };

                if (EsNuevoEstudiante)
                {
                    var nuevoEstudiante = await _gestionUsuarioService.CrearEstudianteAsync(estudianteDto);
                    if (nuevoEstudiante == null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "No se pudo crear el estudiante", "OK");
                        return;
                    }

                    await CargarEstudiantes();
                    await Application.Current.MainPage.DisplayAlert("Éxito", "Estudiante creado correctamente", "OK");
                }
                else if (EstudianteSeleccionado != null)
                {
                    var resultado = await _gestionUsuarioService.ActualizarEstudianteAsync(EstudianteSeleccionado.estudianteId.ToString(), estudianteDto);
                    if (!resultado)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "No se pudo actualizar el estudiante", "OK");
                        return;
                    }

                    await CargarEstudiantes();
                    await Application.Current.MainPage.DisplayAlert("Éxito", "Estudiante actualizado correctamente", "OK");
                }

                MostrarFormularioEdicion = false;
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error al guardar: {ex.Message}", "OK");
            }
        }

        private void CancelarEdicion()
        {
            MostrarFormularioEdicion = false;
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            EstudianteSeleccionado = null;
            Nombres = string.Empty;
            Apellidos = string.Empty;
            Email = string.Empty;
            Telefono = string.Empty;
            Contrasena = string.Empty;
            Cumpleanos = DateTime.Now.AddYears(-10);
        }
    }
}
