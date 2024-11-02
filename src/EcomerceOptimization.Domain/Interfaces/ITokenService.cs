using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomerceOptimization.Domain.Interfaces
{
    public interface ITokenService
    {
        Task<string> AuthenticateAsync(string email, string password);
    }
}
