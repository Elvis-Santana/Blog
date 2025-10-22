using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Models;

public record PostUpdateDTO(string? Title=""  , string? Text = "" , string? CategoryId = "" );
