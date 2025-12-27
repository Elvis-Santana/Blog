using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepository.IPostRepository;

public interface IPostRepository
{
    Task CreatePost(Post post);
    void RemovePost(Post post);

    Task<IEnumerable<Post>> GetAllPosts();

    Task<Post?> GetPostsById(string id);

    Task LoadCategoryReferenceAsync(Post post);




}
