using Application.Dtos.Models;
using Domain.Entities;
using Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Models;

public record AuthorViewModel (string Id, FullName Name, List<PostViewModel> Post);



