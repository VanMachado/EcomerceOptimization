using EcomerceOptimization.Domain.Entity;
using EcomerceOptimization.Domain.Entity.DTO;

namespace EcomerceOptimization.Domain.Interfaces
{
    public interface IEcommerceRepository
    {        
        Task<ClientEcommerceDTO> GetClientByIdAsync(int id);
        Task<IEnumerable<ClientEcommerceDTO>> GetAllClientsAsync();
        Task<bool> CreateClientEcommerceAsync(ClientEcommerceDTO dto);
        Task<ClientEcommerceDTO> UpdateClientEcommerceAsync(ClientEcommerceDTO dto);
        Task<bool> DeleteClientEcommerceAsync(int id);
    }
}
