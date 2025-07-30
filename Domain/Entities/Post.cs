using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Post
{
    public Guid Id { get; private set; }

    public string Title { get; private set; }

    public string Text { get; private set; }

    public DateTime Date { get; private set; }

    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; }

    public Guid AuthorId { get; private set; }

    [JsonIgnore]
    public Author Author { get; private set; }


    public Post()
    {
        this.Id = Guid.NewGuid();   
    }

    public Post(string title, string text, DateTime date,  Guid categoryId,  Guid authorId)
    {
        this.Id = Guid.NewGuid();
        this.Title = title;
        this.Text = text;
        this.Date = date;
        this.CategoryId = categoryId;
        this.AuthorId = authorId;
    }

    public Post(string title, string text, DateTime date, Category category, Author author)
    {
        this.Id = Guid.NewGuid();
        this.Title = title;
        this.Text = text;
        this.Date = date;
        this.CategoryId = category.Id;
        this.AuthorId = author.Id;
        this.Category = category;
        this.Author = author;

    }

}
