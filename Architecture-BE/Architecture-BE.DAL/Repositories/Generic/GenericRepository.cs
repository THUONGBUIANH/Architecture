using Architecture_BE.DAL.DataContext;
using Architecture_BE.DAL.Entities;
using Architecture_BE.Helper.Config;
using Architecture_BE.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Architecture_BE.DAL.Repositories
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : EntityBase
    {
        public readonly DbArchitectureContext _context;
        public GenericRepository(DbArchitectureContext context)
        {
            _context = context;
        }
        public IQueryable<TEntity> FindAll()
        {
            return _context.Set<TEntity>();
        }
        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> expression)
        {
            return _context.Set<TEntity>().Where(expression);
        }
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _context.Set<TEntity>().AnyAsync(expression);
        }
        public void Create(TEntity entity)
        {
            entity.Id = Guid.NewGuid();
            entity.CreatedDate = entity.UpdatedDate = DateTime.UtcNow;
            entity.CreatedBy = entity.UpdatedBy = Config.UserName;
            entity.IsDeleted = StatusEnum.Active;

            _context.Entry<TEntity>(entity).State = EntityState.Added;
        }
        public void Update(TEntity entity)
        {
            entity.UpdatedDate = DateTime.UtcNow;
            entity.UpdatedBy = Config.UserName;

            _context.Attach<TEntity>(entity);
            _context.Entry<TEntity>(entity).State = EntityState.Modified;
        }
        public void Delete(TEntity entity)
        {
            entity.IsDeleted = StatusEnum.InActive;
            Update(entity);
        }
    }
}
