using AutoMapper;
using ConnectionLib.ConnectionServices;
using ConnectionLib.ConnectionServices.BackgroundConnectionServices;
using ConnectionLib.ConnectionServices.Interfaces;
using Core.HttpLogic;
using Dal.Ef;
using Dal.Entities;
using Dal.Interfaces;
using Dal.Repositories;
using Domain.Interfaces;
using Infastracted.Data;
using Infastracted.EF;
using Logic.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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

builder.Services.AddHostedService<RabbitMQBackgroundUserConnectionService>();

builder.Services.AddHttpRequestService();

builder.Services.AddDbContext<ProjectServiceDbContext>(options => options.UseSqlServer(connection));

builder.Services
    .AddDbContext<IdentityServiceDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityDbConnection")))
    .AddIdentity<UserDal, RoleDal>(options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireDigit = true;
        options.User.RequireUniqueEmail = false;
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<IdentityServiceDbContext>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserService>();

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
