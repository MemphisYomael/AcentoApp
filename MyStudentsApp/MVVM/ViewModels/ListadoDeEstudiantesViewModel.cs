using System.Windows.Input;
using MyStudentsApp.Services;
using MyStudentsApp.Shared.DTOShared;
using PropertyChanged;

namespace MyStudentsApp.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class ListadoDeEstudiantesViewModel
    {
        public string titulo { get; set; } = "Listado de estudiantes";
        public List<EstudianteResponseDTO> Estudiantes { get; set; } = new();
        public List<EstudianteResponseDTO> EstudiantesFiltrados { get; set; } = new();
        public ICommand buscar { get; set; }
        public string? ErrorCarga { get; set; }

        private readonly IGestionUsuarioServiceApp _gestionUsuarioService;

        public ListadoDeEstudiantesViewModel(IGestionUsuarioServiceApp gestionUsuarioService)
        {
            _gestionUsuarioService = gestionUsuarioService;
            buscar = new Command<string>(BuscarEstudiantes);
        }

        public async Task ObtenerEstudiantes()
        {
            try
            {
                ErrorCarga = null;
                var estudiantes = await _gestionUsuarioService.ObtenerEstudiantesAsync();
                Estudiantes = estudiantes ?? new List<EstudianteResponseDTO>();
                EstudiantesFiltrados = Estudiantes.ToList();
            }
            catch (Exception ex)
            {
                Estudiantes = new List<EstudianteResponseDTO>();
                EstudiantesFiltrados = new List<EstudianteResponseDTO>();
                ErrorCarga = $"Error al cargar estudiantes: {ex.Message}";
            }
        }

        public void BuscarEstudiantes(string? busquedaTexto)
        {
            if (string.IsNullOrWhiteSpace(busquedaTexto))
            {
                EstudiantesFiltrados = Estudiantes.ToList();
                return;
            }

            EstudiantesFiltrados = Estudiantes
                .Where(x =>
                    (x?.nombres ?? string.Empty).Contains(busquedaTexto, StringComparison.OrdinalIgnoreCase) ||
                    (x?.apellidos ?? string.Empty).Contains(busquedaTexto, StringComparison.OrdinalIgnoreCase) ||
                    (x?.curso ?? string.Empty).Contains(busquedaTexto, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
