using Microsoft.EntityFrameworkCore;
using ServiceB.Domain;

namespace ServiceB.Infrastructure;

public static class DataSeedExtension
{
    public static async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await dbContext.Database.EnsureCreatedAsync();

        if (await dbContext.Authors.AnyAsync())
            return;

        // Author data
        var authors = new List<Author>
        {
            new Author
            {
                Id = Guid.NewGuid(),
                Firstname = "John",
                Lastname = "Doe",
                Picture = "https://example.com/johndoe.jpg"
            },
            new Author
            {
                Id = Guid.NewGuid(),
                Firstname = "Jane",
                Lastname = "Smith",
                Picture = "https://example.com/janesmith.jpg"
            }
        };

        // Book data
        var books = new List<Book>
        {
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "Book 1",
                Description = "Description for Book 1",
                Price = 19.99m,
                PublicationDate = DateTime.UtcNow,
                ISBN = "978-3-16-148410-0",
                Language = "English",
                AuthorId = authors[0].Id
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "Book 2",
                Description = "Description for Book 2",
                Price = 29.99m,
                PublicationDate = DateTime.UtcNow,
                ISBN = "978-1-234-56789-7",
                Language = "English",
                AuthorId = authors[1].Id
            }
        };

        await dbContext.Authors.AddRangeAsync(authors);
        await dbContext.Books.AddRangeAsync(books);
        await dbContext.SaveChangesAsync();
    }
}