using Domain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTest.EntitiesTest;

public class FollowTest
{


    [Fact]
    public void CreateFollow_WithValidParameters_SetsFollowerAndFollowingIds ()
    {

        string FollowerId = Guid.NewGuid().ToString();
        string FollowingId = Guid.NewGuid().ToString(); 

        Follow follow = Follow.CreateFollow(FollowerId, FollowingId);


        follow.FollowerId.Should().Be(FollowerId);
        follow.FollowingId.Should().Be(FollowingId);

    }
}
