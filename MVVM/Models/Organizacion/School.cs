
using Acento.MVVM.Models.Estudents;
using Acento.MVVM.Models.SchoolTypeUsers;

namespace Acento.MVVM.Models.Organizacion
{
    public class School
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<int>? Students { get; set; }
        public virtual List<Student>? StudentsLists { get; set; }

        public List<int>? Teachers { get; set; }
        public virtual List<Teacher>? TeachersLists { get; set; }

        public virtual School? school { get; set; }
        public int Courses { get; set; }

    }
}
