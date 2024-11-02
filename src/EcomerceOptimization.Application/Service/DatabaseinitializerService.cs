using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using EcomerceOptimization.Domain.Interfaces;
using EcomerceOptimization.Infraestructure.Data.UOW.Service;
using EcomerceOptimization.Domain.Entity;
using EcomerceOptimization.Infraestructure.Data.Repository;

namespace EcomerceOptimization.Application.Service
{
    public class DatabaseinitializerService : IDatabaseInitializer
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DatabaseinitializerService> _logger;

        public DatabaseinitializerService(IConfiguration configuration, 
                                  ILogger<DatabaseinitializerService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async void InitializeDatabase()
        {            
            using (var uow = UserEcommerceServiceUoW.GetUnitOfWork())
            {
                var adminExists = await uow.GetRepository<UserEcommerceRepository>().CheckAdminQuery("admin", 1);

                if (!adminExists)
                {                    
                    uow.GetRepository<UserEcommerceRepository>().InsertIntoAdminQuery().GetAwaiter().GetResult();

                    _logger.LogInformation("Adim user sucefully inserted!");
                }
                else
                {
                    _logger.LogInformation("Admin user already exists. Actions not needed.");
                }
            }
        }
    }
}
