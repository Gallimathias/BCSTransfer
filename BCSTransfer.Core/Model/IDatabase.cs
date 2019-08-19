using Microsoft.EntityFrameworkCore;

namespace BCSTransfer.Core.Model
{
    public interface IDatabase
    {
        DbSet<Setting> Settings { get; }
        DbSet<Contact> Contacts { get; }

        bool EnsureCreated();
        IDatabase GetEnsureDatabase(string source);
    }
}