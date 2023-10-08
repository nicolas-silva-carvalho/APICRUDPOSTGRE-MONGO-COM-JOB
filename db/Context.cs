using System.Reflection;
using BookStoreApi.Models;
using Microsoft.EntityFrameworkCore;
using teste.model;

namespace teste.db;

public class Context : DbContext
{
    

    public Context(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<BookContrato> bookContratos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Book>().HasKey(t => t.Id);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

}