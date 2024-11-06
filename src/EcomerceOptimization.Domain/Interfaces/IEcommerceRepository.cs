using EcomerceOptimization.Domain.Entity;
using EcomerceOptimization.Domain.Entity.DTO;
using EcomerceOptimization.Domain.Entity.Enum;

namespace EcomerceOptimization.Domain.Interfaces
{
    public interface IEcommerceRepository
    {        
        Task<ClientEcommerceDTO> GetClientByIdAsync(int id);
        Task<IEnumerable<ClientEcommerceDTO>> GetAllClientsAsync();
        Task<bool> CreateClientEcommerceAsync(ClientEcommerceDTO dto);
        Task<ClientEcommerceDTO> UpdateClientEcommerceAsync(ClientEcommerceDTO dto);
        Task<bool> DeleteClientEcommerceAsync(int id);
        Task<bool> CreateOrderAsync(OrderEcommerceDTO dto);
        Task<OrderEcommerceDTO> GetOrderByIdAsync(int id);
        Task RegisterUpdateAsync();
        Task<DateTime> GetUpdateTimeAsync();
    }
}
