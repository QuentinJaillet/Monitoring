using Microsoft.EntityFrameworkCore;
using ServiceB.Domain;

namespace ServiceB.Infrastructure.Persistence;

public class AuthorRepository : IAuthorRepository
{
    private readonly ApplicationDbContext _dbContext;

    public AuthorRepository(ApplicationDbContext context)
    {
        _dbContext = context;
    }

    public async Task<Author?> GetAuthorByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Authors
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<IList<Author>> GetAuthorsAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Authors
            .AsNoTracking()
            .OrderBy(s => s.Firstname)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<bool> VerifyIfAuthorExistsAsync(string firstname, string lastname, CancellationToken cancellationToken)
    {
        return await _dbContext.Authors
            .AnyAsync(a => a.Firstname == firstname && a.Lastname == lastname, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<Guid> AddAuthorAsync(Author author, CancellationToken cancellationToken)
    {   
        await _dbContext.Authors
            .AddAsync(author, cancellationToken)
            .ConfigureAwait(false);
        
        await _dbContext
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);
        
        return author.Id;
    }
}