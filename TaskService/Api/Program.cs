using AutoMapper;
using ConnectionLib.ConnectionServices;
using ConnectionLib.ConnectionServices.BackgroundConnectionService;
using ConnectionLib.ConnectionServices.BackgroundConnectionServices;
using ConnectionLib.ConnectionServices.Interfaces;
using Core.Dal;
using Core.Dal.Base;
using Core.HttpLogic;
using Dal.EF;
using Dal.Entities;
using Dal.Interfaces;
using Dal.Repositories;
using Domain.Entities;
using Infastracted.Data;
using Infastracted.EF;
using Logic.DTO;
using Logic.MapperLogic;
using Logic.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
var projectConnection = builder.Configuration.GetConnectionString("ProjectConnection");
var mappingConfig = new MapperConfiguration(config => config.AddProfile(new MappingProfile()));
var mapper = mappingConfig.CreateMapper();
var connectionFactory = new ConnectionFactory { HostName = "localhost" };
var poolPolicy = new RabbitMQConnectionPoolPolicy(connectionFactory);
var connectionPool = new DefaultObjectPool<IConnection>(poolPolicy);

builder.Services.AddSingleton(mapper);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IUnitOfWork, EFUnitOfWork>();
builder.Services.AddTransient<IRepository<TaskDal, int>, TaskRepository>();
builder.Services.AddTransient<IRepository<CommentDal, int>, CommentRepository>();
builder.Services.AddTransient<IDtoService<TaskDTO, int>, TaskService>();
builder.Services.AddTransient<IDtoService<CommentDTO, int>, CommentService>();

builder.Services.AddHostedService<RabbitMQBackgroundAddTaskService>();

builder.Services.AddTransient<IProjectConnectionService, ProjectConnectionService<Project>>();
builder.Services.AddHttpRequestService();

builder.Services.AddDbContext<TaskManagerDbContext>(options => options.UseSqlServer(connection));
builder.Services.AddDbContext<ProjectServiceDbContext>(options => options.UseSqlServer(projectConnection));

builder.Services.AddTransient<IProjectRepository<Project, int>, ProjectRepository>();
builder.Services.AddTransient<IAddTaskIdToProjectTaskIdList, AddTaskIdToProjectTaskIdList<Project>>();
builder.Services.AddTransient<IGetProjectById<Project>, GetProjectById<Project>>();

builder.Services.AddSingleton<ObjectPool<IConnection>>(connectionPool);

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