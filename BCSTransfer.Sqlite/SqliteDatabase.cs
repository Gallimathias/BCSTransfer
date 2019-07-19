using BCSTransfer.Core.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BCSTransfer.Sqlite
{
    class SqliteDatabase : Database
    {
        static SqliteDatabase() 
            => SQLitePCL.Batteries.Init();

        public SqliteDatabase(DbContextOptions options) : base(options)
        {

        }

        public override IDatabase GetEnsureDatabase(string source)
        {
            var db = GetDatabase(source) as Database;
            db.Database.EnsureCreated();
            return db;
        }

        public static IDatabase GetDatabase(string source)
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseSqlite($"Data Source={source}");
            return new SqliteDatabase(builder.Options);
        }
    }
}
