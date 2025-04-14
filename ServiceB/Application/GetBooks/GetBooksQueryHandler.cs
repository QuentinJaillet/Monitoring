using MediatR;
using ServiceB.Application.Mappers;
using ServiceB.Application.ViewModels;
using ServiceB.Infrastructure.Persistence;

namespace ServiceB.Application.GetBooks;

public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, IList<BookViewModel>>
{
    private ILogger<GetBooksQueryHandler> _logger;
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;

    public GetBooksQueryHandler(ILogger<GetBooksQueryHandler> logger, IBookRepository bookRepository, IAuthorRepository authorRepository)
    {
        _logger = logger;
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
    }

    public async Task<IList<BookViewModel>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get books");
        
        // Get authors to populate the book's author
        var authors = await _authorRepository
            .GetAuthorsAsync(cancellationToken)
            .ConfigureAwait(false);
        
        // Get books
        var books = await _bookRepository
            .GetBooksAsync(cancellationToken)
            .ConfigureAwait(false);
        
        // Populate the book's author
        foreach (var book in books)
        {
            var author = authors.FirstOrDefault(a => a.Id == book.AuthorId);
            if (author != null)
                book.Author = author;
        }
        
        return books.ToViewModel();
    }
}