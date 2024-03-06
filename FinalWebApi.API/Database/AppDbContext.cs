using Microsoft.EntityFrameworkCore;
using FinalWebApi.API.Models;

namespace FinalWebApi.API.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // add models to database context
        public virtual DbSet<BookModel> Books { get; set; }
        public virtual DbSet<UserModel> Users { get; set; }
    }
}