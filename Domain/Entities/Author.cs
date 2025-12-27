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

    
    public string PasswordHash {  get; private set; }

    public string Email { get; private set; }


    private readonly List<Follow> _followers = new();
    public IReadOnlyCollection<Follow> Followers => _followers;

    // Quem eu sigo
    private readonly List<Follow> _following = new();
    public IReadOnlyCollection<Follow> Following => _following;


    private Author(){ }

    public Author(string id, FullName name, List<Post> post, string passwordHash,string email)
    {
        Id = id;
        Name = name;
        Post = post;
        PasswordHash = passwordHash;
        Email= email;
    }

    public Author(string id, FullName name, string passwordHash, string email)
    {
        Id = id;
        Name = name;
        Post = new List<Post>();
        PasswordHash = passwordHash;
        Email= email;
    }
 
    private Author( FullName name, string passwordHash, string email)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        PasswordHash= passwordHash;
        Email= email;
    }
  


    
     public static Author CriarAuthor(
            FullName name,
            string passwordHash,
            string email
      ) =>  new Author(name, passwordHash, email); 


    


    public  void UpdateName(FullName Name)
    {
            this.Name = Name;
    }

    

   
}
