using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStudentsApp.Shared.DTOShared
{
    public class TareaRequestDTO
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime fechaEntrega { get; set; }
        public int cursoId { get; set; }

    }
}
