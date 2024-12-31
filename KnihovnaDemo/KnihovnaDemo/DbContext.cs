using KnihovnaDemo.Models;
using Microsoft.EntityFrameworkCore;

public class LibraryContext : DbContext
{
    public DbSet<Lend> Lends { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=KnihovnaDemo;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
    }
}
