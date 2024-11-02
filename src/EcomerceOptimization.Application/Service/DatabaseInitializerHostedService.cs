using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using EcomerceOptimization.Domain.Interfaces;

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
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred during database initialization.");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
