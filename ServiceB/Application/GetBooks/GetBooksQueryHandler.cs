using MediatR;
using ServiceB.Application.Mappers;
using ServiceB.Application.ViewModels;
using ServiceB.Infrastructure.Persistence;

namespace ServiceB.Application.GetBooks;

public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, IList<BookViewModel>>
{
    private ILogger<GetBooksQueryHandler> _logger;
    private readonly IBookRepository _bookRepository;

    public GetBooksQueryHandler(ILogger<GetBooksQueryHandler> logger, IBookRepository bookRepository)
    {
        _logger = logger;
        _bookRepository = bookRepository;
    }

    public async Task<IList<BookViewModel>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get books");
        var books = await _bookRepository.GetBooksAsync(cancellationToken).ConfigureAwait(false);
        return books.ToViewModel();
    }
}