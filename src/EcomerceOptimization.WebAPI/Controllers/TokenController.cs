using EcomerceOptimization.Application.DTO;
using EcomerceOptimization.Application.Result;
using EcomerceOptimization.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polly.CircuitBreaker;
using System.Data.SqlClient;

namespace EcomerceOptimization.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _service;
        private readonly ILogger _logger;

        public TokenController(ITokenService service,
                              ILogger<TokenController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]            
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]      
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<TokenResult> Login([FromBody] TokenRequestDTO tokenRequest)
        {
            try
            {
                var token = await _service.AuthenticateAsync(tokenRequest.Name, tokenRequest.Password);

                if (token == null)
                    return TokenResult.NotAuthorized();

                return TokenResult.Success(token);
            }            
            catch (SqlException ex)
            {
                _logger.LogError($"Database is out of service. Please, try again latter. {ex.Message}");
                return TokenResult.Error();
            }
            catch (BrokenCircuitException ex)
            {                
                _logger.LogError("Circuit breaker is open, the service is currently unavailable.");

                return TokenResult.Error();
            }
            catch (Exception)
            {
                _logger.LogError($"Something went wrong when calling API");
                throw;
            }
        }
    }
}
