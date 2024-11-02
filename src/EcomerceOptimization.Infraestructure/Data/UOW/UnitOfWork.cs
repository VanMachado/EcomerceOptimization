using EcomerceOptimization.Domain.Interfaces;
using EcomerceOptimization.Infraestructure.Data.Repository;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Reflection;

namespace EcomerceOptimization.Infraestructure.Data.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbConnection _dbConnection = null;
        private IDbTransaction _dbTransaction = null;
        private int _commandTimeout = 0;
        private Guid _id = Guid.Empty;
        Dictionary<string, object> _repository = new Dictionary<string, object>();   

        public UnitOfWork(IDbConnection connection)
        {
            _id = Guid.NewGuid();
            _dbConnection = connection;
        }

        public UnitOfWork(IDbConnection connection, int commandTimeout)
        {
            _id = Guid.NewGuid();
            _dbConnection = connection;
            _commandTimeout = commandTimeout;
        }

        IDbConnection IUnitOfWork.Connection { get { return _dbConnection; } }
        IDbTransaction IUnitOfWork.Transaction { get { return _dbTransaction; } }
        Guid IUnitOfWork.Id { get { return _id; } }
        public int CommandTimeout { get { return _commandTimeout; } }

        public T GetRepository<T>()
        {
            var instance = CreateInstance<T>();
            return (T)_repository[instance.ToString()];
        }

        public void SetRepository(object repository)
        {
            if(!_repository.ContainsKey(repository.ToString()))
                _repository.Add(repository.ToString(), repository);
        }

        private object CreateInstance<T>()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(T));

            return assembly.CreateInstance(typeof(T).FullName, false);
        }

        public void BeginTransaction()
        {
            _dbTransaction = _dbConnection.BeginTransaction();
        }

        public void Commit()
        {
            _dbTransaction.Commit();
            Dispose();
        }

        public void Dispose()
        {
            if (_dbTransaction != null)
                _dbTransaction.Dispose();

            _dbTransaction = null;
        }

        public void RollBack()
        {
            _dbTransaction.Rollback();
            Dispose();
        }        
    }
}
