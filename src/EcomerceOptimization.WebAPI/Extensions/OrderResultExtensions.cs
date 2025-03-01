using EcomerceOptimization.Application.Result.Enum;
using EcomerceOptimization.Application.Result;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EcomerceOptimization.WebAPI.Extensions
{
    public static class OrderResultExtensions
    {
        private static readonly Dictionary<OrderStatus, Func<OrderResult, IActionResult>> dictionary = new Dictionary<OrderStatus, Func<OrderResult, IActionResult>>
        {
            {
                OrderStatus.Created,

                 result =>
                 {
                    var response = new OrderResult(result.Status, $"StatusCode: {HttpStatusCode.Created}. Order successfully created!");

                    return new ObjectResult(response)
                    {
                        StatusCode = (int)HttpStatusCode.Created,
                    };
                 }
            },
            {
                 OrderStatus.Error,

                 result =>
                 {

                     var errorResponse = new OrderResult(
                             OrderStatus.Error,
                             "Order not Found, or doesn't exists!"
                     );

                    return new ObjectResult(errorResponse)
                    {
                       StatusCode = (int)HttpStatusCode.BadRequest
                    };
                 }
            }
        };

        public static IActionResult AsActionResult(this OrderResult result)
        {
            return dictionary[result.Status](result);
        }
    }
}
