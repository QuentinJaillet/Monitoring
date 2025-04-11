namespace ServiceB.Domain;

public class Book
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Author Author { get; set; }
    public Guid AuthorId { get; set; }
    public Decimal Price { get; set; }
    public DateTime PublicationDate { get; set; }
    public string ISBN { get; set; }
    public string Language { get; set; }
}