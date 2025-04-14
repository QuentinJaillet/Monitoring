﻿using Microsoft.EntityFrameworkCore;
using ServiceB.Domain;

namespace ServiceB.Infrastructure.Persistence;

public class AuthorRepository : IAuthorRepository
{
    private readonly ApplicationDbContext _context;

    public AuthorRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Author?> GetAuthorByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Authors
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken)
            .ConfigureAwait(false);
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