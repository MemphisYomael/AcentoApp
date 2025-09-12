using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStudentsApp.Shared.ModelsShared
{
    public class EntregaDeTarea
    {
        [Key]
        public int EntregaTareaId { get; set; }
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }

        public DateTime FechaEntrega { get; set; }

        public DateTime FechaCreacion { get; set; }

        public int HomeWorkId { get; set; }
        public HomeWork? HomeWork { get; set; }

        public int EstudianteId { get; set; }
        public virtual Student? Estudiante { get; set; }

        public string? urlArchivo { get; set; } // URL or path to the submitted file

        public bool EstaCalificada { get; set; } // Indicates if the homework has been graded

        public DateTime? FechaCalificacion { get; set; } // Optional date when the homework was graded
        public string? Comentarios { get; set; } // Optional comments from the teacher regarding the submission

        public string? calificacion { get; set; }
    }
}
