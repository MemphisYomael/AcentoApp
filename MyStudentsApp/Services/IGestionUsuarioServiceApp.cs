using MyStudentsApp.Shared.DTOShared;
using MyStudentsApp.Shared.ModelsShared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyStudentsApp.Services
{
    public interface IGestionUsuarioServiceApp
    {
        #region Autenticación y Usuarios
        Task<bool> Login(LoginRequestDTO request);
        Task<UsuarioResponseDto> GetUsuarioActual();
        Task<List<UsuarioResponseDto>> ObtenerUsuariosAsync(string? busqueda = null);
        Task<UsuarioResponseDto> ObtenerUsuarioPorIdAsync(string id);
        Task<bool> CrearUsuario(usuarioRequestDto usuarioDto);
        Task<bool> ActualizarUsuario(string id, usuarioRequestDto usuarioDto);
        Task<bool> EliminarUsuario(string id);
        #endregion

        #region Estudiantes
        Task<List<EstudianteResponseDTO>> ObtenerEstudiantesAsync(string? busqueda = null);
        Task<EstudianteResponseDTO> ObtenerEstudiantePorIdAsync(string id);
        Task<EstudianteResponseDTO> CrearEstudianteAsync(EstudianteCreacionDTO estudianteDto);
        Task<bool> ActualizarEstudianteAsync(string id, EstudianteCreacionDTO estudianteDto);
        Task<bool> EliminarEstudianteAsync(string id);
        #endregion

        #region Profesores
        Task<List<ProfesorResponseDTO>> ObtenerProfesoresAsync(string? busqueda = null);
        Task<ProfesorResponseDTO> ObtenerProfesorPorIdAsync(string id);
        Task<ProfesorResponseDTO> CrearProfesorAsync(ProfesorCrearDTO profesorDto);
        Task<bool> ActualizarProfesorAsync(string id, ProfesorCrearDTO profesorDto);
        Task<bool> EliminarProfesorAsync(string id);
        Task<List<ProfesorResponseDTO>> obtenerDirectores();
        #endregion

        #region Escuelas
        Task<List<EscuelaResponseDto>> obtenerEscuelasAsync(string? busqueda = null);
        Task<EscuelaResponseDto> ObtenerEscuelaPorIdAsync(string id);
        Task<bool> CrearEscuelaYDirector(UsuarioEspecialRequestDTO escuelaDto); // Renombrado para mayor claridad
        Task<bool> ActualizarEscuelaAsync(string id, EscuelaCreacionDTO escuelaDto);
        Task<bool> EliminarEscuelaAsync(string id);
        #endregion

        #region Cursos
        Task<List<CursoResponseDTO>> ObtenerCursosAsync(string? busqueda = null);
        Task<CursoResponseDTO> ObtenerCursoPorIdAsync(string id);
        Task<CursoResponseDTO> CrearCursoAsync(CursoRequestDTO cursoDto);
        Task<bool> ActualizarCursoAsync(string id, CursoRequestDTO cursoDto);
        Task<bool> EliminarCursoAsync(string id);
        #endregion

        #region Tareas
        Task<List<TareaResponseDTO>> ObtenerTareasAsync(string? busqueda = null);
        Task<TareaResponseDTO> ObtenerTareaPorIdAsync(string id);
        Task<TareaResponseDTO> CrearTareaAsync(TareaRequestDTO tareaDto);
        Task<bool> ActualizarTareaAsync(string id, TareaRequestDTO tareaDto);
        Task<bool> EliminarTareaAsync(string id);
        #endregion

        #region Entregas de Tareas
        Task<List<EntregaTareaResponseDTO>> ObtenerEntregasAsync(string? busqueda = null);
        Task<EntregaTareaResponseDTO> ObtenerEntregaPorIdAsync(string id);
        Task<bool> CrearEntregaAsync(EntregaTareaRequestDTO entregaDto);
        Task<bool> ActualizarEntregaAsync(string id, EntregaTareaRequestDTO entregaDto);
        Task<bool> EliminarEntregaAsync(string id);
        #endregion

        #region Vinculaciones
        Task<bool> VincularEstudianteACurso(int estudianteId, int cursoId);
        Task<bool> DesvincularEstudianteDeCurso(int estudianteId, int cursoId);
        Task<bool> VincularProfesorACurso(int profesorId, int cursoId);
        Task<bool> DesvincularProfesorDeCurso(int profesorId, int cursoId);
        Task<List<EstudianteResponseDTO>> ObtenerEstudiantesDeCurso(int cursoId);
        Task<List<ProfesorResponseDTO>> ObtenerProfesoresDeCurso(int cursoId);
        Task<List<ProfesorResponseDTO>> ObtenerProfesoresDeEstudianteAsync();
        #endregion
    }
}