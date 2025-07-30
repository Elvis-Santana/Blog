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
  
    public Guid Id { get; private set; }

    public FullName Name { get; private set; } 

    public List<Post> Post { get; private set; }

    public Author()
    {
        Id = Guid.NewGuid();
    }

    public Author(Guid id, FullName name, List<Post> post)
    {
        Id = id;
        Name = name;
        Post = post;
    }

    public Author(Guid id, FullName name)
    {
        Id = id;
        Name = name;
        Post = new List<Post>();
    }
 
    public Author( FullName name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

}
