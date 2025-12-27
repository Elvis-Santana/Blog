using Application.Dtos.Models;
using Application.IRepository.IAuthorRepository;
using Application.IServices;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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


        var author = await this._authorRepository.GetByExpression(f => f.Verify(login.password) && f.Email.Equals(login.email));


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

   
