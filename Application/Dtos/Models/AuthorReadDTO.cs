using Application.Dtos.FollowDTO;
using Application.Dtos.Models;
using Domain.Entities;
using Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Models;

public record AuthorReadDTO (string Id, FullName Name, List<PostReadDTO> Post,string Email,  List<FollowReadDTO> Follows);



