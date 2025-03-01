using EcomerceOptimization.Application.Result.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomerceOptimization.Application.Result
{
    public class OrderResult
    {
        public OrderStatus Status { get; set; }
        public string Message { get; set; }

        public OrderResult(OrderStatus status,
                           string message)
        {
            Status = status;
            Message = message;
        }

        public static OrderResult Created() =>
            new OrderResult(OrderStatus.Created, null);        

        public static OrderResult Error() =>
            new OrderResult(OrderStatus.Error, null);
    }
}
