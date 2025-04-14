using ServiceB.Application.ViewModels;

namespace ServiceB.Application.Mappers;

public static class AuthorViewModelMapper
{
    public static AuthorViewModel ToViewModel(this Domain.Author author)
    {
        return new AuthorViewModel
        {
            Id = author.Id,
            Firstname = author.Firstname,
            Lastname = author.Lastname,
            Picture = author.Picture,
            //BooksCount = author.Books.Count
        };
    }

    public static IList<AuthorViewModel> ToViewModel(this IList<Domain.Author> authors)
    {
        return authors.Select(x => x.ToViewModel()).ToList();
    }
}