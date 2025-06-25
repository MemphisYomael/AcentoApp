
using Microsoft.AspNet.Identity.EntityFramework;

namespace Acento.MVVM.Models.Users
{
    public class Usuario : IdentityUser
    {
        public DateTime fechaCreacionCuenta { get; set; }
        public List<int>? Cursos { get; set; }
        public List<Courses>? cursos {  get; set; }
        public bool isStudent { get; set; }

        //SignalR
        //public List<UserLike>? LikedByOtherUsers { get; set; } = [];
        //public List<UserLike>? LikedUsers { get; set; } = [];

        //public List<Message>? messagesSent { get; set; } = [];
        //public List<Message>? messagesReceived { get; set; } = [];


    }
}