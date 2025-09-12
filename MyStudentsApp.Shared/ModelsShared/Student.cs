using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStudentsApp.Shared.Pages;

namespace MyStudentsApp.Shared.ModelsShared
{
    public class Student
    {
        [Key]
        public int estudianteId { get; set; }
        public string? nombres { get; set; }
        public string? apellidos { get; set; }
        public DateTime cumpleanos { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? usuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        [ForeignKey("School")]
        public int schoolId { get; set; }
        public School? School { get; set; }

        public ICollection<Courses>? cursos { get; set; }
        public ICollection<HomeWork>? tareas { get; set; }
        public ICollection<EntregaDeTarea>? entregaDeTareas { get; set; }
        public ICollection<Teacher>? teachers { get; set; }

        public string? contrasena { get; set; }
        public bool usuarioValidado { get; set; }
    }
}
