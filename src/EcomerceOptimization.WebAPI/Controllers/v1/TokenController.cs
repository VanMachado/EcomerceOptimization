using EcomerceOptimization.Application.Result;
using EcomerceOptimization.Domain.Entity.DTO;
using EcomerceOptimization.Domain.Interfaces;
using EcomerceOptimization.WebAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polly.CircuitBreaker;
using Swashbuckle.AspNetCore.Annotations;
using System.Data.SqlClient;
using System.Net;

namespace EcomerceOptimization.WebAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/ecommerce-optimization/token")]
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
        [SwaggerResponse(StatusCodes.Status200OK, "TokenResult", typeof(TokenResult))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(UnauthorizedResult))]
        [SwaggerResponse(StatusCodes.Status503ServiceUnavailable, "InternalServerError")]
        public async Task<IActionResult> Login([FromBody] TokenRequestDTO tokenRequest)
        {
            try
            {
                var token = await _service.AuthenticateAsync(tokenRequest.Name, tokenRequest.Password);                

                if (token == null)
                    return TokenResult.NotAuthorized().AsActionResult();

                return TokenResult.Success(token).AsActionResult();
            }
            catch (SqlException ex)
            {
                _logger.LogError($"Database is out of service. Please, try again latter. {ex.Message}");
                return TokenResult.Error().AsActionResult();
            }
            catch (BrokenCircuitException ex)
            {
                _logger.LogError("Circuit breaker is open, the service is currently unavailable.");

                return TokenResult.Error().AsActionResult();
            }
            catch (Exception)
            {
                _logger.LogError($"Something went wrong when calling API");
                throw;
            }
        }
    }
}
