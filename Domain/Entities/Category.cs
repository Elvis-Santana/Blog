using Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Category
{

    public string Id { get; init; } 
    public string AuthorId { get; init; }
    public string Name { get; set; }
    public Author Author { get; init; }
    private Category() {  }


    private Category(string idAuthor, string name)
    {
        this.AuthorId = idAuthor;
        this.Name = name;
        this.Id = Guid.NewGuid().ToString();
    }

    private Category(string id, string idAuthor, string name)
    {
        this.Id = id;
        this.AuthorId = idAuthor;
        this.Name = name;
    }

    public static class Factory
    {
        public static Category CreateCategory(string idAuthor, string name) => new (idAuthor, name);
        public static Category CreateCategory(string id, string idAuthor, string name) => new(id,idAuthor, name);


    }

    public void UpdateName(string Name)
    {
        if(!string.IsNullOrEmpty(Name))
            this.Name = Name;
    }


}
