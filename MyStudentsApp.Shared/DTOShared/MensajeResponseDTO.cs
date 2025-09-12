using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStudentsApp.Shared.DTOShared
{
    public class MensajeResponseDTO
    {
        public int messageId { get; set; }
        public string? senderNombre { get; set; }
        public string? recipientNombre { get; set; }
        public string? senderUsuarioId { get; set; }
        public string? recipientUsuarioId { get; set; }
        public string? mensaje { get; set; }
        public DateTime enviado { get; set; }
        public bool IsRead { get; set; } = false;
    }
}

