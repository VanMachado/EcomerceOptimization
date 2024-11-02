using System.Data;

namespace EcomerceOptimization.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Guid Id { get; }
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }
        int CommandTimeout { get; }
        void BeginTransaction();
        void Commit();
        void RollBack();        
    }
}
