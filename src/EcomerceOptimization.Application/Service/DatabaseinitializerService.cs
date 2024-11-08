using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using EcomerceOptimization.Domain.Interfaces;
using EcomerceOptimization.Infraestructure.Data.UOW.Service;
using EcomerceOptimization.Domain.Entity;
using EcomerceOptimization.Infraestructure.Data.Repository;
using EcomerceOptimization.Application.Result;
using Polly.CircuitBreaker;

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

        public async Task InitializeDatabase()
        {
            var retryPolicy = ResiliencePolicy.RetryPolicy(_logger);
            var circuitBreakerPolicy = ResiliencePolicy.CircuitBreakerPolicy(_logger);

            await retryPolicy
                .WrapAsync(circuitBreakerPolicy)
                .ExecuteAsync(async () =>
                {
                    try
                    {
                        using (var uow = EcommerceUoWFactory.GetUnitOfWork())
                        {

                            var adminExists = await uow.GetRepository<InitializerEcommerceRepository>().CheckAdminQueryAsync("admin", 1);

                            if (!adminExists)
                            {
                                await uow.GetRepository<InitializerEcommerceRepository>().InsertIntoAdminQueryAsync();

                                _logger.LogInformation("Adim user sucefully inserted!");
                            }
                            else
                            {
                                _logger.LogInformation("Admin user already exists. Actions not needed.");
                            }
                        }
                    }
                    catch (BrokenCircuitException)
                    {
                        _logger.LogError("Circuit breaker is open, the service is currently unavailable.");                                                
                    }
                    catch (SqlException)
                    {
                        _logger.LogError("Database is out of service. Please, try again latter.");                                                
                    }
                });
        }
    }
}
