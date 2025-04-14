using MediatR;
using ServiceB.Application.ViewModels;

namespace ServiceB.Application.Book.GetBooks;

public record GetBooksQuery : IRequest<IList<BookViewModel>>;