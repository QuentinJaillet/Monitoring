using ServiceB.Domain;

namespace ServiceB.Infrastructure.Persistence;

public interface IAuthorRepository
{
    Task<Author> GetAuthorByIdAsync(Guid id);
    Task<IList<Author>> GetAuthorsAsync(CancellationToken cancellationToken);
}