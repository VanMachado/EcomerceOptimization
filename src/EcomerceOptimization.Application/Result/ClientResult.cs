using EcomerceOptimization.Application.Result.Enum;
using EcomerceOptimization.Domain.Entity.DTO;
using System.Net;

namespace EcomerceOptimization.Application.Result
{
    public class ClientResult
    {
        public ClientStatus Status { get; set; }        
        public string Message { get; set; }

        public ClientResult(ClientStatus status,
                           string message)
        {
            Status = status;            
            Message = message;
        }

        public static ClientResult Created () => 
            new ClientResult(ClientStatus.Created, null);

        public static ClientResult Updated() =>
           new ClientResult(ClientStatus.Updated, null);

        public static ClientResult Deleted() =>
           new ClientResult(ClientStatus.Deleted, null);

        public static ClientResult NotFound() =>
          new ClientResult(ClientStatus.NotFound, null);

        public static ClientResult Error () => 
            new ClientResult(ClientStatus.Error, null);

    }
}
