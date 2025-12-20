using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStudentsApp.Shared.DTOShared
{
    public class ProfesorResponseDTO
    {
        public int teacherId { get; set; }
        public string? usuarioId { get; set; }
        public string? nombres { get; set; }
        public string? apellidos { get; set; }
        public DateTime cumpleanos { get; set; }
        public string? email { get; set; }
        public string? telefono { get; set; }
        public int escuelaId { get; set; }
        public bool usuarioValidado { get; set; }
        public bool isAdministrativo { get; set; }
    }
}
