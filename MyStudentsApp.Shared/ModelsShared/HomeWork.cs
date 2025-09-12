using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStudentsApp.Shared.ModelsShared
{
    public class HomeWork
    {
        [Key]
        public int HomeWorkId { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime fechaEntrega { get; set; }

        public DateTime creada { get; set; }

        public List<Student>? Students { get; set; }

        public int teacherId { get; set; }
        public Teacher? Teacher { get; set; }

        public ICollection<EntregaDeTarea>? entregasDeTareas { get; set; }

        public int cursoId { get; set; }
        public Courses? Curso { get; set; }

    }
}
