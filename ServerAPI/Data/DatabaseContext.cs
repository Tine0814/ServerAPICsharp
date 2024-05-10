using Microsoft.EntityFrameworkCore;
using ServerAPI.Models;

namespace ServerAPI.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            
        }

        public DbSet<RegistrationNewUser> Users { get; set; }
        public DbSet<RegistrationNewUser> Writers { get; set; }
    }
}
