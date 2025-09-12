using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStudentsApp.Shared.ModelsShared
{
    public class Usuario 
    {
        public string? id { get; set; }
        public string? userName { get; set; }
        public string? normalizedUserName { get; set; }
        public string? email { get; set; }
        public string? normalizedEmail { get; set; }
        public bool? emailConfirmed { get; set; }
        public string? phoneNumber { get; set; }
        public bool? phoneNumberConfirmed { get; set; }
        public bool? lockoutEnabled { get; set; }
        public int? accessFailedCount { get; set; }
        public DateTime fechaCreacionCuenta { get; set; }
        public ICollection<Courses>? cursos { get; set; }
        public School? escuela { get; set; }
        public bool isStudent { get; set; }

        // Hacer estas nullable
        public int? studentId { get; set; }
        public Student? student { get; set; }

        public int? teacherId { get; set; }
        public Teacher? teacher { get; set; }

        public ICollection<Messages>? mensajes { get; set; }

        public string? passwordHash { get; set; }
        public string? securityStamp { get; set; }
        public string? concurrencyStamp { get; set; }
        public bool twoFactorEnabled { get; set; }
        public DateTime? lockoutEnd { get; set; }

    }
}
