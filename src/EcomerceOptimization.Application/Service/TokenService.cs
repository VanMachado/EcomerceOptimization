using EcomerceOptimization.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EcomerceOptimization.Infraestructure.Data.Repository;
using EcomerceOptimization.Infraestructure.Data.UOW.Service;
using System.Data.SqlClient;
using Polly.CircuitBreaker;

namespace EcomerceOptimization.Application.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        private readonly ILogger<TokenService> _logger;

        public TokenService(IConfiguration configuration,
                            IMemoryCache cache,
                            ILogger<TokenService> logger)
        {
            _configuration = configuration;
            _cache = cache;
            _logger = logger;
        }

        public async Task<string> AuthenticateAsync(string email, string password)
        {
            var retryPolicy = ResiliencePolicy.RetryPolicy(_logger);
            var circuitBreakerPolicy = ResiliencePolicy.CircuitBreakerPolicy(_logger);

            return await retryPolicy
                .WrapAsync(circuitBreakerPolicy)
                .ExecuteAsync(async () =>
                {
                    try
                    {
                        using (var uow = TokenEcommerceServiceUoW.GetUnitOfWork())
                        {

                            var user = await uow.GetRepository<TokenEcommerceRepository>().GetTokenByUserAsync(email, password);

                            if (_cache.TryGetValue(email, out string cachedToken) && VerifyPassword(password, user.Password))
                            {
                                _logger.LogInformation("Token found it on user cache");
                                return cachedToken;
                            }

                            if (user != null && VerifyPassword(password, user.Password))
                            {
                                var claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.Name, user.NomeCompleto),
                                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                                };

                                var userRoles = await uow.GetRepository<TokenEcommerceRepository>().GetRoleByUserAsync(user.Id);

                                foreach (var role in userRoles)
                                {
                                    claims.Add(new Claim(ClaimTypes.Role, role));
                                }

                                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]));

                                var token = new JwtSecurityToken(
                                    issuer: _configuration["JwtSettings:Issuer"],
                                    audience: _configuration["JwtSettings:Audience"],
                                    expires: DateTime.Now.AddHours(3),
                                    claims: claims,
                                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                                );

                                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

                                _cache.Set(email, jwtToken, new MemoryCacheEntryOptions
                                {
                                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(3)
                                });

                                _logger.LogInformation($"Token generated and stored on cache for user {email}");

                                return jwtToken;
                            }

                            _logger.LogWarning($"User {email} not Authorized");

                            return null;
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

        private bool VerifyPassword(string inputPassword, string? storedPasswordHash)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, storedPasswordHash);
        }
    }
}
