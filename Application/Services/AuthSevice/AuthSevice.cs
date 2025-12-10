using Application.Dtos.Models;
using Application.Extensions;
using Application.IServices;
using Domain.Entities;
using Domain.IRepository.IAuthorRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.AuthSevice;

public class AuthSevice  : IAuthSevice
{
    private readonly IConfiguration _configuration;
    private readonly IAuthorRepository _authorRepository;

    public AuthSevice(IConfiguration configuration, IAuthorRepository authorRepository)
    {
        _configuration = configuration;
        _authorRepository = authorRepository;
    }

    public async Task<Token> CriateToken(Login login)
    {


        var author = await this._authorRepository.GetByExpression(f =>f.Verify(login.password));


        if (author is not null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();


            var key = Encoding.ASCII.GetBytes(this._configuration["Api:SecretKey"]);


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    [
                        new Claim(ClaimTypesExtensions.Id,author.Id),
                    ]
                ),
                Expires = DateTime.UtcNow.AddHours(8),
                
                SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),

                  
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
               

            return Token.Factory.CreateToken(tokenHandler.WriteToken(token));
        }
        
       


        return Token.Factory.CreateToken(string.Empty);



    }

    
    public async Task<bool> Validation(Token token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(this._configuration["Api:SecretKey"]);


       var result = (await tokenHandler.ValidateTokenAsync(
                token.token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,   
                    ValidateAudience = false, 
                    ValidateLifetime = true,  
                    ClockSkew = TimeSpan.FromHours(8),
                }
            )
        ).IsValid;


        Console.WriteLine(result);

        return result;
    }
}

   
