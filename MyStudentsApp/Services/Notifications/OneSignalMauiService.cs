using System.Reflection;
using OneSignalSDK.DotNet;
using OneSignalSDK.DotNet.Core;
using OneSignalSDK.DotNet.Core.Notifications;
using MyStudentsApp.Configuration;

namespace MyStudentsApp.Services.Notifications;

public sealed class OneSignalMauiService : IOneSignalMauiService
{
    private readonly INotificationNavigationService _notificationNavigationService;
    private bool _initialized;
    private bool _isAvailable;

    public OneSignalMauiService(INotificationNavigationService notificationNavigationService)
    {
        _notificationNavigationService = notificationNavigationService;
    }

    public Task InitializeAsync()
    {
        if (_initialized)
        {
            return Task.CompletedTask;
        }

        _initialized = true;
#if WINDOWS
        _isAvailable = false;
        System.Diagnostics.Debug.WriteLine("[OneSignalMauiService] OneSignal deshabilitado en Windows.");
        return Task.CompletedTask;
#else
        try
        {
            OneSignal.Initialize(OneSignalMauiConfig.AppId);
            OneSignal.Notifications.Clicked += HandleNotificationClicked;
            _isAvailable = true;
        }
        catch (Exception ex)
        {
            _isAvailable = false;
            System.Diagnostics.Debug.WriteLine($"[OneSignalMauiService] No se pudo inicializar OneSignal: {ex}");
        }

        return Task.CompletedTask;
#endif
    }

    public Task LoginAsync(string usuarioId)
    {
        if (!_isAvailable)
        {
            return Task.CompletedTask;
        }

        OneSignal.Login(usuarioId);
        return Task.CompletedTask;
    }

    public Task LogoutAsync()
    {
        if (!_isAvailable)
        {
            return Task.CompletedTask;
        }

        OneSignal.Logout();
        return Task.CompletedTask;
    }

    public Task RequestPermissionAsync()
    {
        if (!_isAvailable)
        {
            return Task.CompletedTask;
        }

        return OneSignal.Notifications.RequestPermissionAsync(true);
    }

    public Task<NotificationSnapshot> GetCurrentSnapshotAsync()
    {
        if (!_isAvailable)
        {
            return Task.FromResult(new NotificationSnapshot());
        }

        var user = typeof(OneSignal).GetProperty("User", BindingFlags.Public | BindingFlags.Static)?.GetValue(null);
        var pushSubscription = user?.GetType().GetProperty("PushSubscription", BindingFlags.Public | BindingFlags.Instance)
            ?.GetValue(user)
            ?? user?.GetType().GetProperty("pushSubscription", BindingFlags.Public | BindingFlags.Instance)?.GetValue(user);

        var subscriptionId = pushSubscription?.GetType().GetProperty("Id", BindingFlags.Public | BindingFlags.Instance)?.GetValue(pushSubscription)?.ToString()
            ?? pushSubscription?.GetType().GetProperty("id", BindingFlags.Public | BindingFlags.Instance)?.GetValue(pushSubscription)?.ToString();

        var pushToken = pushSubscription?.GetType().GetProperty("Token", BindingFlags.Public | BindingFlags.Instance)?.GetValue(pushSubscription)?.ToString()
            ?? pushSubscription?.GetType().GetProperty("token", BindingFlags.Public | BindingFlags.Instance)?.GetValue(pushSubscription)?.ToString();

        return Task.FromResult(new NotificationSnapshot
        {
            SubscriptionId = subscriptionId,
            PushToken = pushToken
        });
    }

    private async void HandleNotificationClicked(object? sender, NotificationClickedEventArgs eventArgs)
    {
        try
        {
            var data = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            if (eventArgs.Notification?.AdditionalData != null)
            {
                foreach (var pair in eventArgs.Notification.AdditionalData)
                {
                    data[pair.Key] = pair.Value ?? string.Empty;
                }
            }

            var result = eventArgs.Result;
            if (!string.IsNullOrWhiteSpace(result?.Url))
            {
                data["url"] = result.Url;
            }

            if (!string.IsNullOrWhiteSpace(result?.ActionId))
            {
                data["actionId"] = result.ActionId;
            }

            await _notificationNavigationService.HandleNotificationOpenedAsync(data);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[OneSignalMauiService] Error handling notification click: {ex.Message}");
        }
    }
}
