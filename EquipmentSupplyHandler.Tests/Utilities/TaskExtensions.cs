using System;
using System.Threading.Tasks;

namespace EquipmentSupplyHandler.Tests
{
    public static class TaskExtensions
    {
        public static async Task<T> WithTimeout<T>(this Task<T> task, int miliseconds)
        {
            Task delayTask = Task.Delay(miliseconds);
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
