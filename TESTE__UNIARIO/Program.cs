using Application;
using Application.Dtos.Models;
using Application.Validators.AuthorValidator;
using Application.Validators.Validator;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Db;
using Microsoft.AspNetCore.Identity;
using System.Net.Sockets;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddScoped<IValidator<AddAuthorInputModel>, AuthorValidator>();

builder.Services.AddScoped<IValidator<AddCategoryInputModel>, CategoryInputValidator>();

builder.Services.AddScoped
    <IValidator<UpdateCategoryInputModel>, CategoryUpdateValidator>();

builder.Services.AddScoped<IValidator<AddPostInputModel>, PostValidator>();
builder.Services.AddServices(builder.Configuration);



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
{
    var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<DbContextLite>();
    context.Database.EnsureCreated();
}
app.UseHttpsRedirection();

app.MapEndpoints();


app.Run();


