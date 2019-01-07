using Microsoft.EntityFrameworkCore;

namespace Firestorm.EntityFrameworkCore2
{
    internal interface IDbContextFactory<out TDbContext>
        where TDbContext : DbContext
    {
        TDbContext Create();
    }
}