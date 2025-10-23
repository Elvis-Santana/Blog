using Domain.Entities;
using Domain.ObjectValues;
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
       
        modelBuilder.Entity<Author>().OwnsOne(a => a.Name, name =>
        {
            name.Property(n => n.FirstName).HasColumnName("FirstName").HasMaxLength(100);
            name.Property(n => n.LastName).HasColumnName("LastName").HasMaxLength(100).IsRequired(false);
        });

        modelBuilder.Entity<Author>(builder =>
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Ignore(x => x.Email);


            builder.HasMany(a => a.Post)
                .WithOne(c => c.Author)
                .HasForeignKey(c => c.AuthorId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);



        });

        modelBuilder.Entity<Post>(post =>
        {
            post.HasOne(p => p.Category)
                .WithMany()
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
