using MediatR;
using ServiceB.Application.ViewModels;

namespace ServiceB.Application.GetAuthors;

public record GetAuthorsQuery : IRequest<IList<AuthorViewModel>>;