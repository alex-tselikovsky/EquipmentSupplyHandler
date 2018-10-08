namespace NotificationService.Interfaces
{
    public interface INotificator<T>
    {
        void Notify(string entityId, INotification<T>  notification);
    }
}
