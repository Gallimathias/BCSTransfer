using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BCSTransfer.Core.Model
{
    public abstract class Database : DbContext, IDatabase
    {
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        public Database(DbContextOptions options) : base(options)
        {
        }

        public bool EnsureCreated() 
            => Database.EnsureCreated();

        public abstract IDatabase GetEnsureDatabase(string source);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
    }
}
