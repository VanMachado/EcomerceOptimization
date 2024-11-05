using Dapper;
using EcomerceOptimization.Domain.Interfaces;
using EcomerceOptimization.Infraestructure.Data.UOW;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomerceOptimization.Infraestructure.Data.Repository
{
    public class InitializerEcommerceRepository : IInitializerEcommerceRepository
    {
        private readonly IUnitOfWork _unitOfWork = null;

        public InitializerEcommerceRepository()
        {
        }

        public InitializerEcommerceRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CheckAdminQueryAsync(string NomeCompleto, int RoleId)
        {
            string checkAdminQuery = @"
                    SELECT COUNT(1) 
                    FROM [Ecommerce].[dbo].[UsersEcommerce] 
                    WHERE NomeCompleto = @NomeCompleto AND RoleId = @RoleId"
            ;

            var result = await _unitOfWork.Connection.ExecuteScalarAsync<int>(
                   sql: checkAdminQuery,
                   param: new { NomeCompleto, RoleId },
                   transaction: _unitOfWork.Transaction,
                   commandTimeout: _unitOfWork.CommandTimeout,
                   commandType: CommandType.Text
                   ).ConfigureAwait(false) > 0;

            return result;
        }

        public async Task InsertIntoAdminQueryAsync()
        {
            string password = "Admin@123";
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            string insertAdminQuery = @"
                    INSERT INTO [Ecommerce].[dbo].[UsersEcommerce] (RoleId, NomeCompleto, Password)
                    VALUES (@RoleId, @NomeCompleto, @Password)"
            ;

            await _unitOfWork.Connection.ExecuteAsync(insertAdminQuery, new
            {
                RoleId = 1,
                NomeCompleto = "admin",
                Password = hashedPassword
            });
        }
    }
}
