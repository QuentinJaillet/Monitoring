﻿namespace ServiceA.ViewModels;

public class BookViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public AuthorViewModel Author { get; set; }
    public decimal Price { get; set; }
    public DateTime PublicationDate { get; set; }
    public string ISBN { get; set; }
    public string Language { get; set; }
}