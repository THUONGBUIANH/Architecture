using Architecture_BE.DAL.Repositories;
using System;
using System.Threading.Tasks;

namespace Architecture_BE.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        ITokenRepository TokenRepository { get; }
        Task<bool> CommitAsync();
    }
}
