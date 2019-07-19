using BCSTransfer.Core;
using BCSTransfer.Core.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BCSTransfer.Sqlite
{
    public class PluginProvider : IPluginProvider
    {
        public string PluginNamespace => "SQLITE";

        public void Register(ITypeContainer typeContainer, string pluginNamespace)
        {
            typeContainer.Register<IDatabaseProvider, DatabaseProvider>();

            typeContainer.Register<IDatabase, SqliteDatabase>(InstanceBehaviour.Singleton);
            typeContainer.Register<SqliteDatabase, SqliteDatabase>(InstanceBehaviour.Singleton);
        }

        public Task Initalize()
        {
            return Task.CompletedTask;
        }        

        public Task UnInitalize()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }

        private class DatabaseProvider : IDatabaseProvider
        {
            public DbContextOptions GetDatabaseContext(string source)
            {
                var builder = new DbContextOptionsBuilder();
                builder.UseSqlite($"Data Source={source}");
                return builder.Options;
            }
        }
    }
}
