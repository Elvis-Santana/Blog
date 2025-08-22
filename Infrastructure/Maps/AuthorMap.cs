using Domain.Entities;
using Domain.ObjectValues;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Maps;

public class AuthorMap : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {

        Author[] list = [
             new Author(new FullName("ELvis","san")),
             new Author(new FullName("Rene","san"))
        ];
       

        builder.HasData(list);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

     

        builder.OwnsOne(a => a.Name, name =>
        {
            name.Property(n => n.FirstName).HasColumnName("FirstName");
            name.Property(n => n.LastName).HasColumnName("LastName");
        });

        builder.HasMany(a => a.Post)
         .WithOne(c => c.Author)
         .HasForeignKey(c => c.AuthorId).IsRequired(false)
         .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Name.FirstName)
         .HasMaxLength(50);

        builder.Property(x => x.Name.LastName)
         .IsRequired(false)
         .HasMaxLength(50);
    }
}
