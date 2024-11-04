using AutoMapper;
using Dapper;
using EcomerceOptimization.Domain.Entity;
using EcomerceOptimization.Domain.Entity.DTO;
using EcomerceOptimization.Domain.Interfaces;
using System.Data;

namespace EcomerceOptimization.Infraestructure.Data.Repository
{
    public class EcommerceRepository : IEcommerceRepository
    {
        private readonly IUnitOfWork _unitOfWork = null;
        private readonly IMapper _mapper;

        public EcommerceRepository()
        {
        }

        public EcommerceRepository(IUnitOfWork unitOfWork,
                                   IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
                    VALUES (@RoleId, @NomeCompleto, @Password)";

            await _unitOfWork.Connection.ExecuteAsync(insertAdminQuery, new
            {
                RoleId = 1,
                NomeCompleto = "admin",
                Password = hashedPassword
            });
        }

        public Task CreateClientEcommerceAsync(ClientEcommerceDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteClientEcommerceAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ClientEcommerce>> GetAllClientsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ClientEcommerce> GetClientByIdAsync(int id)
        {
            throw new NotImplementedException();
        }        

        public Task UpdateClientEcommerceAsync(ClientEcommerceDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
