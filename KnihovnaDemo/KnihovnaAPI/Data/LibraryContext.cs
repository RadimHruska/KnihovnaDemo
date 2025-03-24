using KnihovnaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace KnihovnaAPI.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Lend> Lends { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Konfigurace vztahů mezi entitami
            modelBuilder.Entity<Lend>()
                .HasOne(l => l.User)
                .WithMany(u => u.Lends)
                .HasForeignKey(l => l.IdUser);

            modelBuilder.Entity<Lend>()
                .HasOne(l => l.Book)
                .WithMany(b => b.Lends)
                .HasForeignKey(l => l.IdBook);
                
            // Konfigurace indexů pro lepší výkon
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Name)
                .IsUnique();
                
            modelBuilder.Entity<Book>()
                .HasIndex(b => b.Name);
                
            modelBuilder.Entity<Lend>()
                .HasIndex(l => new { l.IdUser, l.IdBook });
        }
    }
} 