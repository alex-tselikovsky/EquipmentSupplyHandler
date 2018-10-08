namespace Notifications.Infractructure
{
    public interface INotificator<T>
    {
        void AddNotification(T notification);
    }
}
