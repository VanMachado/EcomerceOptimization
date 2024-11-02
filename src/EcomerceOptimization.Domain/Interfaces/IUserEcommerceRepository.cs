namespace EcomerceOptimization.Domain.Interfaces
{
    public interface IUserEcommerceRepository
    {
        Task<bool> CheckAdminQuery(string NomeCompleto, int RoleId);
        Task InsertIntoAdminQuery();
    }
}
