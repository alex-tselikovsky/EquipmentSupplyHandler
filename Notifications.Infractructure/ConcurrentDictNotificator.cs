using Notifications.Infractructure.ConfigModel;
using System;
using System.Collections.Concurrent;

namespace Notifications.Infractructure
{
    /// <summary>
    /// Группирует сообщения по идентификаторам. ДЛя каждого идентификатора, свой собственный нотификатор 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConcurrentDictNotificator<T>
    {
        ConcurrentDictionary<string, INotificator<T>> Notificators { get; set; } = new ConcurrentDictionary<string, INotificator<T>>();
        Func<INotificator<T>> PrivateNotificatorFabric{ get; set; }

        public ConcurrentDictNotificator(DictNotificatorConfig<T> config)
        {
            PrivateNotificatorFabric = config.PrivateNotificatorFabric;
        }

        public void Notify(string entityId, T notification)
        {
            var operations = Notificators.GetOrAdd(entityId, PrivateNotificatorFabric());
            operations.AddNotification(notification);
        }

    }
}
