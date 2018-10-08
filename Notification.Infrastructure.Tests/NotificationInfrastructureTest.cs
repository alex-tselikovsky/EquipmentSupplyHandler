using Notifications.Infractructure;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Notification.Infrastructure.Tests
{
   
    public class NotificationProcessor : INotificationsProcessor<int>
    {
        public ConcurrentQueue<Notification<int>> q = new ConcurrentQueue<Notification<int>>();
        int waitedCount;
        int currentCount = 0;
        TaskCompletionSource<int> taskCompletionSource = new TaskCompletionSource<int>();
        public NotificationProcessor(int waitedCount)
        {
            this.waitedCount = waitedCount;
        }

        public Task Process(IEnumerable<Notification<int>> notifications)
        {

            foreach (var not in notifications)
            {
                q.Enqueue(not);
                if (Interlocked.Increment(ref currentCount) == waitedCount) taskCompletionSource.SetResult(waitedCount);
            }
            return Task.CompletedTask;
        }

        public Task<int> Complete => taskCompletionSource.Task;
    }


    public class NotificationInfrastructureTest
    {
        [Fact]
        public async Task Test1()
        {
            List<Task> gtasks = new List<Task>();
            for (int i = 0; i < 10000; i++)
            {
                gtasks.Add(Task.Run(async () =>
                {
                    int messageCount = 1000;
                    List<Task> tasks = new List<Task>();
                    NotificationProcessor notificationProcessor = new NotificationProcessor(messageCount);
                    ConcurrentDelayedNotificator<int> notificator = new ConcurrentDelayedNotificator<int>(new DelayedNotificatorConfig() { DelayInSeconds = 5 }, notificationProcessor);

                    for (int threadNumber = 0; threadNumber < messageCount; threadNumber++)
                    {
                        int tNum = threadNumber;
                        tasks.Add(Task.Run(() =>
                        {
                            notificator.AddNotification(tNum);
                        }));
                        await Task.Delay(1);
                    }
                    await Task.WhenAll(tasks.ToArray());
                    try
                    {
                        var receviedMessagesCount = await notificationProcessor.Complete.WithTimeout(50000);
                        Assert.Equal(messageCount, receviedMessagesCount);
                        var orderedMessages = notificationProcessor.q.Select(x => x.Entity).OrderBy(x => x);
                        Assert.Equal<int>(Enumerable.Range(0, messageCount), orderedMessages);
                    }
                    catch (Exception ex)
                    {
                        Assert.True(false);
                    }

                }));
            }
            await Task.WhenAll(gtasks.ToArray());
        }
    }
}
