using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServiceB.Application.Book.GetBooks;
using ServiceB.Application.ViewModels;

[ApiController]
[Route("books")]
public class BooksController : ControllerBase
{
    private readonly IMediator _mediator;

    public BooksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IList<BookViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBooks()
    {
        var books = await _mediator.Send(new GetBooksQuery());
        return Ok(books);
    }
}
