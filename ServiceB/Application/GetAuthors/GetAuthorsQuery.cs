using MediatR;

namespace ServiceB.Application.GetAuthors;

public record GetAuthorsQuery : IRequest<IList<AuthorViewModel>>;