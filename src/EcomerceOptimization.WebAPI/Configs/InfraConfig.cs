using EcomerceOptimization.Application.Service;
using EcomerceOptimization.Domain.Interfaces;
using EcomerceOptimization.Infraestructure.Data.Repository;
using EcomerceOptimization.Infraestructure.Data.UOW;

namespace EcomerceOptimization.WebAPI.Configs
{
    public static class InfraConfig
    {
        public static void RegisterInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            var commandTimeout = configuration.GetValue<int>("DatabaseDefaults:DefaultCommandTimeout");

            var EODB = "EO-DEV-SQL";
            var connectionString = configuration.GetConnectionString(EODB);
            UnitOfWorkConnectionStringPool.SetConnectionString(EODB, connectionString, commandTimeout);

            services.AddTransient<IEcommerceRepository, EcommerceRepository>();
            services.AddTransient<IUserEcommerceRepository, UserEcommerceRepository>();
            services.AddSingleton<IDatabaseInitializer, DatabaseinitializerService>();
            services.AddHostedService<DatabaseInitializerHostedService>();
        }
    }
}
