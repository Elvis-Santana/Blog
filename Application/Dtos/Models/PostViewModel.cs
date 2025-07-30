using Application.Dtos.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Models;

public record PostViewModel(Guid Id,string Title,string Text,DateTime Date,Guid CategoryId,CategoryViewModel Category,Guid AuthorId , AuthorViewModel Author);


