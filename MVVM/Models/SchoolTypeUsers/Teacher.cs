

using Acento.MVVM.Models.Organizacion;

namespace Acento.MVVM.Models.SchoolTypeUsers
{
    public class Teacher
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; } 
        public DateTime BirthDate { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int schoolId { get; set; }
        public School? School { get; set; }

        public string? contrasena { get; set; }
        public bool usuarioValidado { get; set; }
    }
}
