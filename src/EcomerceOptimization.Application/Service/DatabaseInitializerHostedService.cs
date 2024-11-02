using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using EcomerceOptimization.Domain.Interfaces;
using Polly.CircuitBreaker;
using System.Data.SqlClient;

namespace EcomerceOptimization.Application.Service
{
    public class DatabaseInitializerHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DatabaseInitializerHostedService> _logger;

        public DatabaseInitializerHostedService(IServiceProvider serviceProvider, ILogger<DatabaseInitializerHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var initializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();

                try
                {
                    initializer.InitializeDatabase();
                    _logger.LogInformation("Database initialization completed successfully.");
                }
                catch (SqlException)
                {
                    _logger.LogError("Database is out of service. Please, try again latter");
                    throw;
                }
                catch (BrokenCircuitException)
                {
                    _logger.LogError("Circuit breaker is open, the service is currently unavailable.");
                    throw;
                }
                catch (Exception)
                {
                    _logger.LogError("Error occurred during database initialization.");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
