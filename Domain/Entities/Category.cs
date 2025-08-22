using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Category
{


    public string Id { get; private set; } 

    public string AuthorId { get; private set; }


    public string Name { get; private set; }

    public Author Author { get; private set; }

    private Category() { this.Id = Guid.NewGuid().ToString(); }


    public Category(string idAuthor, string name)
    {
        this.AuthorId = idAuthor;
        this.Name = name;
        this.Id = Guid.NewGuid().ToString();
    }

    public Category(string id, string idAuthor , string name)
    {
        this.Id = id;
        this.AuthorId = idAuthor;
        this.Name = name;
    }
 

}
