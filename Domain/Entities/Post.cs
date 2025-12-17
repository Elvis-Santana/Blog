using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Post
{
    public string Id { get; init; }

    public string Title { get; private set; }

    public string Text { get; private set; }

    public DateTime Date { get; init; }

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

   
    private Post (string title, string text, DateTime date, Category category, Author author)
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





    public void UpdateAttributes(string? title="", string? text = "", string? CategoryId = "")
    {
        if (!string.IsNullOrWhiteSpace(title))
            this.Title = title;

        if (!string.IsNullOrWhiteSpace(text))
            this.Text = text;

        if (!string.IsNullOrWhiteSpace(CategoryId))
           this.CategoryId = CategoryId;
        

    
    }
     
    public void UpdateCategoryFromTest(Category category)
    {
        this.Category = category;
        this.CategoryId = category.Id;


    }




    public static class Factory
    {
        public static Post CreatePost(string title, string text, DateTime date, string? categoryId, string authorId)
         => new (title, text, date, categoryId, authorId);

            
        
        public static Post CreatePost(string title, string text, DateTime date, Category category, Author author)
         => new(title, text, date, category, author);
    }

}
