using Architecture_BE.DAL.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Architecture_BE.DAL.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : EntityBase
    {
        IQueryable<TEntity> FindAll();
        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression);
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
