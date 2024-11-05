using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomerceOptimization.Domain.Interfaces
{
    public interface IInitializerEcommerceRepository
    {
        Task<bool> CheckAdminQueryAsync(string NomeCompleto, int RoleId);
        Task InsertIntoAdminQueryAsync();
    }
}
