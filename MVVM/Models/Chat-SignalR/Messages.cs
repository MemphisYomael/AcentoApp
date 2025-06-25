using Acento.MVVM.Models.Works;

namespace Acento.MVVM.Models.ChatSignalR
{
    public class Messages
    {
        public int Id { get; set; }
        public string? senderUserName { get; set; }
        public string senderUserId { get; set; }
        public string? recipientUserName { get; set; }
        public string? recipientUserId { get; set; }
        public string? Message { get; set; }
        public int? tareaId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; } = false;
        public bool isStudent { get; set; }
        public bool isTeacher { get; set; }
    }
}
