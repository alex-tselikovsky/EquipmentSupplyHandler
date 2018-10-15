using EquipmentSupplyHandler.Controllers;
using EquipmentSupplyHandler.Notifications;
using ESHRepository.EF;
using ESHRepository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notifications.Infractructure;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EquipmentSupplyHandler.Tests
{
    public class NotificationUnitTest
    {
        [Fact]
        public async Task Three_events_send_as_two_messages()
        {
            //Настройка IOC контейнера
            var services = new ServiceCollection();
            var config = new ConfigurationBuilder().AddJsonFile("testProject.json").Build();
            var startup = new Startup(config);
            startup.ConfigNotificationProcessor(services);

            services.AddSingleton<INotificationSender, SenderMock>(sprovider=>new SenderMock(2));

            var serviceProvider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

        
            //In memory dbcontext
            services.AddDbContext<ESHContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
                options.UseInternalServiceProvider(serviceProvider);
            });
            services.AddScoped<IESHRepository, ESHRepository.EF.ESHRepository>();

            var sp = services.BuildServiceProvider();
            
            //получаем наш синглтон сендера
            var senderMock = (SenderMock)sp.GetService<INotificationSender>();

            //моделируем работу с поставками
            using (var scope = sp.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IESHRepository>();
                var notificator = scope.ServiceProvider.GetRequiredService<ConcurrentDictNotificator<SupplyOperation>>();
                DeliveryController deliveryController = new DeliveryController(repository, notificator);
                await deliveryController.Create(new ESHRepository.Model.Supply()
                {
                    Id = "1",
                    Count = 1
                });
                await deliveryController.Create(new ESHRepository.Model.Supply()
                {
                    Id = "2",
                    Count = 1
                });
            }
            using (var scope = sp.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IESHRepository>();
                var notificator = scope.ServiceProvider.GetRequiredService<ConcurrentDictNotificator<SupplyOperation>>();
                DeliveryController deliveryController = new DeliveryController(repository, notificator);
                await deliveryController.Update(new ESHRepository.Model.Supply()
                {
                    Id = "2",
                    Count = 1
                });

                //ждем получения сообщений
                var messages = await senderMock.GetMessagesAsync().WithTimeout(10000);
                Assert.Equal(2, messages.Count());
                Assert.True(messages.FirstOrDefault(m => m.Title.Contains("Информация по записи 2") && m.Body.Contains("Запись создана и обновлена")) != null);
                Assert.True(messages.FirstOrDefault(m => m.Title.Contains("Информация по записи 1") && m.Body.Contains("Запись создана")) != null);
            }
        }
    }
}
