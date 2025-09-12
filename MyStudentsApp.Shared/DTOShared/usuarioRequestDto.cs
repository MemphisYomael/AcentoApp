using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStudentsApp.Shared.DTOShared
{
    public class usuarioRequestDto
    {
        public bool isStudent { get; set; }
        public int? studentId { get; set; }
        public int? teacherId { get; set; }
        public string? contrasena { get; set; }
        public string? Email { get; set; }
        public bool isAdministrativo { get; set; }
    }
}
