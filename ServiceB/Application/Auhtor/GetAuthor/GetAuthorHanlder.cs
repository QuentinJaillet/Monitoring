using MediatR;
using ServiceB.Application.Mappers;
using ServiceB.Application.ViewModels;
using ServiceB.Infrastructure.Persistence;

namespace ServiceB.Application.Auhtor.GetAuthor;

public class GetAuthorHanlder : IRequestHandler<GetAuthorQuery, AuthorViewModel>
{
    private readonly ILogger<GetAuthorHanlder> _logger;
    private readonly IAuthorRepository _authorRepository;

    public GetAuthorHanlder(ILogger<GetAuthorHanlder> logger, IAuthorRepository authorRepository)
    {
        _logger = logger;
        _authorRepository = authorRepository;
    }

    public async Task<AuthorViewModel> Handle(GetAuthorQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get author with id {Id}", request.Id);

        var author = await _authorRepository
            .GetAuthorByIdAsync(request.Id, cancellationToken)
            .ConfigureAwait(false);
        
        return author == null ? 
            throw new InvalidOperationException("Author not found") : 
            author.ToViewModel();
    }
}