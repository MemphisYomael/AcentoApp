namespace MyStudentsApp.Services.Notifications;

public sealed class NotificationSnapshot
{
    public string? SubscriptionId { get; init; }
    public string? PushToken { get; init; }
}
