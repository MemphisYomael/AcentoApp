using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStudentsApp.Shared.ModelsShared
{
    public class Teacher
    {
        [Key]
        public int teacherId { get; set; }
        public string? nombres { get; set; }
        public string? apellidos { get; set; }
        public DateTime cumpleanos { get; set; }
        public string? email { get; set; }
        public string? telefono { get; set; }
        public int escuelaId { get; set; }
        public School? escuela { get; set; }
        public ICollection<Student>? estudiantes { get; set; }
        public string? usuarioId { get; set; }
        public Usuario? usuario { get; set; }
        public ICollection<Courses>? cursos { get; set; }
        public ICollection<HomeWork>? tareas { get; set; }
        public string? contrasena { get; set; }
        public bool usuarioValidado { get; set; }
        public bool isAdministrativo { get; set; }
    }
}
