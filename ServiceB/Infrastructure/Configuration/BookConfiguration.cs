using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceB.Domain;

namespace ServiceB.Infrastructure.Configuration;

public sealed class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(x => x.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
        
        builder.Property(x => x.PublicationDate)
            .IsRequired();
        
        builder.Property(x => x.ISBN)
            .IsRequired()
            .HasMaxLength(13);
        
        builder.Property(x => x.Language)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.HasOne(x => x.Author)
            .WithMany(x => x.Books)
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasIndex(x => x.ISBN)
            .IsUnique();
    }
}