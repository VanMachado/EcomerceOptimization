using EcomerceOptimization.Application.Result;
using EcomerceOptimization.Domain.Entity.DTO;
using EcomerceOptimization.Domain.Interfaces;
using EcomerceOptimization.WebAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EcomerceOptimization.WebAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/ecommerce-optimization/order-ecommerce")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderEcommerceService _service;
        private readonly ILogger _logger;

        public OrderController(IOrderEcommerceService service,
                              ILogger<OrderController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound, "NotFound")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(UnauthorizedResult))]
        [SwaggerResponse(StatusCodes.Status503ServiceUnavailable, "InternalServerError")]
        public async Task<IActionResult> GetOrderById([FromRoute] int id)
        {
            try
            {
                var result = await _service.GetOrderByIdAsync(id);

                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(UnauthorizedResult))]
        [SwaggerResponse(StatusCodes.Status503ServiceUnavailable, "InternalServerError")]
        public async Task<IActionResult> Create([FromBody] OrderEcommerceDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest();

                if (await _service.CreatOrderAsync(dto))
                    return OrderResult.Created().AsActionResult();

                return OrderResult.Error().AsActionResult();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
