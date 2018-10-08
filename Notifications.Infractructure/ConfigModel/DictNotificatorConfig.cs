using System;

namespace Notifications.Infractructure.ConfigModel
{
    public class DictNotificatorConfig<T>
    {
       public  Func<INotificator<T>> PrivateNotificatorFabric { get; set; }
    }
}
