using EcomerceOptimization.Domain.DTO;
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
    public class ClientEcommerceService : IClientEcommerceService
    {
        private readonly IEcommerceRepository _repository;
        private readonly IMemoryCache _cache;
        private readonly ILogger<TokenService> _logger;

        public ClientEcommerceService(IEcommerceRepository repository,
                                   IMemoryCache cache,
                                   ILogger<TokenService> logger)
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
                            if (_cache.TryGetValue("all_clients", out IEnumerable<CachedObjectDTO> cachedQuery))
                            {
                                var updateTime = await uow.GetRepository<EcommerceRepository>().GetUpdateTimeAsync();
                                var checkDate = cachedQuery.Where(c => c.InsertDate >= updateTime).FirstOrDefault();

                                if (checkDate != null)
                                {
                                    _logger.LogInformation("Clients found in cache.");
                                    var clientCachingList = new List<ClientEcommerceDTO>();

                                    foreach (var cachedObject in cachedQuery)
                                        clientCachingList.Add(cachedObject.ClientDTO);

                                    return clientCachingList;
                                }
                            }

                            var result = await uow.GetRepository<EcommerceRepository>().GetAllClientsAsync();
                            _logger.LogInformation("Clients successfully retrieved from the database.");

                            var listToCache = new List<CachedObjectDTO>();

                            foreach (var client in result)
                            {
                                var cached = new CachedObjectDTO
                                {
                                    ClientDTO = client,
                                    InsertDate = DateTime.Now
                                };

                                listToCache.Add(cached);
                            }

                            _cache.Set("all_clients", listToCache, new MemoryCacheEntryOptions
                            {
                                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(3)
                            });
                            _logger.LogInformation("Query stored on cache.");

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
                            if (_cache.TryGetValue("client", out CachedObjectDTO cachedQuery))
                            {
                                var updateTime = await uow.GetRepository<EcommerceRepository>().GetUpdateTimeAsync();

                                if (cachedQuery.InsertDate >= updateTime)
                                {
                                    _logger.LogInformation("Client found it on user cache");
                                    return cachedQuery.ClientDTO;
                                }
                            }

                            var result = await uow.GetRepository<EcommerceRepository>().GetClientByIdAsync(id);
                            _logger.LogInformation($"Client: {result.NomeCompleto} successfully retrived on Database!");

                            cachedQuery = new CachedObjectDTO
                            {
                                ClientDTO = result,
                                InsertDate = DateTime.Now
                            };

                            _cache.Set("client", cachedQuery, new MemoryCacheEntryOptions
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
                    catch (NullReferenceException)
                    {
                        _logger.LogWarning("Client not found!");
                        return null;
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
                        using (var uow = ClientEcommerceServiceUoW.GetUnitOfWork())
                        {
                            uow.BeginTransaction();
                            var result = await uow.GetRepository<EcommerceRepository>().CreateClientEcommerceAsync(dto);

                            _logger.LogInformation($"Client: {dto.NomeCompleto} successfully created! Client Email: {dto.Email}");

                            using (var update = ClientEcommerceServiceUoW.GetUnitOfWork())
                            {
                                update.BeginTransaction();
                                await update.GetRepository<EcommerceRepository>().RegisterUpdateAsync();
                            }

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
                        using (var uow = ClientEcommerceServiceUoW.GetUnitOfWork())
                        {
                            uow.BeginTransaction();

                            var result = await uow.GetRepository<EcommerceRepository>().UpdateClientEcommerceAsync(dto);
                            _logger.LogInformation($"Client: {dto.NomeCompleto} successfully updated! Client Email: {dto.Email}");

                            using (var update = ClientEcommerceServiceUoW.GetUnitOfWork())
                            {
                                update.BeginTransaction();
                                await update.GetRepository<EcommerceRepository>().RegisterUpdateAsync();
                            }

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
                        using (var uow = ClientEcommerceServiceUoW.GetUnitOfWork())
                        {                            
                            uow.BeginTransaction();
                            
                            if(!await uow.GetRepository<EcommerceRepository>().DeleteClientEcommerceAsync(id))
                            {
                                _logger.LogWarning("Client not Found!");
                                return false;
                            }

                            _logger.LogInformation($"Client successfully deleted!");

                            using (var update = ClientEcommerceServiceUoW.GetUnitOfWork())
                            {
                                update.BeginTransaction();
                                await update.GetRepository<EcommerceRepository>().RegisterUpdateAsync();
                            }

                            return true;
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
                    catch (NullReferenceException)
                    {
                        return false;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                });
        }
    }
}
