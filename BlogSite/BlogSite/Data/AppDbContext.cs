using BlogSite.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Data
    {
    public class AppDbContext : DbContext
        {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
            {
            }
        public DbSet<Post> tbl_Posts { get; set; }
        public DbSet<Profile> tbl_Profiles { get; set; }

        }
        }




