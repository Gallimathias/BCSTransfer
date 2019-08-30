using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading;
using System.Threading.Tasks;

namespace BCSTransfer.Core.Model
{
    public interface IDatabase
    {
        DbSet<Setting> Settings { get; }
        DbSet<Contact> Contacts { get; }

        IDatabase GetEnsureDatabase(string source);

        bool EnsureCreated();
        Task<int> SaveChanges(CancellationToken token);
        EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;
        EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class;
    }
}