﻿using EcomerceOptimization.Infraestructure.Data.Repository;
using System.Data.SqlClient;
using System.Data;
using AutoMapper;
using EcomerceOptimization.Domain.Interfaces;
using System.Transactions;

namespace EcomerceOptimization.Infraestructure.Data.UOW.Service
{
    public class ClientEcommerceServiceUoW
    {
        private static string _connectionStringName = "EO-DEV-SQL";
        private static IDbConnection _connection = null;        
        private static UnitOfWork _unitOfWork = null;

        public static UnitOfWork GetUnitOfWork()
        {

            if(_unitOfWork == null)
            {                
                _connection = new SqlConnection(UnitOfWorkConnectionStringPool.GetConnectionString(_connectionStringName));
                _connection.Open();
                                
                _unitOfWork = new UnitOfWork(_connection, UnitOfWorkConnectionStringPool.GetConnectionTimeout(_connectionStringName));
                _unitOfWork.SetRepository(new EcommerceRepository(_unitOfWork));
            }            

            return _unitOfWork;
        }
               
        public void Dispose()
        {
            _unitOfWork.Dispose();
            _connection.Dispose();
        }
    }
}