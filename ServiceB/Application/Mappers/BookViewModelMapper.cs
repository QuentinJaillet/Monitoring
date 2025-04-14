using ServiceB.Application.ViewModels;

namespace ServiceB.Application.Mappers;

public static class BookViewModelMapper
{
    public static BookViewModel ToViewModel(this Domain.Book book)
    {
        return new BookViewModel
        {
            Id = book.Id,
            Title = book.Title,
            Description = book.Description,
            Author = book.Author.ToViewModel(),
            Price = book.Price,
            PublicationDate = book.PublicationDate,
            ISBN = book.ISBN,
            Language = book.Language
        };
    }

    public static IList<BookViewModel> ToViewModel(this IList<Domain.Book> books)
    {
        return books.Select(x => x.ToViewModel()).ToList();
    }
}