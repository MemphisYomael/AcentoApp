
using Acento.MVVM.Models.SchoolTypeUsers;

namespace Acento.MVVM.Models.Works
{
    public class HomeWork
    {
        public int HomeWorkId { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime fechaEntrega { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<int>? Students { get; set; }

        public virtual Teacher? Teacher { get; set; }
        public int teacherId { get; set; }

        public List<EntregaDeTarea>? entregasDeTareas { get; set; }
        public List<int>? entregaDeTareas { get; set; }

    }
}
