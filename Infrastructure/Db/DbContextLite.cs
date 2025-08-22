using Domain.Entities;
using Infrastructure.Maps;
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

        modelBuilder.ApplyConfiguration(new AuthorMap());
        
      
        // Relação Author - Post
     



        // Relação Post - Category
        modelBuilder.Entity<Post>(post =>
        {
            post.HasOne(p => p.Category)
             .WithMany() //Indica que uma Category pode estar associada a muitos Posts.
             .HasForeignKey(p => p.CategoryId)
             .IsRequired(false)
             .OnDelete(DeleteBehavior.Restrict);

        
        });



        modelBuilder.Entity<Category>(post =>
        {
            post.HasOne(p => p.Author)
             .WithMany() 
             .HasForeignKey(p => p.AuthorId)
             .OnDelete(DeleteBehavior.Cascade);


        });

       






        base.OnModelCreating(modelBuilder);
    }
}
