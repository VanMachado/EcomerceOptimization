using EcomerceOptimization.Application.Result.Enum;
using System.Net;

namespace EcomerceOptimization.Application.Result
{
    public class TokenResult
    {
        public TokenStatus TokenStatus { get; set; }
        public HttpStatusCode HttpStatus { get; set; }
        public string Token { get; set; }

        public TokenResult(TokenStatus tokenStatus, 
                          HttpStatusCode httpStatus, 
                          string token)
        {
            TokenStatus = tokenStatus;
            HttpStatus = httpStatus;
            Token = token;
        }

        public static TokenResult Success(string token) =>
            new TokenResult(TokenStatus.Success, HttpStatusCode.OK, token);

        public static TokenResult NotAuthorized() =>
            new TokenResult(TokenStatus.NotAuthorized, HttpStatusCode.Unauthorized, "Unauthorized user");

        public static TokenResult Error() =>
            new TokenResult(TokenStatus.Error, HttpStatusCode.ServiceUnavailable, "Service temporarily unavailable. Please try again later.");
    }
}
