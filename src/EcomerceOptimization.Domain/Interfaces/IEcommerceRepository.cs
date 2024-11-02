using EcomerceOptimization.Domain.Entity;

namespace EcomerceOptimization.Domain.Interfaces
{
    public interface IEcommerceRepository
    {
        Task<UserEcommerce> GetTokenByUserAsync(string email, string password);
        Task<IEnumerable<string>> GetRoleByUserAsync(int userId);
    }
}
