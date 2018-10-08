using ESHRepository.Model;
using System;

namespace NotificationService.Interfaces
{
    public class INotification<T> 
    {
        public T Entity { get; }
        public DateTime Created { get; }
    }   
}
