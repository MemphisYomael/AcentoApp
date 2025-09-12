using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStudentsApp.Shared.Pages;

namespace MyStudentsApp.Shared.ModelsShared
{
    public class Courses
    {
        [Key]
        public int cursoId { get; set; }
        public string? nombre { get; set; }
        public string? Description { get; set; }
        public int schoolId { get; set; }
        public School? School { get; set; }
        public ICollection<Student>? estudiantes { get; set; }
        public ICollection<Teacher>? profesores { get; set; }
        public ICollection<HomeWork>? tareas { get; set; }
    }
}
