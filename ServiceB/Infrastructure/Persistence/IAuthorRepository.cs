using ServiceB.Domain;

namespace ServiceB.Infrastructure.Persistence;

public interface IAuthorRepository
{
    Task<Author?> GetAuthorByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IList<Author>> GetAuthorsAsync(CancellationToken cancellationToken);
    Task<bool> VerifyIfAuthorExistsAsync(string firstname, string lastname, CancellationToken cancellationToken);
    Task<Guid> AddAuthorAsync(Author author, CancellationToken cancellationToken);
}       