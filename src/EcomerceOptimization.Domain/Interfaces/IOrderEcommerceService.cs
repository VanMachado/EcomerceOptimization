using EcomerceOptimization.Domain.Entity.DTO;

namespace EcomerceOptimization.Domain.Interfaces
{
    public interface IOrderEcommerceService
    {
        Task<OrderEcommerceDTO> GetOrderByIdAsync(int id);
        Task<bool> CreatOrderAsync(OrderEcommerceDTO dto);
    }
}
