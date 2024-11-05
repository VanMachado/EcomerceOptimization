using EcomerceOptimization.Application.Result.Enum;
using EcomerceOptimization.Domain.Entity.DTO;
using System.Net;

namespace EcomerceOptimization.Application.Result
{
    public class ClientResult
    {
        public ClientStatus Status { get; set; }
        public ClientEcommerceDTO Client { get; set; }
        public string Message { get; set; }

        public ClientResult(ClientStatus status, ClientEcommerceDTO client, string message)
        {
            Status = status;
            Client = client;
            Message = message;
        }

        public static ClientResult Created () => 
            new ClientResult(ClientStatus.Created, null, null);

        public static ClientResult Updated(ClientEcommerceDTO client) =>
           new ClientResult(ClientStatus.Updated, client ,null);

        public static ClientResult Deleted() =>
           new ClientResult(ClientStatus.Deleted,null, null);

        public static ClientResult Error () => 
            new ClientResult(ClientStatus.Error, null, null);

    }
}
