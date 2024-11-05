using EcomerceOptimization.Infraestructure.Data.Repository;
using System.Data.SqlClient;
using System.Data;

namespace EcomerceOptimization.Infraestructure.Data.UOW.Service
{
    public class OrderEcommerceServiceUoW
    {
        private static string _connectionStringName = "EO-DEV-SQL";
        private static IDbConnection _connection = null;
        private static IDbTransaction _transaction = null;
        private static UnitOfWork _unitOfWork = null;

        public static UnitOfWork GetUnitOfWork()
        {
            if (_unitOfWork == null)
            {                
                _connection = new SqlConnection(UnitOfWorkConnectionStringPool.GetConnectionString(_connectionStringName));
                _connection.Open();

                _transaction = _connection.BeginTransaction();

                _unitOfWork = new UnitOfWork(_connection, _transaction, UnitOfWorkConnectionStringPool.GetConnectionTimeout(_connectionStringName));
                _unitOfWork.SetRepository(new EcommerceRepository(_unitOfWork));
            }

            return _unitOfWork;
        }

        public static void ResetUnitOfWork()
        {
            _unitOfWork?.Dispose();
            _connection?.Dispose();

            _connection = new SqlConnection(UnitOfWorkConnectionStringPool.GetConnectionString(_connectionStringName));
            _connection.Open();
            _transaction = _connection.BeginTransaction();

            _unitOfWork = new UnitOfWork(_connection, _transaction, UnitOfWorkConnectionStringPool.GetConnectionTimeout(_connectionStringName));
            _unitOfWork.SetRepository(new EcommerceRepository(_unitOfWork));
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
            _connection.Dispose();
        }
    }
}
