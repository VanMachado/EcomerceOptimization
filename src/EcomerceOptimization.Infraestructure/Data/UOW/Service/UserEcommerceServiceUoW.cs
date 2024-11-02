using EcomerceOptimization.Infraestructure.Data.Repository;
using System.Data.SqlClient;
using System.Data;

namespace EcomerceOptimization.Infraestructure.Data.UOW.Service
{
    public class UserEcommerceServiceUoW
    {
        private static string _connectionStringName = "EO-DEV-SQL";
        private static IDbConnection _connection = null;
        private static UnitOfWork _unitOfWork = null;

        public static UnitOfWork GetUnitOfWork()
        {
            if (_unitOfWork == null)
            {
                _connection = new SqlConnection(UnitOfWorkConnectionStringPool.GetConnectionString(_connectionStringName));
                _connection.Open();

                _unitOfWork = new UnitOfWork(_connection, 90);
                _unitOfWork.SetRepository(new UserEcommerceRepository(_unitOfWork));
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
