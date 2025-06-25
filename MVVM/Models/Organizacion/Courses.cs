
using Acento.MVVM.Models.Estudents;
using Acento.MVVM.Models.Organizacion;
using Acento.MVVM.Models.SchoolTypeUsers;

namespace Acento.MVVM.Models
{
    public class Courses
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int schoolId { get; set; }
        public School? School { get; set; }
        public List<int>? Students { get; set; }
        public List<Student>? students { get; set; }
        public List<int>? Teachers { get; set; }
        public List<Teacher>? teachers { get; set; }
        public List<int>? Assignments { get; set; }
        public List<int>? Exams { get; set; }
        public List<int>? Notes { get; set; }
        public List<int>? Grades { get; set; }
    }
}
