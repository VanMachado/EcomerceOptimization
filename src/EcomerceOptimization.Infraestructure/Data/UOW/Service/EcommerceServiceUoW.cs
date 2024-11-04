using EcomerceOptimization.Infraestructure.Data.Repository;
using System.Data.SqlClient;
using System.Data;
using AutoMapper;

namespace EcomerceOptimization.Infraestructure.Data.UOW.Service
{
    public class EcommerceServiceUoW
    {
        private static string _connectionStringName = "EO-DEV-SQL";
        private static IDbConnection _connection = null;
        private static UnitOfWork _unitOfWork = null;
        private static IMapper _mapper;

        public static UnitOfWork GetUnitOfWork()
        {
            if (_unitOfWork == null)
            {
                _connection = new SqlConnection(UnitOfWorkConnectionStringPool.GetConnectionString(_connectionStringName));
                _connection.Open();

                _unitOfWork = new UnitOfWork(_connection, UnitOfWorkConnectionStringPool.GetConnectionTimeout(_connectionStringName));
                _unitOfWork.SetRepository(new EcommerceRepository(_unitOfWork, _mapper));
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
