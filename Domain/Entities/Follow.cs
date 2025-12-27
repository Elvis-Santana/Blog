using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Follow
{
    public string FollowerId { get; private set; }
    public string FollowingId { get; private set; }

    public Author Follower { get; private set; }
    public Author Following { get; private set; }

    protected Follow() { }

    protected Follow(string FollowerId,string FollowingId)
    {
        this.FollowerId = FollowerId;
        this.FollowingId = FollowingId;
    }


    public static Follow CreateFollow(string FollowerId, string FollowingId) => new (FollowerId, FollowingId);
    

    
}
