using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStudentsApp.Shared.Pages;

namespace MyStudentsApp.Shared.ModelsShared
{
    public class School
    {
        [Key]
        public int escuelaId { get; set; }
        public string? nombre { get; set; }
        public string? direccion { get; set; }
        public string? numero { get; set; }
        public ICollection<Student>? estudiantes { get; set; }
        public ICollection<Teacher>? Teachers { get; set; }
        public ICollection<Courses>? cursos { get; set; }
        public string? directorId { get; set; }
        public Usuario? director { get; set; }
    }
}
