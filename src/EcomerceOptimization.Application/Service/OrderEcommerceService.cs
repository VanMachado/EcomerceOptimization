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
    public class OrderEcommerceService : IOrderEcommerceService
    {        
        private readonly IMemoryCache _cache;
        private readonly ILogger<TokenService> _logger;

        public OrderEcommerceService(IMemoryCache cache, ILogger<TokenService> logger)
        {     
            _cache = cache;
            _logger = logger;
        }

        public async Task<bool> CreatOrderAsync(OrderEcommerceDTO dto)
        {
            var retryPolicy = ResiliencePolicy.RetryPolicy(_logger);
            var circuitBreakerPolicy = ResiliencePolicy.CircuitBreakerPolicy(_logger);

            return await retryPolicy
                .WrapAsync(circuitBreakerPolicy)
                .ExecuteAsync(async () =>
                {
                    try
                    {                        
                        using (var uow = OrderEcommerceServiceUoW.GetUnitOfWork())
                        {
                            uow.BeginTransaction();

                            var result = await uow.GetRepository<EcommerceRepository>().CreateOrderAsync(dto);
                            _logger.LogInformation($"Order: {dto.Id} successfully created!");

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

        public async Task<OrderEcommerceDTO> GetOrderByIdAsync(int id)
        {
            var retryPolicy = ResiliencePolicy.RetryPolicy(_logger);
            var circuitBreakerPolicy = ResiliencePolicy.CircuitBreakerPolicy(_logger);

            return await retryPolicy
                .WrapAsync(circuitBreakerPolicy)
                .ExecuteAsync(async () =>
                {
                    try
                    {                        
                        using (var uow = OrderEcommerceServiceUoW.GetUnitOfWork())
                        {
                            uow.BeginTransaction();

                            if (_cache.TryGetValue("order", out OrderEcommerceDTO cachedQuery))
                            {
                                _logger.LogInformation("Client found it on user cache");
                                return cachedQuery;
                            }

                            var result = await uow.GetRepository<EcommerceRepository>().GetOrderByIdAsync(id);
                            _logger.LogInformation($"Order: {result.NomeProduto} successfully retrived!");

                            _cache.Set("order", result, new MemoryCacheEntryOptions
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
    }
}
