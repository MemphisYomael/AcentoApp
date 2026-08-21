using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Maui.Storage;
using MyStudentsApp.Shared.Services;

namespace MyStudentsApp.Services.Notifications;

public sealed class NotificationDeviceService : INotificationDeviceService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    private readonly HttpClient _httpClient;
    private readonly IOneSignalMauiService _oneSignalMauiService;

    public NotificationDeviceService(IOneSignalMauiService oneSignalMauiService)
    {
        _oneSignalMauiService = oneSignalMauiService;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri($"{ApiEndpoints.BaseUrl}/api/notificaciones/")
        };
    }

    public async Task RegistrarDispositivoAsync(string usuarioId)
    {
        var snapshot = await _oneSignalMauiService.GetCurrentSnapshotAsync();

        var request = new
        {
            usuarioID = usuarioId,
            platform = GetPlatformName(),
            oneSignalExternalUserId = usuarioId,
            oneSignalSubscriptionId = snapshot.SubscriptionId,
            oneSignalPushToken = snapshot.PushToken,
            deviceId = await GetOrCreateLocalDeviceIdAsync(),
            deviceName = DeviceInfo.Name,
            deviceModel = DeviceInfo.Model,
            manufacturer = DeviceInfo.Manufacturer,
            appVersion = AppInfo.VersionString,
            buildNumber = AppInfo.BuildString,
            osVersion = DeviceInfo.VersionString,
            idioma = CultureInfo.CurrentCulture.TwoLetterISOLanguageName,
            timeZone = TimeZoneInfo.Local.Id,
            isPushEnabled = true,
            isSubscribed = true
        };

        await SendAsync("dispositivos/registrar", request);
    }

    public async Task DesactivarDispositivoActualAsync()
    {
        var snapshot = await _oneSignalMauiService.GetCurrentSnapshotAsync();
        var request = new
        {
            oneSignalSubscriptionId = snapshot.SubscriptionId,
            deviceId = await GetOrCreateLocalDeviceIdAsync()
        };

        await SendAsync("dispositivos/desactivar", request);
    }

    private async Task SendAsync(string relativeUrl, object payload)
    {
        var token = Preferences.Get("token", string.Empty);
        if (!string.IsNullOrWhiteSpace(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        using var content = new StringContent(JsonSerializer.Serialize(payload, JsonOptions), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(relativeUrl, content);
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"[NotificationDeviceService] {response.StatusCode} {body}");
        }
    }

    private static string GetPlatformName()
    {
        if (DeviceInfo.Platform == DevicePlatform.iOS)
        {
            return "iOS";
        }

        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            return "Android";
        }

        return DeviceInfo.Platform.ToString();
    }

    private static async Task<string> GetOrCreateLocalDeviceIdAsync()
    {
        const string storageKey = "notification_device_id";
        var existing = await SecureStorage.Default.GetAsync(storageKey);
        if (!string.IsNullOrWhiteSpace(existing))
        {
            return existing;
        }

        var generated = Guid.NewGuid().ToString("N");
        await SecureStorage.Default.SetAsync(storageKey, generated);
        return generated;
    }
}
