using Architecture_BE.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Architecture_BE.DAL.DataContext
{
    public class DbArchitectureContext: DbContext
    {
        public DbArchitectureContext(DbContextOptions<DbArchitectureContext> options)
      : base(options)
        { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Token> Tokens { get; set; }
    }
}
