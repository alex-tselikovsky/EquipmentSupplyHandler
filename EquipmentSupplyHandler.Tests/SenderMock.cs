using EquipmentSupplyHandler.Notifications;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EquipmentSupplyHandler.Tests
{
    public class SenderMock : INotificationSender
    {
        readonly int ExpectedMessagesCount;
        int CurrentCount = 0;
        public ConcurrentQueue<Message> Messages = new ConcurrentQueue<Message>();

        TaskCompletionSource<IEnumerable<Message>> taskCompletionSource = new TaskCompletionSource<IEnumerable<Message>>();

        public SenderMock(int expectedMessagesCount)
        {
            ExpectedMessagesCount = expectedMessagesCount;
        }
        public Task SendAsync(Message message)
        {
            Messages.Enqueue(message);
            if (Interlocked.Increment(ref CurrentCount) == ExpectedMessagesCount) taskCompletionSource.SetResult(Messages.ToArray());

            return Task.CompletedTask;
        }

        public Task<IEnumerable<Message>> GetMessagesAsync() => taskCompletionSource.Task;
    }

}
