using Application;
using Application.Dtos.Models;
using Application.Validators.Validator;
using Application.Validators.Validator.AuthorValidator;
using Application.Validators.Validator.CategoryValidator;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Db;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net.Sockets;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();



builder.Services.AddServices(builder.Configuration);




builder.Services.AddAuthentication()
    .AddJwtBearer("some-scheme", jwtOptions =>
    {

        jwtOptions.MetadataAddress = builder.Configuration["Api:MetadataAddress"];
        jwtOptions.Authority = builder.Configuration["Api:Authority"];
        jwtOptions.Audience = builder.Configuration["Api:Audience"];


        jwtOptions.RequireHttpsMetadata = false;

        jwtOptions.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            ValidateLifetime=true,

            ValidIssuers= builder.Configuration.GetSection("Api:ValidIssuers").Get<string[]>(),
            ValidAudiences = builder.Configuration.GetSection("Api:ValidAudiences").Get<string[]>(),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Api:SecretKey"]))
        };

        jwtOptions.MapInboundClaims = false;


    });



builder.Services.AddAuthorizationBuilder()
    .SetDefaultPolicy(new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build());

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
   
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha Minimal API v1");
    });


}

using (var scope = app.Services.CreateScope())
{
    db.Database.Migrate(); 
}



app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapEndpoints();


app.Run();


public partial class Program { }
