using Microsoft.EntityFrameworkCore;
using TP_TDD_Bibliotheque.Models;

namespace TP_TDD_Bibliotheque
{
    public class AppDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}