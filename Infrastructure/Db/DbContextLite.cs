using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Db;

public class DbContextLite :DbContext
{

    public DbContextLite(DbContextOptions<DbContextLite> db) : base(db)
    {

    }
    public DbSet<Author> Authors { get; set; }

    public DbSet<Post> Posts { get; set; }

    public DbSet<Category> Category { get; set; }




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>().OwnsOne(a => a.Name, name =>
        {
            name.Property(n => n.FirstName).HasColumnName("FirstName");
            name.Property(n => n.LastName).HasColumnName("LastName");
        });

        // Relação Author - Post
        modelBuilder.Entity<Author>()
            .HasMany(a => a.Post)
            .WithOne(c => c.Author)
            .HasForeignKey(c => c.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);



        // Relação Post - Category
        modelBuilder.Entity<Post>()
            .HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict); 

      

        base.OnModelCreating(modelBuilder);
    }
}
