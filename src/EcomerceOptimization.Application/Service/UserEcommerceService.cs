using EcomerceOptimization.Domain.Entity.DTO;
using EcomerceOptimization.Domain.Interfaces;
using EcomerceOptimization.Infraestructure.Data.Repository;
using EcomerceOptimization.Infraestructure.Data.UOW.Service;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Polly.CircuitBreaker;
using System.Data.SqlClient;

namespace EcomerceOptimization.Application.Service
{
    public class UserEcommerceService : IUserEcommerceService
    {
        private readonly IEcommerceRepository _repository;
        private readonly IMemoryCache _cache;
        private readonly ILogger<TokenService> _logger;

        public UserEcommerceService(IEcommerceRepository repository, IMemoryCache cache, ILogger<TokenService> logger)
        {
            _repository = repository;
            _cache = cache;
            _logger = logger;
        }

        public async Task<IEnumerable<ClientEcommerceDTO>> GetClienstAsync()
        {
            var retryPolicy = ResiliencePolicy.RetryPolicy(_logger);
            var circuitBreakerPolicy = ResiliencePolicy.CircuitBreakerPolicy(_logger);

            return await retryPolicy
                .WrapAsync(circuitBreakerPolicy)
                .ExecuteAsync(async () =>
                {
                    try
                    {                        
                        using (var uow = ClientEcommerceServiceUoW.GetUnitOfWork())
                        {                            
                            if (_cache.TryGetValue("all_clients", out IEnumerable<ClientEcommerceDTO> cachedQuery))
                            {
                                _logger.LogInformation("Clients found in cache.");
                                return cachedQuery;
                            }
                                                        
                            var result = await uow.GetRepository<EcommerceRepository>().GetAllClientsAsync();
                            _logger.LogInformation("Clients successfully retrieved from the database.");
                                                        
                            _cache.Set("all_clients", result, new MemoryCacheEntryOptions
                            {
                                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(3)
                            });
                            _logger.LogInformation("Query stored in cache.");

                            return result;
                        }
                    }
                    catch (SqlException ex)
                    {
                        _logger.LogError("Database is out of service. Please, try again later.");
                        throw;
                    }
                    catch (BrokenCircuitException ex)
                    {
                        _logger.LogError(ex, "Circuit breaker is open; the service is currently unavailable.");
                        throw;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An unexpected error occurred.");
                        throw;
                    }
                });
        }

        public async Task<ClientEcommerceDTO> GetClientByIdAsync(int id)
        {
            var retryPolicy = ResiliencePolicy.RetryPolicy(_logger);
            var circuitBreakerPolicy = ResiliencePolicy.CircuitBreakerPolicy(_logger);

            return await retryPolicy
                .WrapAsync(circuitBreakerPolicy)
                .ExecuteAsync(async () =>
                {
                    try
                    {                        
                        using (var uow = ClientEcommerceServiceUoW.GetUnitOfWork())
                        {
                            if (_cache.TryGetValue("client", out ClientEcommerceDTO cachedQuery))
                            {
                                _logger.LogInformation("Client found it on user cache");
                                return cachedQuery;
                            }

                            var result = await uow.GetRepository<EcommerceRepository>().GetClientByIdAsync(id);
                            _logger.LogInformation($"Client: {result.NomeCompleto} successfully retrived!");

                            _cache.Set("client", result, new MemoryCacheEntryOptions
                            {
                                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(3)
                            });
                            _logger.LogInformation("Query stored on cache!");

                            return result;
                        }
                    }
                    catch (SqlException ex)
                    {
                        _logger.LogError("Database is out of service. Please, try again latter");
                        throw;
                    }
                    catch (BrokenCircuitException ex)
                    {
                        _logger.LogError(ex, "Circuit breaker is open, the service is currently unavailable.");
                        throw;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                });
        }

        public async Task<bool> CreateClientAsync(ClientEcommerceDTO dto)
        {
            var retryPolicy = ResiliencePolicy.RetryPolicy(_logger);
            var circuitBreakerPolicy = ResiliencePolicy.CircuitBreakerPolicy(_logger);

            return await retryPolicy
                .WrapAsync(circuitBreakerPolicy)
                .ExecuteAsync(async () =>
                {
                    try
                    {
                        ClientEcommerceServiceUoW.ResetUnitOfWork();

                        using (var uow = ClientEcommerceServiceUoW.GetUnitOfWork())
                        {
                            var result = await uow.GetRepository<EcommerceRepository>().CreateClientEcommerceAsync(dto);
                            _logger.LogInformation($"Client: {dto.NomeCompleto} successfully created! Client Email: {dto.Email}");

                            return result;
                        }
                    }
                    catch (SqlException ex)
                    {
                        _logger.LogError("Database is out of service. Please, try again latter");
                        throw;
                    }
                    catch (BrokenCircuitException ex)
                    {
                        _logger.LogError(ex, "Circuit breaker is open, the service is currently unavailable.");
                        throw;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                });
        }

        public async Task<ClientEcommerceDTO> UpdateClientAsync(ClientEcommerceDTO dto)
        {
            var retryPolicy = ResiliencePolicy.RetryPolicy(_logger);
            var circuitBreakerPolicy = ResiliencePolicy.CircuitBreakerPolicy(_logger);

            return await retryPolicy
                .WrapAsync(circuitBreakerPolicy)
                .ExecuteAsync(async () =>
                {
                    try
                    {
                        ClientEcommerceServiceUoW.ResetUnitOfWork();

                        using (var uow = ClientEcommerceServiceUoW.GetUnitOfWork())
                        {
                            var result = await uow.GetRepository<EcommerceRepository>().UpdateClientEcommerceAsync(dto);
                            _logger.LogInformation($"Client: {dto.NomeCompleto} successfully updated! Client Email: {dto.Email}");

                            return result;
                        }
                    }
                    catch (SqlException ex)
                    {
                        _logger.LogError("Database is out of service. Please, try again latter");
                        throw;
                    }
                    catch (BrokenCircuitException ex)
                    {
                        _logger.LogError(ex, "Circuit breaker is open, the service is currently unavailable.");
                        throw;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                });
        }

        public async Task<bool> DeleteClientAsync(int id)
        {
            var retryPolicy = ResiliencePolicy.RetryPolicy(_logger);
            var circuitBreakerPolicy = ResiliencePolicy.CircuitBreakerPolicy(_logger);

            return await retryPolicy
                .WrapAsync(circuitBreakerPolicy)
                .ExecuteAsync(async () =>
                {
                    try
                    {
                        ClientEcommerceServiceUoW.ResetUnitOfWork();

                        using (var uow = ClientEcommerceServiceUoW.GetUnitOfWork())
                        {
                            var result = await uow.GetRepository<EcommerceRepository>().DeleteClientEcommerceAsync(id);
                            _logger.LogInformation($"Client successfully deleted!");

                            return result;
                        }
                    }
                    catch (SqlException ex)
                    {
                        _logger.LogError("Database is out of service. Please, try again latter");
                        throw;
                    }
                    catch (BrokenCircuitException ex)
                    {
                        _logger.LogError(ex, "Circuit breaker is open, the service is currently unavailable.");
                        throw;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                });
        }
    }
}
