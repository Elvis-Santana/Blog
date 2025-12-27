using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Follow
{
    public string FollowerId { get; init; }
    public string AuthorId { get; init; }  

    protected Follow() { }

    public Follow(string FollowerId,string AuthorId)
    {
        this.FollowerId = FollowerId;
        this.AuthorId = AuthorId;
    }
}
