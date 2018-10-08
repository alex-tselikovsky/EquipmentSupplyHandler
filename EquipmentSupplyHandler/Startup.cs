using EquipmentSupplyHandler.ConfigModel;
using EquipmentSupplyHandler.Notifications;
using ESHRepository.EF;
using ESHRepository.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Notifications.Infractructure;
using Notifications.Infractructure.ConfigModel;

namespace EquipmentSupplyHandler
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigDBContext(services);
            ConfigRepository(services);
            //Нотификации
            ConfigNotificationProcessor(services);

            ConfigSender(services);

            services.AddMvc();
        }

        public virtual void ConfigDBContext(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ESHContext>(options => options.UseSqlServer(connectionString));
        }
        public virtual void ConfigNotificationProcessor(IServiceCollection services)
        {
            services.Configure<DelayedNotificatorConfig>(Configuration.GetSection("DelayedNotificatorConfig"));
            services.AddSingleton(sp => sp.GetService<IOptions<DelayedNotificatorConfig>>().Value);
            services.AddSingleton<ConcurrentDictNotificator<SupplyOperation>>(); //Синглтон нотификатора c разделением. Его зависимости определяются ниже
            services.AddTransient<ConcurrentDelayedNotificator<SupplyOperation>>();//для каждого идентификатора создается собственный экземпляр (фабрика определяется ниже)
            services.AddSingleton(sp => new DictNotificatorConfig<SupplyOperation>()
            {
                PrivateNotificatorFabric = sp.GetService<ConcurrentDelayedNotificator<SupplyOperation>>
            });

            //Реализация процессора обработки для экономии ресурсов позволяет использовать синглтон. 
            //Для других реализаций возможно потребуется другой тип завиисмоти
            services.AddSingleton<INotificationsProcessor<SupplyOperation>, SupplyOperationNotificationProcessor>();
        }

        public virtual void ConfigSender(IServiceCollection services)
        {
            services.Configure<EmailConfig>(Configuration.GetSection("EmailSettings"));
            services.AddSingleton(sp => sp.GetService<IOptions<EmailConfig>>().Value);
            services.AddSingleton<INotificationSender, EmailNotificationSender>();
        }

        public virtual void ConfigRepository(IServiceCollection services)
        {
            services.AddScoped<IESHRepository, ESHRepository.EF.ESHRepository>();
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
