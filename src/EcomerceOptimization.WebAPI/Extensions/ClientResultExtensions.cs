using EcomerceOptimization.Application.Result.Enum;
using EcomerceOptimization.Application.Result;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using EcomerceOptimization.Domain.Entity.DTO;

namespace EcomerceOptimization.WebAPI.Extensions
{
    public static class ClientResultExtensions
    {
        private static readonly Dictionary<ClientStatus, Func<ClientResult, IActionResult>> dictionary = new Dictionary<ClientStatus, Func<ClientResult, IActionResult>>
        {
            {
                ClientStatus.Created,

                result =>
                {                    
                    var response = new ClientResult(result.Status, $"StatusCode: {HttpStatusCode.Created}. Client successfully created!");

                    return new ObjectResult(response)
                    {
                        StatusCode = (int)HttpStatusCode.Created,
                    };
                }
            },
            {
                ClientStatus.Updated,

                result =>
                {
                    var updatedResponse = new ClientResult(
                        ClientStatus.Updated,                       
                        "Client successfully update!"
                    );

                    return new ObjectResult(updatedResponse)
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                    };
                }
            },
            {
                ClientStatus.Deleted,

                result =>
                {

                    var deleteResponse = new ClientResult(
                            ClientStatus.Deleted,                            
                            "Client successfully deleted!"
                    );

                    return new ObjectResult(deleteResponse);

                }
            },
            {
                ClientStatus.NotFound,

                result =>
                {                    
                    var notFOundResponse = new ClientResult(
                            ClientStatus.NotFound,
                            "Client not found!"
                    );

                    return new ObjectResult(notFOundResponse)
                    { 
                        StatusCode = (int)HttpStatusCode.NotFound,
                    };

                }
            },
            {
                ClientStatus.Error,

                result =>
                {

                    var errorResponse = new ClientResult(
                            ClientStatus.Error,  
                            "Client not Found!"
                    );

                    return new ObjectResult(errorResponse)
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest
                    };

                }
            }
        };

        public static IActionResult AsActionResult(this ClientResult result)
        {
            return dictionary[result.Status](result);
        }
    }
}
