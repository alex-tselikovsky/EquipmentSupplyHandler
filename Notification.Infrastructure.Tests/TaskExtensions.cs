using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Infrastructure.Tests
{
    public static class TaskExtensions
    {
        public static async Task<T> WithTimeout<T>(this Task<T> task, int time)
        {
            Task delayTask = Task.Delay(time);
            Task firstToFinish = await Task.WhenAny(task, delayTask);
            if (firstToFinish == delayTask)
            {
                // Первой закончилась задача задержки – разобраться с исключениями
                task.ContinueWith(HandleException);
                throw new TimeoutException();
            }
            // Если мы дошли до этого места, исходная задача уже завершилась
            return await task;
        }
        private static void HandleException<T>(Task<T> task)
        {
            if (task.Exception != null)
            {
                // logging.LogException(task.Exception);
            }
        }
    }
}
