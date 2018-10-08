using System;

namespace Notifications.Infractructure
{
    public class Notification<T> 
    {
        public T Entity { get; set; }
        public DateTime Created { get; set; }
    }   
}
