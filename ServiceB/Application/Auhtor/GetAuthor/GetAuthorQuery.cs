using MediatR;
using ServiceB.Application.ViewModels;

namespace ServiceB.Application.Auhtor.GetAuthor;

public record GetAuthorQuery(Guid Id) : IRequest<AuthorViewModel>;