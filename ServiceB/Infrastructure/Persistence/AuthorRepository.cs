using Microsoft.EntityFrameworkCore;
using ServiceB.Domain;

namespace ServiceB.Infrastructure.Persistence;

public class AuthorRepository : IAuthorRepository
{
    private readonly ApplicationDbContext _context;

    public AuthorRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Author> GetAuthorByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IList<Author>> GetAuthorsAsync(CancellationToken cancellationToken)
    {
        return await _context.Authors
            .AsNoTracking()
            .OrderBy(s => s.Firstname)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}