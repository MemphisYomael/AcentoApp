namespace MyStudentsApp.Services.Notifications;

public interface INotificationNavigationService
{
    Task HandleNotificationOpenedAsync(IDictionary<string, object> data);
    void MarkShellReady();
}
