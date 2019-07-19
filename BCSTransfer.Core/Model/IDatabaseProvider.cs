using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BCSTransfer.Core.Model
{
    public interface IDatabaseProvider
    {
        DbContextOptions GetDatabaseContext(string source);
    }
}
