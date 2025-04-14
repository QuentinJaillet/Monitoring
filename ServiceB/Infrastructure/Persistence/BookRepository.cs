using Microsoft.EntityFrameworkCore;
using ServiceB.Domain;

namespace ServiceB.Infrastructure.Persistence;

public class BookRepository : IBookRepository
{
    private readonly ApplicationDbContext _context;

    public BookRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Book?> GetBookByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Books
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<IList<Book>> GetBooksAsync(CancellationToken cancellationToken)
    {
        return await _context.Books
            .AsNoTracking()
            .OrderBy(s => s.Title)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}