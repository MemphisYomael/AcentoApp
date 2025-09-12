using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStudentsApp.Shared.DTOShared
{
    public class CursoRequestDTO
    {
        public string? nombre { get; set; }
        public string? Description { get; set; }
        public int schoolId { get; set; }
    }
}
