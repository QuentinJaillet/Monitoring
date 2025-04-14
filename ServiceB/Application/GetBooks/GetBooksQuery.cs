using MediatR;
using ServiceB.Application.ViewModels;

namespace ServiceB.Application.GetBooks;

public record GetBooksQuery : IRequest<IList<BookViewModel>>;