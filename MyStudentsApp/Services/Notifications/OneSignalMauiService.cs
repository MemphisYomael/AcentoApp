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

        OneSignal.Initialize(OneSignalMauiConfig.AppId);
        OneSignal.Notifications.Clicked += HandleNotificationClicked;
        _initialized = true;
        return Task.CompletedTask;
    }

    public Task LoginAsync(string usuarioId)
    {
        OneSignal.Login(usuarioId);
        return Task.CompletedTask;
    }

    public Task LogoutAsync()
    {
        OneSignal.Logout();
        return Task.CompletedTask;
    }

    public Task RequestPermissionAsync()
    {
        return OneSignal.Notifications.RequestPermissionAsync(true);
    }

    public Task<NotificationSnapshot> GetCurrentSnapshotAsync()
    {
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
