using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStudentsApp.Shared.DTOShared
{
    public class EscuelaResponseDto
    {
        public int escuelaId { get; set; }
        public string? nombre { get; set; }
        public string? direccion { get; set; }
        public string? numero { get; set; }
        public string? directorId { get; set; }
        public string? directorName { get; set; }
    }
}
