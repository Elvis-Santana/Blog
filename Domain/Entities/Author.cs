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

    public FullName Name { get; private set; } 

    public List<Post> Post { get; init; } = new List<Post>();

    private Author()
    {
        
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
 
    private Author( FullName name)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
    }

    

    public static class Factory
    {
        public static Author CriarAuthor(FullName name) { return new Author(name); }


    }


    public  void UpdateName(FullName Name)
   {
            this.Name = Name;
   }
    

   
}
