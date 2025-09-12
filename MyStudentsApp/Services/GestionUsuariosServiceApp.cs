using MyStudentsApp.DbContext;
using MyStudentsApp.Shared.DTOShared;
using MyStudentsApp.Shared.ModelsShared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyStudentsApp.Services
{
    public class GestionUsuariosServiceApp : IGestionUsuarioServiceApp
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;

        public GestionUsuariosServiceApp()
        {
            _httpClient = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true, // Es una buena práctica para la deserialización
                WriteIndented = true
            };
            _httpClient.BaseAddress = new Uri("https://localhost:7224/api/GestionUsuarios/");
        }

        /// <summary>
        /// Adjunta el token de autenticación guardado a la cabecera del cliente HTTP.
        /// </summary>
        private void SetAuthorizationHeader()
        {
            var token = Preferences.Get("token", string.Empty);
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        #region Helpers Genéricos para Peticiones

        private async Task<T> GetAsync<T>(string uri)
        {
            try
            {
                SetAuthorizationHeader();
                var response = await _httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var contentStream = await response.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<T>(contentStream, _serializerOptions);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GET {uri}: {ex}");
            }
            return default;
        }

        private async Task<TResponse> PostAsync<TRequest, TResponse>(string uri, TRequest data)
        {
            try
            {
                SetAuthorizationHeader();
                var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    var contentStream = await response.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<TResponse>(contentStream, _serializerOptions);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en POST {uri}: {ex}");
            }
            return default;
        }

        private async Task<bool> PutAsync<TRequest>(string uri, TRequest data)
        {
            try
            {
                SetAuthorizationHeader();
                var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(uri, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en PUT {uri}: {ex}");
                return false;
            }
        }

        private async Task<bool> DeleteAsync(string uri)
        {
            try
            {
                SetAuthorizationHeader();
                var response = await _httpClient.DeleteAsync(uri);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en DELETE {uri}: {ex}");
                return false;
            }
        }

        #endregion

        #region Autenticación y Usuarios

        public async Task<bool> Login(LoginRequestDTO request)
        {
            var response = await PostAsync<LoginRequestDTO, GuardarSesionModel>("login", request);
            if (response != null)
            {
                Preferences.Set("token", response.token);
                Preferences.Set("userName", response.userName);
                Preferences.Set("userId", response.userId);
                Preferences.Set("password", request.Password);
                Preferences.Set("email", request.Email);
                return true;
            }
            return false;
        }

        public async Task<UsuarioResponseDto> GetUsuarioActual() => await GetAsync<UsuarioResponseDto>("getUsuarioActual");

        public async Task<List<UsuarioResponseDto>> ObtenerUsuariosAsync(string? busqueda = null) => await GetAsync<List<UsuarioResponseDto>>($"obtenerUsuarios?busqueda={busqueda}");

        public async Task<UsuarioResponseDto> ObtenerUsuarioPorIdAsync(string id) => await GetAsync<UsuarioResponseDto>($"obtenerUsuario/{id}");

        public async Task<bool> CrearUsuario(usuarioRequestDto usuarioDto)
        {
            var response = await PostAsync<usuarioRequestDto, UsuarioResponseDto>("crearUsuario", usuarioDto);
            return response != null;
        }

        public async Task<bool> ActualizarUsuario(string id, usuarioRequestDto usuarioDto) => await PutAsync($"actualizarUsuario/{id}", usuarioDto);

        public async Task<bool> EliminarUsuario(string id) => await DeleteAsync($"eliminarUsuario/{id}");

        #endregion

        #region Estudiantes

        public async Task<List<EstudianteResponseDTO>> ObtenerEstudiantesAsync(string? busqueda = null) => await GetAsync<List<EstudianteResponseDTO>>($"obtenerEstudiantes?busqueda={busqueda}");

        public async Task<EstudianteResponseDTO> ObtenerEstudiantePorIdAsync(string id) => await GetAsync<EstudianteResponseDTO>($"obtenerEstudiante/{id}");

        public async Task<EstudianteResponseDTO> CrearEstudianteAsync(EstudianteCreacionDTO estudianteDto) => await PostAsync<EstudianteCreacionDTO, EstudianteResponseDTO>("crearEstudiante", estudianteDto);

        public async Task<bool> ActualizarEstudianteAsync(string id, EstudianteCreacionDTO estudianteDto) => await PutAsync($"actualizarEstudiante/{id}", estudianteDto);

        public async Task<bool> EliminarEstudianteAsync(string id) => await DeleteAsync($"eliminarEstudiante/{id}");

        #endregion

        #region Profesores

        public async Task<List<ProfesorResponseDTO>> ObtenerProfesoresAsync(string? busqueda = null) => await GetAsync<List<ProfesorResponseDTO>>($"obtenerProfesores?busqueda={busqueda}");

        public async Task<ProfesorResponseDTO> ObtenerProfesorPorIdAsync(string id) => await GetAsync<ProfesorResponseDTO>($"obtenerProfesor/{id}");

        public async Task<ProfesorResponseDTO> CrearProfesorAsync(ProfesorCrearDTO profesorDto) => await PostAsync<ProfesorCrearDTO, ProfesorResponseDTO>("crearProfesor", profesorDto);

        public async Task<bool> ActualizarProfesorAsync(string id, ProfesorCrearDTO profesorDto) => await PutAsync($"actualizarProfesor/{id}", profesorDto);

        public async Task<bool> EliminarProfesorAsync(string id) => await DeleteAsync($"eliminarProfesor/{id}");

        public async Task<List<ProfesorResponseDTO>> obtenerDirectores() => await GetAsync<List<ProfesorResponseDTO>>("obtenerDirectores");

        #endregion

        #region Escuelas

        // CORREGIDO: El endpoint del controller usa 'busqueda'.
        public async Task<List<EscuelaResponseDto>> obtenerEscuelasAsync(string? busqueda = null) => await GetAsync<List<EscuelaResponseDto>>($"obtenerEscuelas?busqueda={busqueda}");

        public async Task<EscuelaResponseDto> ObtenerEscuelaPorIdAsync(string id) => await GetAsync<EscuelaResponseDto>($"obtenerEscuela/{id}");

        // CORREGIDO: Llama al endpoint correcto que crea el usuario, el profesor y la escuela.
        public async Task<bool> CrearEscuelaYDirector(UsuarioEspecialRequestDTO escuelaDto)
        {
            var response = await PostAsync<UsuarioEspecialRequestDTO, UsuarioResponseDto>("crearUsuarioEspecial", escuelaDto);
            return response != null;
        }

        public async Task<bool> ActualizarEscuelaAsync(string id, EscuelaCreacionDTO escuelaDto) => await PutAsync($"actualizarEscuela/{id}", escuelaDto);

        public async Task<bool> EliminarEscuelaAsync(string id) => await DeleteAsync($"eliminarEscuela/{id}");

        #endregion

        #region Cursos

        public async Task<List<CursoResponseDTO>> ObtenerCursosAsync(string? busqueda = null) => await GetAsync<List<CursoResponseDTO>>($"obtenerCursos?busqueda={busqueda}");

        public async Task<CursoResponseDTO> ObtenerCursoPorIdAsync(string id) => await GetAsync<CursoResponseDTO>($"obtenerCurso/{id}");

        public async Task<CursoResponseDTO> CrearCursoAsync(CursoRequestDTO cursoDto) => await PostAsync<CursoRequestDTO, CursoResponseDTO>("crearCurso", cursoDto);

        public async Task<bool> ActualizarCursoAsync(string id, CursoRequestDTO cursoDto) => await PutAsync($"actualizarCurso/{id}", cursoDto);

        public async Task<bool> EliminarCursoAsync(string id) => await DeleteAsync($"eliminarCurso/{id}");

        #endregion

        #region Tareas

        public async Task<List<TareaResponseDTO>> ObtenerTareasAsync(string? busqueda = null) => await GetAsync<List<TareaResponseDTO>>($"obtenerTareas?busqueda={busqueda}");

        public async Task<TareaResponseDTO> ObtenerTareaPorIdAsync(string id) => await GetAsync<TareaResponseDTO>($"obtenerTarea/{id}");

        public async Task<TareaResponseDTO> CrearTareaAsync(TareaRequestDTO tareaDto) => await PostAsync<TareaRequestDTO, TareaResponseDTO>("crearTarea", tareaDto);

        public async Task<bool> ActualizarTareaAsync(string id, TareaRequestDTO tareaDto) => await PutAsync($"actualizarTarea/{id}", tareaDto);

        public async Task<bool> EliminarTareaAsync(string id) => await DeleteAsync($"eliminarTarea/{id}");

        #endregion

        #region Entregas de Tareas

        public async Task<List<EntregaTareaResponseDTO>> ObtenerEntregasAsync(string? busqueda = null) => await GetAsync<List<EntregaTareaResponseDTO>>($"obtenerEntregas?busqueda={busqueda}");

        public async Task<EntregaTareaResponseDTO> ObtenerEntregaPorIdAsync(string id) => await GetAsync<EntregaTareaResponseDTO>($"obtenerEntrega/{id}");

        public async Task<bool> CrearEntregaAsync(EntregaTareaRequestDTO entregaDto)
        {
            var response = await PostAsync<EntregaTareaRequestDTO, EntregaTareaResponseDTO>("crearEntrega", entregaDto);
            return response != null;
        }

        public async Task<bool> ActualizarEntregaAsync(string id, EntregaTareaRequestDTO entregaDto) => await PutAsync($"actualizarEntrega/{id}", entregaDto);

        public async Task<bool> EliminarEntregaAsync(string id) => await DeleteAsync($"eliminarEntrega/{id}");

        #endregion

        // Los métodos originales que no fueron reemplazados pueden ser eliminados o comentados si ya no los necesitas.
        // He dejado "obtenerDirectores" y "Login" ya que estaban bien definidos.
        // Los otros métodos originales como ObtenerUsuariosAsync y obtenerEscuelasAsync han sido reemplazados
        // por las nuevas implementaciones genéricas para mantener la consistencia.
        public Task<bool> CrearEscuela(UsuarioEspecialRequestDTO escuela)
        {
            // Este método está obsoleto, usar CrearEscuelaYDirector en su lugar.
            throw new NotImplementedException("Usar CrearEscuelaYDirector en su lugar.");
        }
    }
}