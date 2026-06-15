using Microsoft.Maui.ApplicationModel;

namespace MyStudentsApp.Services.Notifications;

public sealed class NotificationNavigationService : INotificationNavigationService
{
    private readonly SemaphoreSlim _gate = new(1, 1);
    private IDictionary<string, object>? _pendingData;
    private bool _shellReady;

    public async Task HandleNotificationOpenedAsync(IDictionary<string, object> data)
    {
        await _gate.WaitAsync();
        try
        {
            if (!_shellReady || Shell.Current == null)
            {
                _pendingData = new Dictionary<string, object>(data);
                return;
            }
        }
        finally
        {
            _gate.Release();
        }

        await NavigateAsync(data);
    }

    public void MarkShellReady()
    {
        _shellReady = true;
        _ = FlushPendingAsync();
    }

    private async Task FlushPendingAsync()
    {
        IDictionary<string, object>? pending = null;
        await _gate.WaitAsync();
        try
        {
            if (_shellReady && _pendingData != null)
            {
                pending = _pendingData;
                _pendingData = null;
            }
        }
        finally
        {
            _gate.Release();
        }

        if (pending != null)
        {
            await NavigateAsync(pending);
        }
    }

    private static async Task NavigateAsync(IDictionary<string, object> data)
    {
        var senderId = GetString(data, "senderId") ?? GetString(data, "conversationId");
        var senderName = GetString(data, "senderName") ?? "Nuevo mensaje";
        if (string.IsNullOrWhiteSpace(senderId) || Shell.Current == null)
        {
            return;
        }

        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await Shell.Current.GoToAsync("chatZone", new Dictionary<string, object>
            {
                ["nombre"] = senderName,
                ["usuarioId"] = senderId
            });
        });
    }

    private static string? GetString(IDictionary<string, object> data, string key)
    {
        return data.TryGetValue(key, out var value) ? value?.ToString() : null;
    }
}
