using Dapper;
using EcomerceOptimization.Domain.Entity;
using EcomerceOptimization.Domain.Entity.DTO;
using EcomerceOptimization.Domain.Interfaces;
using System.Collections.Generic;
using System.Data;

namespace EcomerceOptimization.Infraestructure.Data.Repository
{
    public class EcommerceRepository : IEcommerceRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public EcommerceRepository()
        {                
        }

        public EcommerceRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;            
        }   
        
        public async Task<IEnumerable<ClientEcommerceDTO>> GetAllClientsAsync()
        {            
            var get = @"SELECT TOP (1000) [Id]
                          ,[NomeCompleto]
                          ,[Email]
                      FROM [Ecommerce].[dbo].[ClientsEcommerce]";

            try
            {
                var clients = await _unitOfWork.Connection.QueryAsync<ClientEcommerceDTO>(
                    sql: get,                    
                    transaction: _unitOfWork.Transaction,
                    commandTimeout: _unitOfWork.CommandTimeout,
                    commandType: CommandType.Text
                ).ConfigureAwait(false);

                return clients;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ClientEcommerceDTO> GetClientByIdAsync(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add(name: "@Id", dbType: DbType.Int32, direction: ParameterDirection.Input, value: id);

            var get = @"SELECT * FROM [Ecommerce].[dbo].[ClientsEcommerce] WHERE Id = @Id";

            try
            {
                var client = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<ClientEcommerceDTO>(
                    sql: get,
                    param: parameters,
                    transaction: _unitOfWork.Transaction,
                    commandTimeout: _unitOfWork.CommandTimeout,
                    commandType: CommandType.Text
                ).ConfigureAwait(false);                

                return client;
            }
            catch (Exception)
            {                
                throw;
            }            
        }


        public async Task<bool> CreateClientEcommerceAsync(ClientEcommerceDTO dto)
        {            
            var parameters = new DynamicParameters();

            parameters.Add(name: "@NomeCompleto", dbType: DbType.String, direction: ParameterDirection.Input, value: dto.NomeCompleto);
            parameters.Add(name: "@Email", dbType: DbType.String, direction: ParameterDirection.Input, value: dto.Email);

            var insert = @"INSERT INTO [Ecommerce].[dbo].[ClientsEcommerce] (NomeCompleto, Email)
                        VALUES (@NomeCompleto, @Email)";

            try
            {
                await _unitOfWork.Connection.ExecuteAsync(
                    sql: insert,
                    param: parameters,                    
                    transaction: _unitOfWork.Transaction,
                    commandTimeout: _unitOfWork.CommandTimeout,
                    commandType: CommandType.Text
                ).ConfigureAwait(false);  
                _unitOfWork.Commit();
                                       
                return true;
            }
            catch (Exception)
            {                
                _unitOfWork.RollBack();
                throw;
            }
        }

        public async Task<ClientEcommerceDTO> UpdateClientEcommerceAsync(ClientEcommerceDTO dto)
        {
            var parameters = new DynamicParameters();
            
            parameters.Add(name: "@Id", dbType: DbType.Int32, direction: ParameterDirection.Input, value: dto.Id);
            parameters.Add(name: "@NomeCompleto", dbType: DbType.String, direction: ParameterDirection.Input, value: dto.NomeCompleto);
            parameters.Add(name: "@Email", dbType: DbType.String, direction: ParameterDirection.Input, value: dto.Email);

            var update = @"UPDATE [Ecommerce].[dbo].[ClientsEcommerce] 
                         SET NomeCompleto = @NomeCompleto
                             , Email = @Email
                         WHERE Id = @Id";

            try
            {
                await _unitOfWork.Connection.ExecuteAsync(
                    sql: update,
                    param: parameters,
                    transaction: _unitOfWork.Transaction,
                    commandTimeout: _unitOfWork.CommandTimeout,
                    commandType: CommandType.Text
                ).ConfigureAwait(false);
                _unitOfWork.Commit();

                return await GetClientByIdAsync(dto.Id);
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }            
        }

        public async Task<bool> DeleteClientEcommerceAsync(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add(name: "@Id", dbType: DbType.Int32, direction: ParameterDirection.Input, value: id);

            var delete = @"DELETE FROM [Ecommerce].[dbo].[ClientsEcommerce] WHERE Id = @Id";

            try
            {
                await _unitOfWork.Connection.ExecuteAsync(
                    sql: delete,
                    param: parameters,
                    transaction: _unitOfWork.Transaction,
                    commandTimeout: _unitOfWork.CommandTimeout,
                    commandType: CommandType.Text
                ).ConfigureAwait(false);        
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception)
            {                      
                _unitOfWork.RollBack();
                throw;
            }
        }

        public async Task<bool> CreateOrderAsync(OrderEcommerceDTO dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add(name: "@NomeProduto", dbType: DbType.String, direction: ParameterDirection.Input, value: dto.NomeProduto);
            parameters.Add(name: "@Preco", dbType: DbType.Double, direction: ParameterDirection.Input, value: dto.Preco);
            parameters.Add(name: "@ClientId", dbType: DbType.Int32, direction: ParameterDirection.Input, value: dto.ClientId);

            var insert = @"INSERT INTO [Ecommerce].[dbo].[OrdersEcommerce] (NomeProduto, Preco, ClientId)
                        VALUES (@NomeProduto, @Preco, @CLientId)";

            try
            {
                await _unitOfWork.Connection.ExecuteAsync(
                    sql: insert,
                    param: parameters,
                    transaction: _unitOfWork.Transaction,
                    commandTimeout: _unitOfWork.CommandTimeout,
                    commandType: CommandType.Text
                ).ConfigureAwait(false);
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public async Task<OrderEcommerceDTO> GetOrderByIdAsync(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add(name: "@Id", dbType: DbType.Int32, direction: ParameterDirection.Input, value: id);

            var get = @"SELECT * FROM [Ecommerce].[dbo].[OrdersEcommerce] WHERE Id = @Id";

            try
            {
                var order = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<OrderEcommerceDTO>(
                    sql: get,
                    param: parameters,
                    transaction: _unitOfWork.Transaction,
                    commandTimeout: _unitOfWork.CommandTimeout,
                    commandType: CommandType.Text
                ).ConfigureAwait(false);

                return order;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
