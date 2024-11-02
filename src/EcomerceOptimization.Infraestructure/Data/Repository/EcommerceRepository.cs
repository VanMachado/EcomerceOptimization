using Dapper;
using EcomerceOptimization.Domain.Entity;
using EcomerceOptimization.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;

namespace EcomerceOptimization.Infraestructure.Data.Repository
{
    public class EcommerceRepository : IEcommerceRepository
    {
        private readonly IUnitOfWork _unitOfWork = null;
        private readonly ILogger<EcommerceRepository> _logger;

        public EcommerceRepository()
        {
        }

        public EcommerceRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserEcommerce> GetTokenByUserAsync(string nomeCompleto, string password)
        {
            try
            {
                var query = @"SELECT * FROM UsersEcommerce WHERE NomeCompleto = @NomeCompleto";

                var result = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<UserEcommerce>(
                    sql: query,
                    param: new { nomeCompleto },
                    transaction: _unitOfWork.Transaction,
                    commandTimeout: _unitOfWork.CommandTimeout,
                    commandType: CommandType.Text
                    ).ConfigureAwait(false);

                return result;
            }
            catch (SqlException ex)
            {
                _logger.LogError("Error when try connect to SQL Server");
                throw new InvalidOperationException("Server out, please try later");
            }

        }

        public async Task<IEnumerable<string>> GetRoleByUserAsync(int userId)
        {
            try
            {
                var query = @"SELECT R.RoleName FROM RoleNameEcommerce R
                                INNER JOIN UsersEcommerce UR ON R.Id = UR.RoleId
                                WHERE UR.Id = @UserId";

                var result = await _unitOfWork.Connection.QueryAsync<string>(
                   sql: query,
                   param: new { userId },
                   transaction: _unitOfWork.Transaction,
                   commandTimeout: _unitOfWork.CommandTimeout,
                   commandType: CommandType.Text
                   ).ConfigureAwait(false);

                return result;
            }
            catch (SqlException ex)
            {
                _logger.LogError("Error when try connect to SQL Server");
                throw new InvalidOperationException("Server out, please try later");
            }
        }
    }
}
