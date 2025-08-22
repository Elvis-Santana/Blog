using Application.Dtos.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Dtos.Models;

public  record PostViewModel(string Id,string Title,string Text,DateTime Date,string CategoryId,  CategoryViewModel Category,string AuthorId);


