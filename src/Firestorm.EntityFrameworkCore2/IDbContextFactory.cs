using Microsoft.EntityFrameworkCore;

namespace Firestorm.EntityFrameworkCore2
{
    public interface IDbContextFactory<out TDbContext>
        where TDbContext : DbContext
    {
        TDbContext Create();
    }
}