using ServiceB.Domain;

namespace ServiceB.Infrastructure;

public static class DataSeedExtension
{
    public static async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

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

        await dbContext.Authors.AddRangeAsync(authors);
        await dbContext.SaveChangesAsync();
    }
}