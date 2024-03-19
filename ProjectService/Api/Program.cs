using AutoMapper;
using ConnectionLib.ConnectionServices;
using ConnectionLib.ConnectionServices.BackgroundConnectionServices;
using ConnectionLib.ConnectionServices.Interfaces;
using Core.Dal;
using Core.Dal.Base;
using Core.HttpLogic;
using Dal.Ef;
using Dal.Entities;
using Dal.Repositories;
using Domain.Entities;
using Infastracted.Data;
using Infastracted.EF;
using Logic.Interfaces;
using Logic.Services;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services.Mappers;
using Services.Services;

var builder = WebApplication.CreateBuilder(args);

// Конфигурация подключений
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
var identityDbConnection = builder.Configuration.GetConnectionString("IdentityDbConnection");

// Конфигурация AutoMapper
var mapperProfile = new MapperConfiguration(config => config.AddProfile(new MapperProfile()));
var mapper = mapperProfile.CreateMapper();
builder.Services.AddSingleton(mapper);

// Добавление сервисов
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.EnableAnnotations());
builder.Services.AddHostedService<RabbitMQBackgroundAddProjectService>();
builder.Services.AddHttpRequestService();

// Добавление контекстов базы данных
builder.Services.AddDbContext<ProjectServiceDbContext>(options => options.UseSqlServer(connection));
builder.Services.AddDbContext<IdentityServiceDbContext>(options => options.UseSqlServer(identityDbConnection));

// Конфигурация Identity
builder.Services
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

// Добавление репозиториев и сервисов
builder.Services.AddTransient<IProjectRepository<Project, int>, ProjectRepository>();
builder.Services.AddTransient<IProjectService, ProjectService>();
builder.Services.AddTransient<IUserRepository<UserDal>, UserRepository>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAddProjectIdToUserProjectIdList, AddProjectIdToUserProjectIdList<UserDal>>();

// Добавление соединений
builder.Services.AddTransient<ITaskConnectionService, TaskConnectionService>();
builder.Services.AddTransient<IUserConnectionService, UserConnectionService<UserService>>();

// Добавление сервиса для добавления задачи в проект
builder.Services.AddTransient<IAddTaskIdToProjectTaskIdList, AddTaskIdToProjectTaskIdList<Project>>();
builder.Services.AddTransient<IGetProjectById<Project>, GetProjectById<Project>>();

var app = builder.Build();

// Middleware для разработки
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();