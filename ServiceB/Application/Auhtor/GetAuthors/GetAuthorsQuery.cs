using MediatR;
using ServiceB.Application.ViewModels;

namespace ServiceB.Application.Auhtor.GetAuthors;

public record GetAuthorsQuery : IRequest<IList<AuthorViewModel>>;