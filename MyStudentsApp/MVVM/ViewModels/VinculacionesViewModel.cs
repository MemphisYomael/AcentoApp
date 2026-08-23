using System.Collections.ObjectModel;
using System.Windows.Input;
using MyStudentsApp.Services;
using MyStudentsApp.Shared.DTOShared;
using PropertyChanged;

namespace MyStudentsApp.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class VinculacionesViewModel
    {
        public string Titulo { get; set; } = "Vinculaciones";
        
        // Colecciones
        public ObservableCollection<CursoResponseDTO> Cursos { get; set; } = new();
        public ObservableCollection<EstudianteResponseDTO> EstudiantesDisponibles { get; set; } = new();
        public ObservableCollection<EstudianteResponseDTO> EstudiantesDelCurso { get; set; } = new();
        public ObservableCollection<ProfesorResponseDTO> ProfesoresDisponibles { get; set; } = new();
        public ObservableCollection<ProfesorResponseDTO> ProfesoresDelCurso { get; set; } = new();
        
        // Selecciones
        public CursoResponseDTO? CursoSeleccionado { get; set; }
        public EstudianteResponseDTO? EstudianteSeleccionado { get; set; }
        public ProfesorResponseDTO? ProfesorSeleccionado { get; set; }
        
        // Estados de vista
        public bool MostrandoEstudiantes { get; set; } = true;
        public bool MostrandoProfesores { get; set; }
        
        private readonly IGestionUsuarioServiceApp _gestionUsuarioService;
        
        // Comandos
        public ICommand CargarCursosCommand { get; set; }
        public ICommand SeleccionarCursoCommand { get; set; }
        public ICommand VincularEstudianteCommand { get; set; }
        public ICommand DesvincularEstudianteCommand { get; set; }
        public ICommand VincularProfesorCommand { get; set; }
        public ICommand DesvincularProfesorCommand { get; set; }
        public ICommand MostrarEstudiantesCommand { get; set; }
        public ICommand MostrarProfesoresCommand { get; set; }
        
        public VinculacionesViewModel(IGestionUsuarioServiceApp gestionUsuarioService)
        {
            _gestionUsuarioService = gestionUsuarioService;
            
            CargarCursosCommand = new Command(async () => await CargarCursos());
            SeleccionarCursoCommand = new Command<CursoResponseDTO>(async (c) => await SeleccionarCurso(c));
            VincularEstudianteCommand = new Command<EstudianteResponseDTO>(async (e) => await VincularEstudiante(e));
            DesvincularEstudianteCommand = new Command<EstudianteResponseDTO>(async (e) => await DesvincularEstudiante(e));
            VincularProfesorCommand = new Command<ProfesorResponseDTO>(async (p) => await VincularProfesor(p));
            DesvincularProfesorCommand = new Command<ProfesorResponseDTO>(async (p) => await DesvincularProfesor(p));
            MostrarEstudiantesCommand = new Command(MostrarEstudiantes);
            MostrarProfesoresCommand = new Command(MostrarProfesores);
            
            _ = CargarCursos();
        }
        
        private async Task CargarCursos()
        {
            try
            {
                var cursos = await _gestionUsuarioService.ObtenerCursosAsync();
                Cursos.Clear();
                
                if (cursos != null)
                {
                    foreach (var curso in cursos)
                    {
                        Cursos.Add(curso);
                    }
                }
                else
                {
                    cursos = new();
                    foreach (var curso in cursos)
                    {
                        Cursos.Add(curso);
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error al cargar cursos: {ex.Message}", "OK");
            }
        }

        private async void OnCursoSeleccionadoChanged()
        {
            if (CursoSeleccionado == null)
                return;

            await SeleccionarCurso(CursoSeleccionado);
        }


        private async Task SeleccionarCurso(CursoResponseDTO curso)
        {
            if (curso == null) return;
            
            CursoSeleccionado = curso;
            
            try
            {
                // Cargar estudiantes del curso
                var estudiantesCurso = await _gestionUsuarioService.ObtenerEstudiantesDeCurso(curso.cursoId);
                EstudiantesDelCurso.Clear();
                
                if (estudiantesCurso != null)
                {
                    foreach (var estudiante in estudiantesCurso)
                    {
                        EstudiantesDelCurso.Add(estudiante);
                    }
                }
                
                // Cargar todos los estudiantes disponibles
                var todosEstudiantes = await _gestionUsuarioService.ObtenerEstudiantesAsync();
                EstudiantesDisponibles.Clear();
                
                if (todosEstudiantes != null)
                {
                    foreach (var estudiante in todosEstudiantes)
                    {
                        // Solo agregar si no está ya en el curso
                        if (!estudiantesCurso.Any(e => e.estudianteId == estudiante.estudianteId))
                        {
                            EstudiantesDisponibles.Add(estudiante);
                        }
                    }
                }
                
                // Cargar profesores del curso
                var profesoresCurso = await _gestionUsuarioService.ObtenerProfesoresDeCurso(curso.cursoId);
                ProfesoresDelCurso.Clear();
                
                if (profesoresCurso != null)
                {
                    foreach (var profesor in profesoresCurso)
                    {
                        ProfesoresDelCurso.Add(profesor);
                    }
                }
                
                // Cargar todos los profesores disponibles
                var todosProfesores = await _gestionUsuarioService.ObtenerProfesoresAsync();
                ProfesoresDisponibles.Clear();
                
                if (todosProfesores != null)
                {
                    foreach (var profesor in todosProfesores)
                    {
                        // Solo agregar si no está ya en el curso
                        if (!profesoresCurso.Any(p => p.usuarioId == profesor.usuarioId))
                        {
                            ProfesoresDisponibles.Add(profesor);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error al cargar detalles: {ex.Message}", "OK");
            }
        }
        
        private async Task VincularEstudiante(EstudianteResponseDTO estudiante)
        {
            if (estudiante == null || CursoSeleccionado == null) return;
            
            try
            {
                var resultado = await _gestionUsuarioService.VincularEstudianteACurso(estudiante.estudianteId, CursoSeleccionado.cursoId);
                
                if (resultado)
                {
                    EstudiantesDisponibles.Remove(estudiante);
                    EstudiantesDelCurso.Add(estudiante);
                    await Application.Current.MainPage.DisplayAlert("Éxito", "Estudiante vinculado correctamente", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se pudo vincular el estudiante", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }
        
        private async Task DesvincularEstudiante(EstudianteResponseDTO estudiante)
        {
            if (estudiante == null || CursoSeleccionado == null) return;
            
            bool confirmar = await Application.Current.MainPage.DisplayAlert(
                "Confirmar",
                $"¿Desvincular a {estudiante.nombres} {estudiante.apellidos} del curso?",
                "Sí", "No");
                
            if (!confirmar) return;
            
            try
            {
                var resultado = await _gestionUsuarioService.DesvincularEstudianteDeCurso(estudiante.estudianteId, CursoSeleccionado.cursoId);
                
                if (resultado)
                {
                    EstudiantesDelCurso.Remove(estudiante);
                    EstudiantesDisponibles.Add(estudiante);
                    await Application.Current.MainPage.DisplayAlert("Éxito", "Estudiante desvinculado correctamente", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se pudo desvincular el estudiante", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }
        
        private async Task VincularProfesor(ProfesorResponseDTO profesor)
        {
            if (profesor == null || CursoSeleccionado == null) return;
            
            try
            {
                var resultado = await _gestionUsuarioService.VincularProfesorACurso(profesor.teacherId, CursoSeleccionado.cursoId);
                
                if (resultado)
                {
                    ProfesoresDisponibles.Remove(profesor);
                    ProfesoresDelCurso.Add(profesor);
                    await Application.Current.MainPage.DisplayAlert("Éxito", "Profesor vinculado correctamente", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se pudo vincular el profesor", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }
        
        private async Task DesvincularProfesor(ProfesorResponseDTO profesor)
        {
            if (profesor == null || CursoSeleccionado == null) return;
            
            bool confirmar = await Application.Current.MainPage.DisplayAlert(
                "Confirmar",
                $"¿Desvincular a {profesor.nombres} {profesor.apellidos} del curso?",
                "Sí", "No");
                
            if (!confirmar) return;
            
            try
            {
                var resultado = await _gestionUsuarioService.DesvincularProfesorDeCurso(profesor.teacherId, CursoSeleccionado.cursoId);
                
                if (resultado)
                {
                    ProfesoresDelCurso.Remove(profesor);
                    ProfesoresDisponibles.Add(profesor);
                    await Application.Current.MainPage.DisplayAlert("Éxito", "Profesor desvinculado correctamente", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se pudo desvincular el profesor", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }
        
        private void MostrarEstudiantes()
        {
            MostrandoEstudiantes = true;
            MostrandoProfesores = false;
        }
        
        private void MostrarProfesores()
        {
            MostrandoEstudiantes = false;
            MostrandoProfesores = true;
        }
    }
}
