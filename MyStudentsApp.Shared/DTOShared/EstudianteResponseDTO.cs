using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStudentsApp.Shared.DTOShared
{
    public class EstudianteResponseDTO
    {
        public int estudianteId { get; set; }
        public string? nombres { get; set; }
        public string? apellidos { get; set; }
        public DateTime cumpleanos { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? usuarioId { get; set; }
        public int schoolId { get; set; }
        public string? contrasena { get; set; }
        public bool usuarioValidado { get; set; }
        public string? curso { get; set; }
        public int mensajesPendientes { get; set; }
    }
}
