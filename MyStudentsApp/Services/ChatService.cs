using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using MyStudentsApp.Shared.DTOShared;
using PropertyChanged;
using Microsoft.AspNetCore.SignalR.Client;
namespace MyStudentsApp.Services
{
    // Ya no necesita la lógica del patrón Singleton
    // usando MyStudentsApp.Shared.DTOShared (tu DTO compartido)
    public class ChatService
    {
        private HubConnection _connection;
        public bool IsConnected => _connection?.State == HubConnectionState.Connected;

        public event Action<MensajeResponseDTO>? mensajeNuevo;
        public event Action<MensajeResponseDTO[]>? recibirMensajesHistorial;

        public ChatService()
        {
            Debug.WriteLine("[CONEXIONES_HUB] 💡 Constructor ChatService.");
            // no await en constructor; exponer StartAsync para iniciar la conexión desde fuera si hace falta
            InitializeConnection();
        }

        private void InitializeConnection()
        {
            Debug.WriteLine("[CONEXIONES_HUB] Configurando conexión SignalR");

            _connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7224/hubs/chat", options =>
                {
                    // mejor patrón: AccessTokenProvider
                    options.AccessTokenProvider = async () =>
                    {
                        string token = Preferences.Get("token", string.Empty);
                        // Preferences.Get es de Xamarin/MAUI; adapta según plataforma
                        return token;
                    };
                })
                .WithAutomaticReconnect()
                .Build();

            // registrar eventos que vienen del servidor
            SetupHubEvents();

            // arrancar la conexión (no bloqueante)
            _ = StartConnectionAsync(); // puedes guardar la Task si quieres esperar en otro sitio

            // reconnection events
            _connection.Reconnecting += error =>
            {
                Debug.WriteLine($"[SIGNALR_CONEXION] Reconectando... {error?.Message}");
                return Task.CompletedTask;
            };
            _connection.Reconnected += connId =>
            {
                Debug.WriteLine($"[SIGNALR_CONEXION] Reconectado: {connId}");
                return Task.CompletedTask;
            };
            _connection.Closed += error =>
            {
                Debug.WriteLine($"[SIGNALR_CONEXION] Cerrado: {error?.Message}");
                return Task.CompletedTask;
            };
        }

        private void SetupHubEvents()
        {
            // cuando el servidor hace: Clients.Client(...).SendAsync("ReceiveMessage", message);
            _connection.On<MensajeResponseDTO>("ReceiveMessage", (message) =>
            {
                Debug.WriteLine("[SIGNALR_EVENT] ReceiveMessage: " + message);
                // construir un DTO mínimo o emitir solo el string. Aquí emitimos evento con DTO simple:
                MensajeResponseDTO dto = message;
                mensajeNuevo?.Invoke(dto);
            });

            // cuando el servidor hace: Clients.Caller.SendAsync("ReceivedHistoryChat", messageDtos);
            // si tu DTO es List<MensajeResponseDTO> o MensajeResponseDTO[] puedes elegir:
            _connection.On<List<MensajeResponseDTO>>("ReceivedHistoryChat", (messages) =>
            {
                Debug.WriteLine("[SIGNALR_EVENT] ReceivedHistoryChat count: " + messages.Count);
                recibirMensajesHistorial?.Invoke(messages.ToArray());
            });

            // si el servidor pudiera enviar arrays en vez de listas:
            _connection.On<MensajeResponseDTO[]>("ReceivedHistoryChat", (messages) =>
            {
                Debug.WriteLine("[SIGNALR_EVENT] ReceivedHistoryChat (array) count: " + messages.Length);
                recibirMensajesHistorial?.Invoke(messages);
            });
        }

        private async Task StartConnectionAsync()
        {
            try
            {
                await _connection.StartAsync();
                Debug.WriteLine("[SIGNALR_CONEXION] Conexión iniciada correctamente");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[SIGNALR_CONEXION] Error al iniciar: " + ex.Message);
            }
        }

        // Invocar método GetMessages que en el Hub actual no devuelve valor directo,
        // pero en tu Hub GetMessages manda el historial via Clients.Caller.SendAsync("ReceivedHistoryChat", messageDtos);
        // Si quieres obtener directamente la lista como respuesta, modifica el Hub para return messageDtos.
        // Aquí llamas y esperas que el servidor envíe el evento "ReceivedHistoryChat".
        public async Task<List<MensajeResponseDTO>> obtenerAllMessages(string recipientId)
        {
            if (_connection.State != HubConnectionState.Connected)
                await StartConnectionAsync();

            // Opciones:
            // 1) Si en el Hub GetMessages simplemente envía ReceivedHistoryChat, invócala y recibe el evento que ya registraste.
            await _connection.SendAsync("GetMessages", recipientId);

            // 2) Si prefieres que el Hub devuelva la lista directamente, cambia el Hub a:
            //    public async Task<List<MensajeResponseDTO>> GetMessages(string recipientId) { ... return messageDtos; }
            // y aquí harías:
            // return await _connection.InvokeAsync<List<MensajeResponseDTO>>("GetMessages", recipientId);

            // Como en tu Hub actual la lista llega por evento, devolvemos vacío o manejar mediante evento.
            return new List<MensajeResponseDTO>(); // o null; preferible usar el evento recibirMensajesHistorial
        }

        // Para enviar mensaje al Hub (SendMessage en tu Hub)
        public async Task SendMessageTo(string recipientId, string message)
        {
            if (_connection.State != HubConnectionState.Connected)
                await StartConnectionAsync();

            try
            {
                // SendAsync ya está bien para métodos Task sin resultado
                await _connection.SendAsync("SendMessage", recipientId, message);
                Debug.WriteLine("[SIGNALR] Mensaje enviado al Hub");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[SIGNALR] Error enviando mensaje: " + ex.Message);
                throw;
            }
        }
    }

}
