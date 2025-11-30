using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using CeraPressingManager.Data;

namespace CeraPressingManager
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PressingDbContext>
    {
        public PressingDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PressingDbContext>();
            optionsBuilder.UseSqlite("Data Source=cerapressing.db");
            return new PressingDbContext(optionsBuilder.Options);
        }
    }
}