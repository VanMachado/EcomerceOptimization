using EcomerceOptimization.Application.Result;
using EcomerceOptimization.Domain.Entity.DTO;
using EcomerceOptimization.Domain.Entity.Enum;
using EcomerceOptimization.Domain.Interfaces;
using EcomerceOptimization.WebAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using Swashbuckle.AspNetCore.Annotations;

namespace EcomerceOptimization.WebAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/ecommerce-optimization/user-ecommerce")]
    public class UserEcommerceController : ControllerBase
    {
        private readonly IClientEcommerceService _service;
        private readonly ILogger _logger;

        public UserEcommerceController(IClientEcommerceService service,
                                      ILogger<UserEcommerceController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        [SwaggerResponse(StatusCodes.Status200OK, "ClientResult", typeof(ClientResult))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(UnauthorizedResult))]
        [SwaggerResponse(StatusCodes.Status503ServiceUnavailable, "InternalServerError")]
        public async Task<IActionResult> GetClientById([FromRoute] int id)
        {
            try
            {
                var result = await _service.GetClientByIdAsync(id);

                if (result == null)
                    return ClientResult.Error().AsActionResult();

                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        [SwaggerResponse(StatusCodes.Status200OK, "ClientResult", typeof(ClientResult))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(UnauthorizedResult))]
        [SwaggerResponse(StatusCodes.Status503ServiceUnavailable, "InternalServerError")]
        public async Task<IActionResult> GetAllClients()
        {
            try
            {
                var results = await _service.GetClienstAsync();

                if (results == null)
                    return ClientResult.Error().AsActionResult();

                return Ok(results);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [SwaggerResponse(StatusCodes.Status201Created, "ClientResult", typeof(ClientResult))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(UnauthorizedResult))]
        [SwaggerResponse(StatusCodes.Status503ServiceUnavailable, "InternalServerError")]
        public async Task<IActionResult> Create([FromBody] ClientEcommerceDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest();

                if (await _service.CreateClientAsync(dto))
                    return ClientResult.Created().AsActionResult();

                return ClientResult.Error().AsActionResult();
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPut]
        [Authorize(Roles = "admin")]
        [SwaggerResponse(StatusCodes.Status200OK, "ClientResult", typeof(ClientResult))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(UnauthorizedResult))]
        [SwaggerResponse(StatusCodes.Status503ServiceUnavailable, "InternalServerError")]
        public async Task<IActionResult> Update([FromBody] ClientEcommerceDTO dto)
        {
            try
            {
                var result = await _service.UpdateClientAsync(dto);

                if (result == null)
                    return ClientResult.Error().AsActionResult();

                return ClientResult.Updated(result).AsActionResult();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "ClientResult", typeof(ClientResult))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(UnauthorizedResult))]
        [SwaggerResponse(StatusCodes.Status503ServiceUnavailable, "InternalServerError")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                if (await _service.DeleteClientAsync(id))
                    return ClientResult.Deleted().AsActionResult();

                return ClientResult.Error().AsActionResult();
            }
            catch (NullReferenceException)
            {
                return ClientResult.Error().AsActionResult();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
