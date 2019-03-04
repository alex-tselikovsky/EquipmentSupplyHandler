using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Notifications.Infractructure
{
    public class ConcurrentDelayedNotificator<T>:INotificator<T>
    {
        ConcurrentQueue<Notification<T>> Notifications { get; set; } = new ConcurrentQueue<Notification<T>>();
        TimeSpan Interval { get; set; }
        int InProgress = 0;
        DateTime lastTimerStart = DateTime.Now;
        INotificationsProcessor<T> NotificationProcessor { get; set; }

        public ConcurrentDelayedNotificator(DelayedNotificatorConfig config, INotificationsProcessor<T> notificationProcessor)
        {
            Interval = TimeSpan.FromSeconds(config.DelayInSeconds);
            NotificationProcessor = notificationProcessor;
        }

        public void AddNotification(T notification)
        {
            Notifications.Enqueue(new Notification<T>() { Entity = notification, Created = DateTime.Now });
            if (InProgress == 0)
                TryStartTimer();
        }

        /// <summary>
        /// Пробует захватить таймер
        /// </summary>
        void TryStartTimer()
        {
            if (Interlocked.Exchange(ref InProgress, 1) == 0)
                StartTimer();
        }
        void StartTimer()
        {
            lastTimerStart = DateTime.Now;
            Task.Delay((int)Interval.TotalMilliseconds).ContinueWith((task) => NotificationProcessor.Process(GetCurrentNotificationsPack()));
        }


        List<Notification<T>> GetCurrentNotificationsPack()
        {
            List<Notification<T>> notificationsPart = new List<Notification<T>>();

            while (Notifications.TryDequeue(out Notification<T> currentOperation))
            {
                //Складываем сообщение
                notificationsPart.Add(currentOperation);

                //Если уже прошло время последнего интервала, заканчиваем обработку и запускаем новый таймер для следующей порции событий
                if (currentOperation.Created.Subtract(lastTimerStart) > Interval)
                {
                    StartTimer();
                    return notificationsPart;
                }
            }


            //В очереди не было сообщений - отпускаем таймер
            InProgress = 0;

            //В очередь данного таймера уже не летят сообщения, но они там могут быть, попав до сброса InProgress
            //Проверяем не прилетело ли к нам еще чего
            if (Notifications.TryDequeue(out Notification<T> nextOperation))
            {
                //Если сообщение извлечено, 
                //Значит либо к нашему таймеру прилетели сообщения, либо запустился новый таймер

                //запустим таймер, если он не был запущен
                TryStartTimer();
                //Складываем сообщения
                notificationsPart.Add(nextOperation);
            }

            return notificationsPart;
        }

    }

}
