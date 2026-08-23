namespace MyStudentsApp.Services.Notifications;

public interface INotificationDeviceService
{
    Task RegistrarDispositivoAsync(string usuarioId);
    Task DesactivarDispositivoActualAsync();
}
