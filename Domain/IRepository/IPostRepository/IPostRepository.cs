using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepository.IPostRepository;

public interface IPostRepository
{
    Task CreatePost(Post post);

    Task<IEnumerable<Post>> GetAllPosts();

     void RemovePost(Post post);

    Task<Post?> GetPostsById(string id);

    //void Update(Post post);


    //Task<bool> Save();

}
