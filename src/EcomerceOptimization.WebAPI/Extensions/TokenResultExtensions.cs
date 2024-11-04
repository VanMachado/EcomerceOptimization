using EcomerceOptimization.Application.Result;
using EcomerceOptimization.Application.Result.Enum;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;
using System.Runtime.CompilerServices;

namespace EcomerceOptimization.WebAPI.Extensions
{
    public static class TokenResultExtensions
    {
        private static readonly Dictionary<TokenStatus, Func<TokenResult, IActionResult>> dictionary = new Dictionary<TokenStatus, Func<TokenResult, IActionResult>>
        {
            {
                TokenStatus.Success,

                result =>
                {
                    var response = new TokenResult(result.TokenStatus, result.HttpStatus, result.Token);

                    return new OkObjectResult(response);
                }
            },
            {
                TokenStatus.NotAuthorized,                

                result =>
                {
                    var notAuthorizedResponse = new TokenResult(
                        TokenStatus.NotAuthorized,
                        HttpStatusCode.Unauthorized,
                        "Unauthorized user"
                    );
                    
                    return new ObjectResult(notAuthorizedResponse)
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized,
                    };                    
                }
            },
            {
                TokenStatus.Error,

                result =>
                {

                    var errorResponse = new TokenResult(
                            TokenStatus.Error,
                            HttpStatusCode.ServiceUnavailable,
                            "Service temporarily unavailable. Please try again later."
                    );

                    return new ObjectResult(errorResponse)
                    {
                        StatusCode = (int)HttpStatusCode.ServiceUnavailable
                    };

                }
            }
        };

        public static IActionResult AsActionResult(this TokenResult result)
        {
            return dictionary[result.TokenStatus](result);
        }
    }
}
