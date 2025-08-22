using Application.Dtos.Models;
using Domain.Entities;
using Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Models;

public record AuthorViewModel
{
   
   public  string Id {  get; set; }
   public FullName Name { get; set; }
   public List<PostViewModel> Post {  get; set; } = new List<PostViewModel>();

   public AuthorViewModel(string id, FullName name, List<PostViewModel> post)
   {
        Id = id;
        Name = name;
        Post = post?? new List<PostViewModel>(); 
   }

}




