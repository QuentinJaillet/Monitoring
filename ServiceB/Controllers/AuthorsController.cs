using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServiceB.Application.Auhtor.AddAuthor;
using ServiceB.Application.Auhtor.GetAuthor;
using ServiceB.Application.Auhtor.GetAuthors;
using ServiceB.Application.ViewModels;

[ApiController]
[Route("authors")]
public class AuthorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IList<AuthorViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAuthors()
    {
        var authors = await _mediator.Send(new GetAuthorsQuery());
        return Ok(authors);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddAuthor([FromQuery] string firstname, [FromQuery] string lastname, [FromQuery] string picture)
    {
        var authorId = await _mediator.Send(new AddAuthorCommand(firstname, lastname, picture));

        return authorId == Guid.Empty
            ? BadRequest("Author already exists")
            : Created($"/authors/{authorId}", authorId);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AuthorViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAuthor(Guid id)
    {
        var author = await _mediator.Send(new GetAuthorQuery(id));
        return Ok(author);
    }
}
