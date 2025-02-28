using Microsoft.EntityFrameworkCore;
using TP_TDD_Bibliotheque.Models;

namespace TP_TDD_Bibliotheque
{
    public class AppDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Editor> Editors { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasIndex(b => b.ISBN)
                .IsUnique();

            modelBuilder.Entity<Book>()
                .Property(b => b.EditorId)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}