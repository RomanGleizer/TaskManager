using AutoMapper;
using Domain.Interfaces;
using Infastracted.Data;
using Infastracted.EF;
using Services.Interfaces;
using Services.Mappers;
using Services.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
var projectMappingConfig = new MapperConfiguration(config => config.AddProfile(new ProjectMapperProfile()));
var memberMappingConfig = new MapperConfiguration(config => config.AddProfile(new MemberMapperProfile()));
var roleMappingConfig = new MapperConfiguration(config => config.AddProfile(new RoleMapperProfile()));

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

builder.Services.AddSingleton(projectMappingConfig.CreateMapper());
builder.Services.AddSingleton(memberMappingConfig.CreateMapper());
builder.Services.AddSingleton(roleMappingConfig.CreateMapper());

builder.Services
    .AddDbContext<ProjectServiceDbContext>(options => options.UseSqlServer(connection));

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
