namespace ServiceB.Domain;

public class Author
{
    public Guid Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Picture { get; set; }
    public ICollection<Book> Books { get; set; } = new List<Book>();
}