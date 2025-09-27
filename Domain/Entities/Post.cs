using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Post
{
    public string Id { get; init; }

    public string Title { get; init; }

    public string Text { get; init; }

    public DateTime Date { get; private set; }

    public string? CategoryId { get; private set; } 

    public Category? Category { get; private set; }

    public string AuthorId { get; init; }
    public Author Author { get; init; }


    private Post(){}

    private Post(string title, string text, DateTime date,  string? categoryId,  string authorId)
    {
        this.Id = Guid.NewGuid().ToString();
        this.Title = title;
        this.Text = text;
        this.Date = date;
        this.AuthorId = authorId;

        this.CategoryId = categoryId!.Equals(string.Empty)? null: categoryId;

       
        
    }

   
    private Post(string title, string text, DateTime date, Category category, Author author)
    {
        this.Id = Guid.NewGuid().ToString();
        this.Title = title;
        this.Text = text;
        this.Date = date;
        this.CategoryId = category.Id;
        this.AuthorId = author.Id;
        this.Category = category;
        this.Author = author;

       
    }

    public static class Factory
    {
        public static Post CreatePost(string title, string text, DateTime date, string? categoryId, string authorId)
         => new (title, text, date, categoryId, authorId);
        
        public static Post CreatePost(string title, string text, DateTime date, Category category, Author author)
         => new(title, text, date, category, author);

        


    }

}
