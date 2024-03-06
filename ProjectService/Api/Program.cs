using AutoMapper;
using Domain.Interfaces;
using Infastracted.Data;
using Infastracted.EF;
using Services.Interfaces;
using Services.Mappers;
using Services.Services;
using Microsoft.EntityFrameworkCore;
using Core.HttpLogic;
using ProjectConnectionLib.ConnectionServices.Interfaces;
using ProjectConnectionLib.ConnectionServices;
using Core.HttpLogic.Services.Interfaces;

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
builder.Services.AddTransient<IMemberRepository, MemberRepository>();
builder.Services.AddTransient<IMemberService, MemberService>();
builder.Services.AddTransient<IRoleRepository, RoleRepository>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<ITaskConnectionService>(provider =>
{
    var httpRequestService = provider.GetRequiredService<IHttpRequestService>();
    return new TaskConnectionService(httpRequestService);
});


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