using Application.Dtos.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Dtos.Models;

public  record PostReadDTO(string Id, string Title, string Text, DateTime Date, string? CategoryId, CategoryReadDTO? Category, string AuthorId);
