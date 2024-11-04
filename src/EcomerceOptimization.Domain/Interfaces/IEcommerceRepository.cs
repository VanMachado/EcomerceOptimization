using EcomerceOptimization.Domain.Entity;
using EcomerceOptimization.Domain.Entity.DTO;

namespace EcomerceOptimization.Domain.Interfaces
{
    public interface IEcommerceRepository
    {
        Task<bool> CheckAdminQueryAsync(string NomeCompleto, int RoleId);
        Task InsertIntoAdminQueryAsync();
        Task<ClientEcommerce> GetClientByIdAsync(int id);
        Task<IEnumerable<ClientEcommerce>> GetAllClientsAsync();
        Task CreateClientEcommerceAsync(ClientEcommerceDTO dto);
        Task UpdateClientEcommerceAsync(ClientEcommerceDTO dto);
        Task<bool> DeleteClientEcommerceAsync(int id);
    }
}
