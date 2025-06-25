using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acento.MVVM.Models;
using PropertyChanged;
using Acento.MVVM.Models.ChatSignalR;

namespace Acento.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class ChatDeEstudianteViewModel
    {

        public List<Acento.MVVM.Models.ChatSignalR.Messages> Messages { get; set; } = new List<Acento.MVVM.Models.ChatSignalR.Messages>();
        public ChatDeEstudianteViewModel()
        {
            Messages.Add(new Acento.MVVM.Models.ChatSignalR.Messages
            {
                Id = 1,
                senderUserName = "Estudiante1",
                senderUserId = "estudiante1",
                recipientUserName = "Profesor1",
                recipientUserId = "profesor1",
                Message = "Hola, ¿cómo estás?",
                CreatedAt = DateTime.Now,
                IsRead = true,
                isStudent = true,
                isTeacher = false,
                tareaId = 0,

            });
            Messages.Add(new Acento.MVVM.Models.ChatSignalR.Messages
            {
                Id = 2,
                senderUserName = "Profesor1",
                senderUserId = "profesor1",
                recipientUserName = "Estudiante1",
                recipientUserId = "estudiante1",
                Message = "Hola, estoy bien. ¿Y tú?",
                CreatedAt = DateTime.Now.AddMinutes(5),
                IsRead = false,
                isStudent = false,
                isTeacher = true,
                tareaId = 4
            });

            Messages.Add(new Acento.MVVM.Models.ChatSignalR.Messages
            {
                Id = 1,
                senderUserName = "Estudiante1",
                senderUserId = "estudiante1",
                recipientUserName = "Profesor1",
                recipientUserId = "profesor1",
                Message = "Hola, ¿cómo estás?",
                CreatedAt = DateTime.Now,
                IsRead = true,
                isStudent = true,
                isTeacher = false,
                tareaId = 0,

            });
            Messages.Add(new Acento.MVVM.Models.ChatSignalR.Messages
            {
                Id = 2,
                senderUserName = "Profesor1",
                senderUserId = "profesor1",
                recipientUserName = "Estudiante1",
                recipientUserId = "estudiante1",
                Message = "Hola, estoy bien. ¿Y tú?",
                CreatedAt = DateTime.Now.AddMinutes(5),
                IsRead = false,
                isStudent = false,
                isTeacher = true,
                tareaId = 4
            });
            Messages.Add(new Acento.MVVM.Models.ChatSignalR.Messages
            {
                Id = 1,
                senderUserName = "Estudiante1",
                senderUserId = "estudiante1",
                recipientUserName = "Profesor1",
                recipientUserId = "profesor1",
                Message = "Hola, ¿cómo estás?",
                CreatedAt = DateTime.Now,
                IsRead = true,
                isStudent = true,
                isTeacher = false,
                tareaId = 0,

            });
            Messages.Add(new Acento.MVVM.Models.ChatSignalR.Messages
            {
                Id = 2,
                senderUserName = "Profesor1",
                senderUserId = "profesor1",
                recipientUserName = "Estudiante1",
                recipientUserId = "estudiante1",
                Message = "Hola, estoy bien. ¿Y tú?",
                CreatedAt = DateTime.Now.AddMinutes(5),
                IsRead = false,
                isStudent = false,
                isTeacher = true,
                tareaId = 4
            });
            Messages.Add(new Acento.MVVM.Models.ChatSignalR.Messages
            {
                Id = 1,
                senderUserName = "Estudiante1",
                senderUserId = "estudiante1",
                recipientUserName = "Profesor1",
                recipientUserId = "profesor1",
                Message = "Hola, ¿cómo estás?",
                CreatedAt = DateTime.Now,
                IsRead = true,
                isStudent = true,
                isTeacher = false,
                tareaId = 0,

            });
            Messages.Add(new Acento.MVVM.Models.ChatSignalR.Messages
            {
                Id = 2,
                senderUserName = "Profesor1",
                senderUserId = "profesor1",
                recipientUserName = "Estudiante1",
                recipientUserId = "estudiante1",
                Message = "Hola, estoy bien. ¿Y tú?",
                CreatedAt = DateTime.Now.AddMinutes(5),
                IsRead = false,
                isStudent = false,
                isTeacher = true,
                tareaId = 4
            }); Messages.Add(new Acento.MVVM.Models.ChatSignalR.Messages
            {
                Id = 1,
                senderUserName = "Estudiante1",
                senderUserId = "estudiante1",
                recipientUserName = "Profesor1",
                recipientUserId = "profesor1",
                Message = "Hola, ¿cómo estás?",
                CreatedAt = DateTime.Now,
                IsRead = true,
                isStudent = true,
                isTeacher = false,
                tareaId = 0,

            });
            Messages.Add(new Acento.MVVM.Models.ChatSignalR.Messages
            {
                Id = 2,
                senderUserName = "Profesor1",
                senderUserId = "profesor1",
                recipientUserName = "Estudiante1",
                recipientUserId = "estudiante1",
                Message = "Hola, estoy bien. ¿Y tú?",
                CreatedAt = DateTime.Now.AddMinutes(5),
                IsRead = false,
                isStudent = false,
                isTeacher = true,
                tareaId = 4
            }); Messages.Add(new Acento.MVVM.Models.ChatSignalR.Messages
            {
                Id = 1,
                senderUserName = "Estudiante1",
                senderUserId = "estudiante1",
                recipientUserName = "Profesor1",
                recipientUserId = "profesor1",
                Message = "Hola, ¿cómo estás?",
                CreatedAt = DateTime.Now,
                IsRead = true,
                isStudent = true,
                isTeacher = false,
                tareaId = 0,

            });
            Messages.Add(new Acento.MVVM.Models.ChatSignalR.Messages
            {
                Id = 2,
                senderUserName = "Profesor1",
                senderUserId = "profesor1",
                recipientUserName = "Estudiante1",
                recipientUserId = "estudiante1",
                Message = "Hola, estoy bien. ¿Y tú?",
                CreatedAt = DateTime.Now.AddMinutes(5),
                IsRead = false,
                isStudent = false,
                isTeacher = true,
                tareaId = 4
            }); Messages.Add(new Acento.MVVM.Models.ChatSignalR.Messages
            {
                Id = 1,
                senderUserName = "Estudiante1",
                senderUserId = "estudiante1",
                recipientUserName = "Profesor1",
                recipientUserId = "profesor1",
                Message = "Hola, ¿cómo estás?",
                CreatedAt = DateTime.Now,
                IsRead = true,
                isStudent = true,
                isTeacher = false,
                tareaId = 0,

            });
            Messages.Add(new Acento.MVVM.Models.ChatSignalR.Messages
            {
                Id = 2,
                senderUserName = "Profesor1",
                senderUserId = "profesor1",
                recipientUserName = "Estudiante1",
                recipientUserId = "estudiante1",
                Message = "Hola, estoy bien. ¿Y tú?",
                CreatedAt = DateTime.Now.AddMinutes(5),
                IsRead = false,
                isStudent = false,
                isTeacher = true,
                tareaId = 4
            }); Messages.Add(new Acento.MVVM.Models.ChatSignalR.Messages
            {
                Id = 1,
                senderUserName = "Estudiante1",
                senderUserId = "estudiante1",
                recipientUserName = "Profesor1",
                recipientUserId = "profesor1",
                Message = "Hola, ¿cómo estás?",
                CreatedAt = DateTime.Now,
                IsRead = true,
                isStudent = true,
                isTeacher = false,
                tareaId = 0,

            });
            Messages.Add(new Acento.MVVM.Models.ChatSignalR.Messages
            {
                Id = 2,
                senderUserName = "Profesor1",
                senderUserId = "profesor1",
                recipientUserName = "Estudiante1",
                recipientUserId = "estudiante1",
                Message = "Hola, estoy bien. ¿Y tú?",
                CreatedAt = DateTime.Now.AddMinutes(5),
                IsRead = false,
                isStudent = false,
                isTeacher = true,
                tareaId = 4
            }); Messages.Add(new Acento.MVVM.Models.ChatSignalR.Messages
            {
                Id = 1,
                senderUserName = "Estudiante1",
                senderUserId = "estudiante1",
                recipientUserName = "Profesor1",
                recipientUserId = "profesor1",
                Message = "Hola, ¿cómo estás?",
                CreatedAt = DateTime.Now,
                IsRead = true,
                isStudent = true,
                isTeacher = false,
                tareaId = 0,

            });
            Messages.Add(new Acento.MVVM.Models.ChatSignalR.Messages
            {
                Id = 2,
                senderUserName = "Profesor1",
                senderUserId = "profesor1",
                recipientUserName = "Estudiante1",
                recipientUserId = "estudiante1",
                Message = "Hola, estoy bien. ¿Y tú?",
                CreatedAt = DateTime.Now.AddMinutes(5),
                IsRead = false,
                isStudent = false,
                isTeacher = true,
                tareaId = 4
            }); Messages.Add(new Acento.MVVM.Models.ChatSignalR.Messages
            {
                Id = 1,
                senderUserName = "Estudiante1",
                senderUserId = "estudiante1",
                recipientUserName = "Profesor1",
                recipientUserId = "profesor1",
                Message = "Hola, ¿cómo estás?",
                CreatedAt = DateTime.Now,
                IsRead = true,
                isStudent = true,
                isTeacher = false,
                tareaId = 0,

            });
            Messages.Add(new Acento.MVVM.Models.ChatSignalR.Messages
            {
                Id = 2,
                senderUserName = "Profesor1",
                senderUserId = "profesor1",
                recipientUserName = "Estudiante1",
                recipientUserId = "estudiante1",
                Message = "Hola, estoy bien. ¿Y tú?",
                CreatedAt = DateTime.Now.AddMinutes(5),
                IsRead = false,
                isStudent = false,
                isTeacher = true,
                tareaId = 4
            }); Messages.Add(new Acento.MVVM.Models.ChatSignalR.Messages
            {
                Id = 1,
                senderUserName = "Estudiante1",
                senderUserId = "estudiante1",
                recipientUserName = "Profesor1",
                recipientUserId = "profesor1",
                Message = "Hola, ¿cómo estás?",
                CreatedAt = DateTime.Now,
                IsRead = true,
                isStudent = true,
                isTeacher = false,
                tareaId = 0,

            });
            Messages.Add(new Acento.MVVM.Models.ChatSignalR.Messages
            {
                Id = 2,
                senderUserName = "Profesor1",
                senderUserId = "profesor1",
                recipientUserName = "Estudiante1",
                recipientUserId = "estudiante1",
                Message = "Hola, estoy bien. ¿Y tú?",
                CreatedAt = DateTime.Now.AddMinutes(5),
                IsRead = false,
                isStudent = false,
                isTeacher = true,
                tareaId = 4
            }); Messages.Add(new Acento.MVVM.Models.ChatSignalR.Messages
            {
                Id = 1,
                senderUserName = "Estudiante1",
                senderUserId = "estudiante1",
                recipientUserName = "Profesor1",
                recipientUserId = "profesor1",
                Message = "Hola, ¿cómo estás?",
                CreatedAt = DateTime.Now,
                IsRead = true,
                isStudent = true,
                isTeacher = false,
                tareaId = 0,

            });
            Messages.Add(new Acento.MVVM.Models.ChatSignalR.Messages
            {
                Id = 2,
                senderUserName = "Profesor1",
                senderUserId = "profesor1",
                recipientUserName = "Estudiante1",
                recipientUserId = "estudiante1",
                Message = "Hola, estoy bien. ¿Y tú?",
                CreatedAt = DateTime.Now.AddMinutes(5),
                IsRead = false,
                isStudent = false,
                isTeacher = true,
                tareaId = 4
            }); Messages.Add(new Acento.MVVM.Models.ChatSignalR.Messages
            {
                Id = 1,
                senderUserName = "Estudiante1",
                senderUserId = "estudiante1",
                recipientUserName = "Profesor1",
                recipientUserId = "profesor1",
                Message = "Hola, ¿cómo estás?",
                CreatedAt = DateTime.Now,
                IsRead = true,
                isStudent = true,
                isTeacher = false,
                tareaId = 0,

            });
            Messages.Add(new Acento.MVVM.Models.ChatSignalR.Messages
            {
                Id = 2,
                senderUserName = "Profesor1",
                senderUserId = "profesor1",
                recipientUserName = "Estudiante1",
                recipientUserId = "estudiante1",
                Message = "Hola, estoy bien. ¿Y tú?",
                CreatedAt = DateTime.Now.AddMinutes(5),
                IsRead = false,
                isStudent = false,
                isTeacher = true,
                tareaId = 4
            }); Messages.Add(new Acento.MVVM.Models.ChatSignalR.Messages
            {
                Id = 1,
                senderUserName = "Estudiante1",
                senderUserId = "estudiante1",
                recipientUserName = "Profesor1",
                recipientUserId = "profesor1",
                Message = "Hola, ¿cómo estás?",
                CreatedAt = DateTime.Now,
                IsRead = true,
                isStudent = true,
                isTeacher = false,
                tareaId = 0,

            });
            Messages.Add(new Acento.MVVM.Models.ChatSignalR.Messages
            {
                Id = 2,
                senderUserName = "Profesor1",
                senderUserId = "profesor1",
                recipientUserName = "Estudiante1",
                recipientUserId = "estudiante1",
                Message = "Hola, estoy bien. ¿Y tú?",
                CreatedAt = DateTime.Now.AddMinutes(5),
                IsRead = false,
                isStudent = false,
                isTeacher = true,
                tareaId = 4
            }); Messages.Add(new Acento.MVVM.Models.ChatSignalR.Messages
            {
                Id = 1,
                senderUserName = "Estudiante1",
                senderUserId = "estudiante1",
                recipientUserName = "Profesor1",
                recipientUserId = "profesor1",
                Message = "Hola, ¿cómo estás?",
                CreatedAt = DateTime.Now,
                IsRead = true,
                isStudent = true,
                isTeacher = false,
                tareaId = 0,

            });
            Messages.Add(new Acento.MVVM.Models.ChatSignalR.Messages
            {
                Id = 2,
                senderUserName = "Profesor1",
                senderUserId = "profesor1",
                recipientUserName = "Estudiante1",
                recipientUserId = "estudiante1",
                Message = "Hola, estoy bien. ¿Y tú?",
                CreatedAt = DateTime.Now.AddMinutes(5),
                IsRead = false,
                isStudent = false,
                isTeacher = true,
                tareaId = 4
            }); Messages.Add(new Acento.MVVM.Models.ChatSignalR.Messages
            {
                Id = 1,
                senderUserName = "Estudiante1",
                senderUserId = "estudiante1",
                recipientUserName = "Profesor1",
                recipientUserId = "profesor1",
                Message = "Hola, ¿cómo estás?",
                CreatedAt = DateTime.Now,
                IsRead = true,
                isStudent = true,
                isTeacher = false,
                tareaId = 0,

            });
            Messages.Add(new Acento.MVVM.Models.ChatSignalR.Messages
            {
                Id = 2,
                senderUserName = "Profesor1",
                senderUserId = "profesor1",
                recipientUserName = "Estudiante1",
                recipientUserId = "estudiante1",
                Message = "Hola, estoy bien. ¿Y tú?",
                CreatedAt = DateTime.Now.AddMinutes(5),
                IsRead = false,
                isStudent = false,
                isTeacher = true,
                tareaId = 4
            });


        }


    }
}
