using Notifications.Infractructure;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Notification.GopTest
{
  
    
    public class NotificationProcessor : INotificationsProcessor<int>
    {
        public ConcurrentQueue<Notification<int>> q = new ConcurrentQueue<Notification<int>>();
        public Task Process(IEnumerable<Notification<int>> notifications)
        {
            foreach (var not in notifications)
                q.Enqueue(not);
            return Task.CompletedTask;
        }
    }


    class Program2
    {
        static void Main(string[] args)
        {
            List<Task> tasks = new List<Task>();
            NotificationProcessor notificationProcessor = new NotificationProcessor();
            ConcurrentDelayedNotificator<int> notificator = new ConcurrentDelayedNotificator<int>(new DelayedNotificatorConfig() { DelayInSeconds = 5 }, notificationProcessor);
      
                for (int threadNumber = 0; threadNumber < 2000; threadNumber++)
                {
                    int tNum = threadNumber;
                    tasks.Add(Task.Run(() =>
                    {
                        notificator.AddNotification(tNum);
                    }));
                    Thread.Sleep(1);
                }
            Task.WaitAll(tasks.ToArray());
            Thread.Sleep(2000);
            Console.WriteLine(notificationProcessor.q.Count);
            Console.ReadKey();
        }
    }
}
