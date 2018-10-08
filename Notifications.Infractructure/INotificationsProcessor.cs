using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notifications.Infractructure
{
    public interface INotificationsProcessor<T>
    {
        Task Process(IEnumerable<Notification<T>> notifications);
    }
}
