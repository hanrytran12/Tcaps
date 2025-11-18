namespace Application.Interfaces
{
    public interface INotificationRealtimeService
    {
        Task SendNotificationToGroupAsync(string groupName, string eventName, object payload);
    }
}
