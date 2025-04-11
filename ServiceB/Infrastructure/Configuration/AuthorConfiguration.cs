using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceB.Domain;

namespace ServiceB.Infrastructure.Configuration;

public sealed class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.ToTable("Authors");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Firstname)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(x => x.Lastname)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Picture)
            .HasMaxLength(200);
        
        builder.HasMany(x => x.Books)
            .WithOne(x => x.Author)
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}