using System.Threading.Tasks;

namespace EquipmentSupplyHandler.Notifications
{
    public interface INotificationSender
    {
        Task SendAsync(Message message);
    }
}
