using Application.Dtos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Models;

public record AddPostInputModel
{
   

    public string title {  get; set; }

   public string text { get; set; }

   public DateTime date { get; set; }

    public string? categoryId { get; set; } = string.Empty;

   public string authorId { get; set; }

    public AddPostInputModel()
    {
        
    }

    public AddPostInputModel(string title, string text, DateTime date, string? categoryId, string authorId)
    {
        this.title = title;
        this.text = text;
        this.date = date;
        this.categoryId = categoryId ;
        this.authorId = authorId;
    }



}


