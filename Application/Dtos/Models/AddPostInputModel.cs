using Application.Dtos.AuthorViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Models;

public record AddPostInputModel(
    string title,string text,DateTime Date,Guid categoryId, Guid authorId);


