using MediatR;
using ServiceB.Application.Mappers;
using ServiceB.Application.ViewModels;
using ServiceB.Infrastructure.Persistence;

namespace ServiceB.Application.Auhtor.GetAuthors;

public class GetAuthorsHanlder : IRequestHandler<GetAuthorsQuery, IList<AuthorViewModel>>
{
    private readonly ILogger<GetAuthorsHanlder> _logger;
    private readonly IAuthorRepository _authorRepository;

    public GetAuthorsHanlder(ILogger<GetAuthorsHanlder> logger, IAuthorRepository authorRepository)
    {
        _logger = logger;
        _authorRepository = authorRepository;
    }

    public async Task<IList<AuthorViewModel>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get authors");

        var authors = await _authorRepository
            .GetAuthorsAsync(cancellationToken)
            .ConfigureAwait(false);

        return authors.ToViewModel();
    }
}