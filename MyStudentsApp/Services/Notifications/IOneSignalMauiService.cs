namespace MyStudentsApp.Services.Notifications;

public interface IOneSignalMauiService
{
    Task InitializeAsync();
    Task LoginAsync(string usuarioId);
    Task LogoutAsync();
    Task RequestPermissionAsync();
    Task<NotificationSnapshot> GetCurrentSnapshotAsync();
}
