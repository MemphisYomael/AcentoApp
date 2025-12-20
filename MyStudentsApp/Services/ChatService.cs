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
    public class ChatService
    {
        private HubConnection? _connection;
        public bool IsConnected => _connection?.State == HubConnectionState.Connected;

        public event Action<MensajeResponseDTO>? mensajeNuevo;
        public event Action<MensajeResponseDTO[]>? recibirMensajesHistorial;

        public ChatService()
        {
            Debug.WriteLine("[CHAT_SERVICE] Constructor - Inicializando servicio de chat");
        }

        public async Task InitializeConnectionAsync()
        {
            try
            {
                // Si ya hay una conexión activa, no crear otra
                if (_connection != null && (_connection.State == HubConnectionState.Connected || _connection.State == HubConnectionState.Connecting))
                {
                    Debug.WriteLine("[CHAT_SERVICE] ⚠️ Conexión ya existe y está activa/conectando");
                    return;
                }

                Debug.WriteLine("[CHAT_SERVICE] 🔌 Configurando nueva conexión SignalR");

                _connection = new HubConnectionBuilder()
                    .WithUrl("https://localhost:7224/hubs/chat", options =>
                    {
                        options.AccessTokenProvider = async () =>
                        {
                            try
                            {
                                string token = Preferences.Get("token", string.Empty);
                                Debug.WriteLine($"[CHAT_SERVICE] 🔑 Token obtenido: {(!string.IsNullOrEmpty(token) ? "✅" : "❌")}");
                                return token;
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"[CHAT_SERVICE] ❌ Error obteniendo token: {ex.Message}");
                                return string.Empty;
                            }
                        };
                    })
                    .WithAutomaticReconnect()
                    .Build();

                SetupHubEvents();
                SetupConnectionEvents();

                await StartConnectionAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CHAT_SERVICE] ❌ Error en InitializeConnectionAsync: {ex.Message}");
                // No lanzar la excepción para evitar que crashee la app
            }
        }

        private void SetupHubEvents()
        {
            try
            {
                Debug.WriteLine("[CHAT_SERVICE] 📡 Configurando eventos del Hub");

                // Importante: Remover handlers anteriores si existen para evitar duplicados
                _connection?.Remove("ReceiveMessage");
                _connection?.Remove("ReceivedHistoryChat");

                _connection?.On<MensajeResponseDTO>("ReceiveMessage", (message) =>
                {
                    Debug.WriteLine($"[CHAT_SERVICE] 📨 ReceiveMessage: {message.mensaje}");
                    mensajeNuevo?.Invoke(message);
                });

                _connection?.On<List<MensajeResponseDTO>>("ReceivedHistoryChat", (messages) =>
                {
                    Debug.WriteLine($"[CHAT_SERVICE] 📚 ReceivedHistoryChat: {messages.Count} mensajes");
                    recibirMensajesHistorial?.Invoke(messages.ToArray());
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CHAT_SERVICE] ❌ Error en SetupHubEvents: {ex.Message}");
            }
        }

        private void SetupConnectionEvents()
        {
            try
            {
                if (_connection == null) return;

                _connection.Reconnecting += error =>
                {
                    Debug.WriteLine($"[CHAT_SERVICE] 🔄 Reconectando... {error?.Message}");
                    return Task.CompletedTask;
                };

                _connection.Reconnected += connId =>
                {
                    Debug.WriteLine($"[CHAT_SERVICE] ✅ Reconectado: {connId}");
                    return Task.CompletedTask;
                };

                _connection.Closed += error =>
                {
                    Debug.WriteLine($"[CHAT_SERVICE] ❌ Conexión cerrada: {error?.Message}");
                    return Task.CompletedTask;
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CHAT_SERVICE] ❌ Error en SetupConnectionEvents: {ex.Message}");
            }
        }

        private async Task StartConnectionAsync()
        {
            try
            {
                if (_connection == null)
                {
                    Debug.WriteLine("[CHAT_SERVICE] ⚠️ Connection es null, no se puede iniciar");
                    return;
                }

                await _connection.StartAsync();
                Debug.WriteLine("[CHAT_SERVICE] ✅ Conexión iniciada correctamente");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CHAT_SERVICE] ❌ Error al iniciar conexión: {ex.Message}");
                // No lanzar la excepción para evitar que crashee la app
            }
        }

        public async Task<List<MensajeResponseDTO>> ObtenerAllMessages(string recipientId)
        {
            try
            {
                if (_connection == null || _connection.State != HubConnectionState.Connected)
                {
                    Debug.WriteLine("[CHAT_SERVICE] ⚠️ No hay conexión, intentando conectar...");
                    await InitializeConnectionAsync();
                }

                Debug.WriteLine($"[CHAT_SERVICE] 📥 Solicitando mensajes con: {recipientId}");
                await _connection.SendAsync("GetMessages", recipientId);

                return new List<MensajeResponseDTO>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CHAT_SERVICE] ❌ Error obteniendo mensajes: {ex.Message}");
                return new List<MensajeResponseDTO>();
            }
        }

        public async Task SendMessageTo(string recipientId, string message)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(message))
                {
                    Debug.WriteLine("[CHAT_SERVICE] ⚠️ Mensaje vacío, no se envía");
                    return;
                }

                if (_connection == null || _connection.State != HubConnectionState.Connected)
                {
                    Debug.WriteLine("[CHAT_SERVICE] ⚠️ No hay conexión, intentando conectar...");
                    await InitializeConnectionAsync();
                }

                Debug.WriteLine($"[CHAT_SERVICE] 📤 Enviando mensaje a: {recipientId}");
                await _connection?.SendAsync("SendMessage", recipientId, message);
                Debug.WriteLine("[CHAT_SERVICE] ✅ Mensaje enviado");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CHAT_SERVICE] ❌ Error enviando mensaje: {ex.Message}");
            }
        }

        public async Task DisconnectAsync()
        {
            try
            {
                if (_connection != null && _connection.State == HubConnectionState.Connected)
                {
                    Debug.WriteLine("[CHAT_SERVICE] 🔌 Desconectando SignalR...");
                    await _connection.StopAsync();
                    await _connection.DisposeAsync();
                    _connection = null;
                    Debug.WriteLine("[CHAT_SERVICE] ✅ Desconectado correctamente");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CHAT_SERVICE] ❌ Error al desconectar: {ex.Message}");
            }
        }

        // Método para limpiar eventos suscritos (importante para evitar memory leaks)
        public void ClearEventHandlers()
        {
            Debug.WriteLine("[CHAT_SERVICE] 🧹 Limpiando event handlers");
            mensajeNuevo = null;
            recibirMensajesHistorial = null;
        }
    }
}
