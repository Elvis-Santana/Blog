using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Category
{


    public Guid Id { get; private set; } 

    public Guid IdAuthor { get; private set; }


    public string Name { get; private set; }

    public Category() { this.Id = Guid.NewGuid(); }


    public Category(Guid idAuthor, string name)
    {
        this.IdAuthor = idAuthor;
        this.Name = name;
        this.Id = Guid.NewGuid();
    }

    public Category(Guid id,Guid idAuthor , string name)
    {
        this.Id = id;
        this.IdAuthor = idAuthor;
        this.Name = name;
    }
 

}
