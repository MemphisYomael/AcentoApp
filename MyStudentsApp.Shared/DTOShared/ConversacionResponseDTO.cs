namespace MyStudentsApp.Shared.DTOShared
{
    public class ConversacionResponseDTO
    {
        public string? conversationId { get; set; }
        public string? otherUserId { get; set; }
        public string? otherUserName { get; set; }
        public string? otherUserRole { get; set; }
        public string? lastMessage { get; set; }
        public DateTime lastMessageAt { get; set; }
        public int unreadCount { get; set; }
    }
}
