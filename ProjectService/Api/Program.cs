using AutoMapper;
using ConnectionLib.ConnectionServices;
using ConnectionLib.ConnectionServices.Interfaces;
using Core.HttpLogic;
using Domain.Interfaces;
using Infastracted.Data;
using Infastracted.EF;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services.Mappers;
using Services.Services;

var builder = WebApplication.CreateBuilder(args);
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
var mapperProfile = new MapperConfiguration(config => config.AddProfile(new MapperProfile()));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

builder.Services.AddTransient<IProjectRepository, ProjectRepository>();
builder.Services.AddTransient<IProjectService, ProjectService>();

builder.Services.AddTransient<ITaskConnectionService, TaskConnectionService>();
builder.Services.AddTransient<IUserConnectionService, UserConnectionService>();

builder.Services.AddSingleton(mapperProfile.CreateMapper());

builder.Services.AddHttpRequestService();

builder.Services.AddDbContext<ProjectServiceDbContext>(options => options.UseSqlServer(connection));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
