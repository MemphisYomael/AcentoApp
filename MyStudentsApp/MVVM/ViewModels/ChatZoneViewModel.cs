using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MyStudentsApp.Services;
using MyStudentsApp.Shared.DTOShared;
using PropertyChanged;

namespace MyStudentsApp.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class ChatZoneViewModel
    {
        public ObservableCollection<MensajeResponseDTO> Mensajes { get; set; }
        public ChatService _ChatService { get; set; }

        public string textoEscritoInput { get; set; }

        public ChatZoneViewModel(ChatService chatService)
        {
            _ChatService = chatService;
            Mensajes = new ObservableCollection<MensajeResponseDTO>
            {
                new MensajeResponseDTO
                {
                    messageId = 1,
                    senderNombre = "Juan Perez",
                    recipientNombre = "Maria Lopez",
                    senderUsuarioId = "user1",
                    recipientUsuarioId = "user2",
                    mensaje = "Hola Memphis, ¿cómo estás?",
                    enviado = DateTime.Now,
                    IsRead = false
                },
                new MensajeResponseDTO
                {
                    messageId = 2,
                    senderNombre = "Memphis Yomael",
                    recipientNombre = "Juan Perez",
                    senderUsuarioId = "user2",
                    recipientUsuarioId = "user1",
                    mensaje = "Hola Juan, estoy bien, ¿y tú?",
                    enviado = DateTime.Now.AddMinutes(1),
                    IsRead = false
                },
                new MensajeResponseDTO
                {
                    messageId = 1,
                    senderNombre = "Juan Perez",
                    recipientNombre = "Maria Lopez",
                    senderUsuarioId = "user1",
                    recipientUsuarioId = "user2",
                    mensaje = "Hola Memphis, ¿cómo estás?",
                    enviado = DateTime.Now,
                    IsRead = false
                },
                new MensajeResponseDTO
                {
                    messageId = 2,
                    senderNombre = "Memphis Yomael",
                    recipientNombre = "Juan Perez",
                    senderUsuarioId = "user2",
                    recipientUsuarioId = "user1",
                    mensaje = "Hola Juan, estoy bien, ¿y tú?",
                    enviado = DateTime.Now.AddMinutes(1),
                    IsRead = false
                },
                new MensajeResponseDTO
                {
                    messageId = 1,
                    senderNombre = "Juan Perez",
                    recipientNombre = "Maria Lopez",
                    senderUsuarioId = "user1",
                    recipientUsuarioId = "user2",
                    mensaje = "Hola Memphis, ¿cómo estás?",
                    enviado = DateTime.Now,
                    IsRead = false
                },
                new MensajeResponseDTO
                {
                    messageId = 2,
                    senderNombre = "Memphis Yomael",
                    recipientNombre = "Juan Perez",
                    senderUsuarioId = "user2",
                    recipientUsuarioId = "user1",
                    mensaje = "Hola Juan, estoy bien, ¿y tú?",
                    enviado = DateTime.Now.AddMinutes(1),
                    IsRead = false
                },
                new MensajeResponseDTO
                {
                    messageId = 1,
                    senderNombre = "Juan Perez",
                    recipientNombre = "Maria Lopez",
                    senderUsuarioId = "user1",
                    recipientUsuarioId = "user2",
                    mensaje = "Hola Memphis, ¿cómo estás?",
                    enviado = DateTime.Now,
                    IsRead = false
                },
                new MensajeResponseDTO
                {
                    messageId = 2,
                    senderNombre = "Memphis Yomael",
                    recipientNombre = "Juan Perez",
                    senderUsuarioId = "user2",
                    recipientUsuarioId = "user1",
                    mensaje = "Hola Juan, estoy bien, ¿y tú?",
                    enviado = DateTime.Now.AddMinutes(1),
                    IsRead = false
                },
                new MensajeResponseDTO
                {
                    messageId = 1,
                    senderNombre = "Juan Perez",
                    recipientNombre = "Maria Lopez",
                    senderUsuarioId = "user1",
                    recipientUsuarioId = "user2",
                    mensaje = "Hola Memphis, ¿cómo estás?",
                    enviado = DateTime.Now,
                    IsRead = false
                },
                new MensajeResponseDTO
                {
                    messageId = 2,
                    senderNombre = "Memphis Yomael",
                    recipientNombre = "Juan Perez",
                    senderUsuarioId = "user2",
                    recipientUsuarioId = "user1",
                    mensaje = "Hola Juan, estoy bien, ¿y tú?",
                    enviado = DateTime.Now.AddMinutes(1),
                    IsRead = false
                }
            };

        }

        public async Task TraerMensajesAsync(string recipientId)
        {
            _ChatService.recibirMensajesHistorial += _ChatService_recibirMensajesHistorial;
            _ChatService.mensajeNuevo += _ChatService_mensajeNuevo;
            await _ChatService.obtenerAllMessages(recipientId);
        }

        private void _ChatService_mensajeNuevo(MensajeResponseDTO mensajeNuevo)
        {
            Mensajes.Add(mensajeNuevo);
        }

        private void _ChatService_recibirMensajesHistorial(MensajeResponseDTO[] mensajesResponse)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Mensajes.Clear();
                foreach (var m in mensajesResponse.OrderBy(x => x.enviado))
                    Mensajes.Add(m);
            });
        }

        public async Task EnviarMensajeAsync(string recipientId)
        {
            await _ChatService.SendMessageTo(recipientId, textoEscritoInput);
        }
    }
}
