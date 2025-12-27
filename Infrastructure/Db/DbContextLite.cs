using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Db;

public class DbContextLite :DbContext
{

    public DbContextLite(DbContextOptions<DbContextLite> db) : base(db)
    {

    }
    public DbSet<Author> Authors { get; set; }

    public DbSet<Post> Posts { get; set; }

    public DbSet<Category> Category { get; set; }

    public DbSet<Follow> Followers { get; set; }
    public DbSet<Notification> Notifications { get; set; }




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

            builder.Property(a => a.Email)
            .IsRequired(true)
            .HasMaxLength(255);


            builder.HasMany(a => a.Post)
                .WithOne(c => c.Author)
                .HasForeignKey(c => c.AuthorId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Follow>(builder =>
        {
            builder.HasKey(x => new { x.FollowerId, x.FollowingId });

            builder.HasOne(x => x.Follower)
                         .WithMany(a => a.Following)
                         .HasForeignKey(x => x.FollowerId)
                         .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Following)
                   .WithMany(a => a.Followers)
                   .HasForeignKey(x => x.FollowingId)
                   .OnDelete(DeleteBehavior.Restrict);


            builder.Property(a => a.FollowerId)
            .IsRequired();

            builder.Property(a => a.FollowingId)
            .IsRequired();

        });

        modelBuilder.Entity<Notification>(builder =>
        {
            builder.Property(n => n.Id)
            .IsRequired(true);

            builder.Property(n => n.Messsage)
            .IsRequired(true);


            builder.Property(n => n.UserId)
            .IsRequired(true);

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
