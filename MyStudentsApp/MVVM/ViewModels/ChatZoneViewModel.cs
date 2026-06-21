using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        public bool IsConnected { get; set; }
        public bool IsSending { get; set; }
        public string StatusMessage { get; set; } = "Conectando...";
        
        private bool _eventosRegistrados = false;
        private string _currentRecipientId;

        public ChatZoneViewModel(ChatService chatService)
        {
            _ChatService = chatService;
            Mensajes = new ObservableCollection<MensajeResponseDTO>();
            Debug.WriteLine("[CHAT_VM] Constructor - ViewModel creado");
        }

        public async Task TraerMensajesAsync(string recipientId)
        {
            try
            {
                _currentRecipientId = recipientId;
                Debug.WriteLine($"[CHAT_VM] 📥 Trayendo mensajes para: {recipientId}");

                // CRÍTICO: Desuscribir eventos anteriores antes de suscribir nuevos
                DesuscribirEventos();

                // Suscribir a eventos
                SuscribirEventos();

                // Asegurar conexión
                await _ChatService.InitializeConnectionAsync();
                IsConnected = _ChatService.IsConnected;
                StatusMessage = IsConnected ? "En línea" : "Sin conexión. Reintentando al enviar.";

                // Solicitar mensajes
                await _ChatService.ObtenerAllMessages(recipientId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CHAT_VM] ❌ Error trayendo mensajes: {ex.Message}");
                IsConnected = false;
                StatusMessage = "No se pudo cargar el chat.";
            }
        }

        private void SuscribirEventos()
        {
            if (!_eventosRegistrados)
            {
                Debug.WriteLine("[CHAT_VM] 📡 Suscribiendo a eventos de chat");
                _ChatService.recibirMensajesHistorial += ChatService_RecibirMensajesHistorial;
                _ChatService.mensajeNuevo += ChatService_MensajeNuevo;
                _eventosRegistrados = true;
            }
        }

        private void DesuscribirEventos()
        {
            if (_eventosRegistrados)
            {
                Debug.WriteLine("[CHAT_VM] 🔇 Desuscribiendo eventos de chat");
                _ChatService.recibirMensajesHistorial -= ChatService_RecibirMensajesHistorial;
                _ChatService.mensajeNuevo -= ChatService_MensajeNuevo;
                _eventosRegistrados = false;
            }
        }

        private void ChatService_MensajeNuevo(MensajeResponseDTO mensajeNuevo)
        {
            try
            {
                Debug.WriteLine($"[CHAT_VM] 📨 Mensaje nuevo recibido de: {mensajeNuevo.senderNombre}");

                // CRÍTICO: Solo agregar el mensaje si pertenece a la conversación actual
                if ((mensajeNuevo.senderUsuarioId == _currentRecipientId || mensajeNuevo.recipientUsuarioId == _currentRecipientId))
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        // Evitar duplicados
                        if (!Mensajes.Any(m => m.messageId == mensajeNuevo.messageId))
                        {
                            Mensajes.Add(mensajeNuevo);
                            Debug.WriteLine($"[CHAT_VM] ✅ Mensaje agregado a la colección");
                        }
                        else
                        {
                            Debug.WriteLine($"[CHAT_VM] ⚠️ Mensaje duplicado ignorado");
                        }
                    });
                }
                else
                {
                    Debug.WriteLine($"[CHAT_VM] ⚠️ Mensaje ignorado - no pertenece a esta conversación");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CHAT_VM] ❌ Error procesando mensaje nuevo: {ex.Message}");
            }
        }

        private void ChatService_RecibirMensajesHistorial(MensajeResponseDTO[] mensajesResponse)
        {
            try
            {
                Debug.WriteLine($"[CHAT_VM] 📚 Historial recibido: {mensajesResponse.Length} mensajes");

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Mensajes.Clear();
                    foreach (var m in mensajesResponse.OrderBy(x => x.enviado))
                    {
                        Mensajes.Add(m);
                    }
                    Debug.WriteLine($"[CHAT_VM] ✅ Historial cargado en la colección");
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CHAT_VM] ❌ Error procesando historial: {ex.Message}");
            }
        }

        public async Task EnviarMensajeAsync(string recipientId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textoEscritoInput))
                {
                    Debug.WriteLine("[CHAT_VM] ⚠️ Texto vacío, no se envía");
                    return;
                }

                Debug.WriteLine($"[CHAT_VM] 📤 Enviando mensaje: {textoEscritoInput}");
                IsSending = true;
                StatusMessage = "Enviando...";
                await _ChatService.SendMessageTo(recipientId, textoEscritoInput);
                IsConnected = _ChatService.IsConnected;
                
                // Limpiar el campo de texto después de enviar
                textoEscritoInput = string.Empty;
                StatusMessage = IsConnected ? "En línea" : "Mensaje pendiente de conexión.";
                Debug.WriteLine("[CHAT_VM] ✅ Mensaje enviado y texto limpiado");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CHAT_VM] ❌ Error enviando mensaje: {ex.Message}");
                StatusMessage = "No se pudo enviar. Intenta de nuevo.";
            }
            finally
            {
                IsSending = false;
            }
        }

        // Método para limpiar cuando se sale del chat
        public void Cleanup()
        {
            Debug.WriteLine("[CHAT_VM] 🧹 Limpiando ViewModel");
            DesuscribirEventos();
            Mensajes.Clear();
            _currentRecipientId = null;
        }
    }
}
