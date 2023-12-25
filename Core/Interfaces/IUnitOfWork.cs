using Core.Models;

namespace Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseModel;    //genericRepo interface
        Task<int> Complete();   //return number of changes to db
    }
}
