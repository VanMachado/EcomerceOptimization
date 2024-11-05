using EcomerceOptimization.Domain.Entity.DTO;

namespace EcomerceOptimization.Domain.Interfaces
{
    public interface IUserEcommerceService
    {
        Task<ClientEcommerceDTO> GetClientByIdAsync(int id);
        Task<IEnumerable<ClientEcommerceDTO>> GetClienstAsync();
        Task<bool> CreateClientAsync(ClientEcommerceDTO client);
        Task<ClientEcommerceDTO> UpdateClientAsync(ClientEcommerceDTO client);
        Task<bool> DeleteClientAsync(int id);
    }
}
