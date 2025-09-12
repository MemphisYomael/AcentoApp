using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MyStudentsApp.Shared.DTOShared
{
    public class EntregaTareaRequestDTO
    {
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public IFormFile? File { get; set; } // Representa el archivo enviado por el estudiante
    }
}
