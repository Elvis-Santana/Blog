using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepository.IFollowRepository;

public interface IFollowRepository
{
    void CreateFollow(Follow follow);

    void RemoveFollow(Follow follow);

}
