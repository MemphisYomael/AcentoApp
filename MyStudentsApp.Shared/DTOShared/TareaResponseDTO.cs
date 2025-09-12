using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStudentsApp.Shared.DTOShared
{
    public class TareaResponseDTO
    {
        public int HomeWorkId { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime fechaEntrega { get; set; }

        public DateTime creada { get; set; }
        public ProfesorResponseDTO? Teacher { get; set; }
        public CursoResponseDTO? Curso { get; set; }
        public List<EstudianteResponseDTO>? Students { get; set; }
    }
}
