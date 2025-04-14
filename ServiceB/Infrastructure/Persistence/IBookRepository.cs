using ServiceB.Domain;

namespace ServiceB.Infrastructure.Persistence;

public interface IBookRepository
{
    Task<Book?> GetBookByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IList<Book>> GetBooksAsync(CancellationToken cancellationToken);
}