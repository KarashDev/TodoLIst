using Microsoft.EntityFrameworkCore;

namespace TodoLIst.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<TodoEntry> TodoEntries { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
           : base(options)
        {
            Database.EnsureCreated();
        }


    }
}
