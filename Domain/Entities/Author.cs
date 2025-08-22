using Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Entities;

public class Author
{
  
    public string Id { get; init; }

    public FullName Name { get; init; } 

    public List<Post> Post { get; init; }

    private Author()
    {
        Id = Guid.NewGuid().ToString();
    }

    public Author(string id, FullName name, List<Post> post)
    {
        Id = id;
        Name = name;
        Post = post;
    }

    public Author(string id, FullName name)
    {
        Id = id;
        Name = name;
        Post = new List<Post>();
    }
 
    public Author( FullName name)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
    }

}
