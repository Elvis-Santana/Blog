using Application;
using Application.Dtos.AuthorViewModel;
using Application.Dtos.Models;
using Application.Validators.AuthorValidator;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddServices(builder.Configuration);
builder.Services.RegisterMap();

builder.Services.AddScoped<IValidator<AddAuthorInputModel>, AuthorValidator>();
builder.Services.AddScoped<IValidator<AddCategoryInputModel>, CategoryValidator>();



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

app.UseHttpsRedirection();

app.MapEndpoints();


app.Run();

