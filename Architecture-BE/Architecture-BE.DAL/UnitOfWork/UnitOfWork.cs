using Architecture_BE.DAL.DataContext;
using Architecture_BE.DAL.Repositories;
using System;
using System.Threading.Tasks;

namespace Architecture_BE.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbArchitectureContext _context;
        private bool _disposed = false;
        private UserRepository _userRepository;
        private TokenRepository _tokenRepository;
        //private Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public UnitOfWork(DbArchitectureContext context)
        {
            _context = context;
            // InitRepositories();
        }

        //private IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : EntityBase
        //{
        //    var typeEntity = typeof(TEntity);

        //    // Check Instance in dictonary
        //    if (_repositories.Keys.Contains(typeEntity))
        //        return _repositories[typeEntity] as IGenericRepository<TEntity>;

        //    var repository = new GenericRepository<TEntity>(_context);

        //    // Add it to the dictionary
        //    _repositories.Add(typeEntity, repository);

        //    return repository;
        //}

        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_context);
                }

                return _userRepository;
            }
        }

        public ITokenRepository TokenRepository
        {
            get
            {
                if (_tokenRepository == null)
                {
                    _tokenRepository = new TokenRepository(_context);
                }

                return _tokenRepository;
            }
        }

        public async Task<bool> CommitAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
