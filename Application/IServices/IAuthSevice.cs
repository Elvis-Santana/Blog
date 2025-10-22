using Application.Dtos.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices;

public interface IAuthSevice
{
    Task<Token> CriateToken(Login login);
    Task<bool> Validation(Token token);

}
