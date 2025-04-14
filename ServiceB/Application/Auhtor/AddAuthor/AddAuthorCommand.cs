using MediatR;

namespace ServiceB.Application.Auhtor.AddAuthor;

public record AddAuthorCommand(string Firstname, string Lastname, string Picture) : IRequest<Guid>;