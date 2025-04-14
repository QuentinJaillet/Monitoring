using MediatR;
using ServiceB.Infrastructure.Persistence;

namespace ServiceB.Application.Auhtor.AddAuthor;

public class AddAuthorHanlder : IRequestHandler<AddAuthorCommand, Guid>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly ILogger<AddAuthorHanlder> _logger;

    public AddAuthorHanlder(IAuthorRepository authorRepository, ILogger<AddAuthorHanlder> logger)
    {
        _authorRepository = authorRepository;
        _logger = logger;
    }

    public async Task<Guid> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
    {
        // Verify that the author does not already exist
        var existingAuthor = await _authorRepository
            .VerifyIfAuthorExistsAsync(request.Firstname, request.Lastname, cancellationToken)
            .ConfigureAwait(false);

        if (existingAuthor)
        {
            _logger.LogWarning("Author already exists");
            throw new InvalidOperationException("Author already exists");
        }

        _logger.LogInformation("Add author");

        var author = new Domain.Author
        {
            Id = Guid.NewGuid(),
            Firstname = request.Firstname,
            Lastname = request.Lastname,
            Picture = request.Picture
        };

        await _authorRepository
            .AddAuthorAsync(author, cancellationToken)
            .ConfigureAwait(false);

        return author.Id;
    }
}