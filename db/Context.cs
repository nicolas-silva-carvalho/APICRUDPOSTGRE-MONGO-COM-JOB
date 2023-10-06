using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreApi.Models;
using Microsoft.EntityFrameworkCore;

namespace teste.db;

public class Context : DbContext
{
    

    public Context(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Book>().HasKey(t => t.Id);
    }

}