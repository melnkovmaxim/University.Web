using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace University.Repositories.Interfaces
{
    public interface IRepository<TEntity>: IDisposable 
        where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> GetById(int id);
        Task Create(TEntity entity);
        Task Update(TEntity entity);
        Task AddOrUpdate(TEntity entity);
        Task Delete(TEntity entity);
        Task DeleteById(int id);
    }
}
