
using Acento.MVVM.Models.Organizacion;
using Acento.MVVM.Models.Users;

namespace Acento.MVVM.Models.Estudents
{
    public class Student
    {
      
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string? usuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public int schoolId { get; set; }
        public School? School { get; set; }

        public List<int>? messagesIds { get; set; }
        public virtual List<Acento.MVVM.Models.ChatSignalR.Messages>? messages { get; set; }
        public string? contrasena { get; set; }
        public bool usuarioValidado { get; set; }

    }
}
