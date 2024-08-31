using ChatApp_Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatApp_Api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<AppUser> Users { get; set; }

    }
}
