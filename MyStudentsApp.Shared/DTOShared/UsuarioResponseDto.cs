using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStudentsApp.Shared.DTOShared
{
    public class UsuarioResponseDto
    {
        public bool isStudent { get; set; }
        public int? studentId { get; set; }
        public int? teacherId { get; set; }
        public DateTime fechaCreacionCuenta { get; set; }
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }


    }
}
