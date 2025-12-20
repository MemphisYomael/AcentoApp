using System.Collections.ObjectModel;
using System.Windows.Input;
using MyStudentsApp.Services;
using MyStudentsApp.Shared.DTOShared;
using PropertyChanged;

namespace MyStudentsApp.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class GestionProfesoresViewModel
    {
        public string Titulo { get; set; } = "Gestión de Profesores";
        public ObservableCollection<ProfesorResponseDTO> Profesores { get; set; } = new();
        public ObservableCollection<ProfesorResponseDTO> ProfesoresFiltrados { get; set; } = new();
        public ProfesorResponseDTO? ProfesorSeleccionado { get; set; }
        public bool MostrarFormularioEdicion { get; set; }
        public bool EsNuevoProfesor { get; set; }
        
        // Propiedades del formulario
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public DateTime Cumpleanos { get; set; } = DateTime.Now.AddYears(-25);
        public string? Contrasena { get; set; }
        public bool IsAdministrativo { get; set; }

        private readonly IGestionUsuarioServiceApp _gestionUsuarioService;

        public ICommand BuscarCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand NuevoProfesorCommand { get; set; }
        public ICommand GuardarCommand { get; set; }
        public ICommand CancelarCommand { get; set; }

        public GestionProfesoresViewModel(IGestionUsuarioServiceApp gestionUsuarioService)
        {
            _gestionUsuarioService = gestionUsuarioService;
            
            BuscarCommand = new Command<string>(BuscarProfesores);
            EditarCommand = new Command<ProfesorResponseDTO>(EditarProfesor);
            EliminarCommand = new Command<ProfesorResponseDTO>(async (p) => await EliminarProfesor(p));
            NuevoProfesorCommand = new Command(MostrarFormularioNuevo);
            GuardarCommand = new Command(async () => await GuardarProfesor());
            CancelarCommand = new Command(CancelarEdicion);
            
            _ = CargarProfesores();
        }

        public async Task CargarProfesores()
        {
            try
            {
                var profesores = await _gestionUsuarioService.ObtenerProfesoresAsync();
                Profesores.Clear();
                ProfesoresFiltrados.Clear();
                
                if (profesores != null)
                {
                    foreach (var profesor in profesores)
                    {
                        Profesores.Add(profesor);
                        ProfesoresFiltrados.Add(profesor);
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error al cargar profesores: {ex.Message}", "OK");
            }
        }

        private void BuscarProfesores(string busqueda)
        {
            if (string.IsNullOrWhiteSpace(busqueda))
            {
                ProfesoresFiltrados.Clear();
                foreach (var profesor in Profesores)
                {
                    ProfesoresFiltrados.Add(profesor);
                }
                return;
            }

            var filtrados = Profesores.Where(p =>
                (!string.IsNullOrEmpty(p.nombres) && p.nombres.Contains(busqueda, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(p.apellidos) && p.apellidos.Contains(busqueda, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(p.email) && p.email.Contains(busqueda, StringComparison.OrdinalIgnoreCase))
            ).ToList();

            ProfesoresFiltrados.Clear();
            foreach (var profesor in filtrados)
            {
                ProfesoresFiltrados.Add(profesor);
            }
        }

        private void EditarProfesor(ProfesorResponseDTO profesor)
        {
            ProfesorSeleccionado = profesor;
            Nombres = profesor.nombres;
            Apellidos = profesor.apellidos;
            Email = profesor.email;
            Telefono = profesor.telefono;
            Cumpleanos = profesor.cumpleanos;
            IsAdministrativo = profesor.isAdministrativo;
            EsNuevoProfesor = false;
            MostrarFormularioEdicion = true;
        }

        private async Task EliminarProfesor(ProfesorResponseDTO profesor)
        {
            if (profesor?.usuarioId == null) return;

            bool confirmar = await Application.Current.MainPage.DisplayAlert(
                "Confirmar eliminación",
                $"¿Está seguro de eliminar al profesor {profesor.nombres} {profesor.apellidos}?",
                "Sí", "No");

            if (!confirmar) return;

            try
            {
                var resultado = await _gestionUsuarioService.EliminarProfesorAsync(profesor.usuarioId);
                if (resultado)
                {
                    Profesores.Remove(profesor);
                    ProfesoresFiltrados.Remove(profesor);
                    await Application.Current.MainPage.DisplayAlert("Éxito", "Profesor eliminado correctamente", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se pudo eliminar el profesor", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error al eliminar: {ex.Message}", "OK");
            }
        }

        private void MostrarFormularioNuevo()
        {
            LimpiarFormulario();
            EsNuevoProfesor = true;
            MostrarFormularioEdicion = true;
        }

        private async Task GuardarProfesor()
        {
            if (string.IsNullOrWhiteSpace(Nombres) || string.IsNullOrWhiteSpace(Email))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Complete los campos obligatorios", "OK");
                return;
            }

            try
            {
                var profesorDto = new ProfesorCrearDTO
                {
                    nombres = Nombres,
                    apellidos = Apellidos,
                    email = Email,
                    telefono = Telefono,
                    cumpleanos = Cumpleanos,
                    contrasena = Contrasena ?? "Temp123!",
                    isAdministrativo = IsAdministrativo
                };

                if (EsNuevoProfesor)
                {
                    var nuevoProfesor = await _gestionUsuarioService.CrearProfesorAsync(profesorDto);
                    if (nuevoProfesor != null)
                    {
                        await CargarProfesores();
                        await Application.Current.MainPage.DisplayAlert("Éxito", "Profesor creado correctamente", "OK");
                    }
                }
                else if (ProfesorSeleccionado?.usuarioId != null)
                {
                    var resultado = await _gestionUsuarioService.ActualizarProfesorAsync(ProfesorSeleccionado.usuarioId, profesorDto);
                    if (resultado)
                    {
                        await CargarProfesores();
                        await Application.Current.MainPage.DisplayAlert("Éxito", "Profesor actualizado correctamente", "OK");
                    }
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
            ProfesorSeleccionado = null;
            Nombres = string.Empty;
            Apellidos = string.Empty;
            Email = string.Empty;
            Telefono = string.Empty;
            Contrasena = string.Empty;
            IsAdministrativo = false;
            Cumpleanos = DateTime.Now.AddYears(-25);
        }
    }
}
