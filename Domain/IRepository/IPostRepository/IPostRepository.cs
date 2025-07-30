using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepository.IPostRepository;

public interface IPostRepository
{
    Task<bool> Create(Post post);

    Task<List<Post>> GetAllPosts();
}
