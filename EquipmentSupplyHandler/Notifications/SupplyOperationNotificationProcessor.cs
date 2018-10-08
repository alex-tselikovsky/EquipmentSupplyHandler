using Notifications.Infractructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EquipmentSupplyHandler.Notifications
{
    public class SupplyOperationNotificationProcessor : INotificationsProcessor<SupplyOperation>
    {
        static readonly Dictionary<Operation, string> operationsDescriptions = new Dictionary<Operation, string>(){
            {Operation.Created,"Запись создана"},
            {Operation.Updated,"Запись обновлена"},
            {Operation.Deleted,"Запись удалена"},
            {Operation.CreatedAndDeleted,"Запись создана и удалена"},
            {Operation.CreatedAndUpdated,"Запись создана и обновлена"},
            {Operation.CreatedUpdatedAndDeleted,"Запись создана и удалена"},
        };
      
        public INotificationSender Sender { get; set; }
        public SupplyOperationNotificationProcessor(INotificationSender sender)
        {
            Sender = sender;
        }

        public Task Process(IEnumerable<Notification<SupplyOperation>> supplies)
        {
            var id = supplies.First().Entity.Supply.Id;
            Operation operations = supplies.Aggregate(new Operation(), (prev, notification) => prev | notification.Entity.Operation);

            DateTime to = supplies.Max(t => t.Created);
            DateTime from = supplies.Min(t => t.Created);

            var body = $"Информация по записи {id} за период с {from} по {to}. {operationsDescriptions[operations]}";
            var title = $"Информация по записи {id}";

            return Sender.SendAsync(new Message()
            {
                Title = title,
                Body = body
            });
        }
    }
}
