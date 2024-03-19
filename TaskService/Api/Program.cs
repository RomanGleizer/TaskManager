using AutoMapper;
using ConnectionLib.ConnectionServices;
using ConnectionLib.ConnectionServices.BackgroundConnectionService;
using ConnectionLib.ConnectionServices.Interfaces;
using Core.Dal;
using Core.Dal.Base;
using Core.HttpLogic;
using Dal.EF;
using Dal.Entities;
using Dal.Interfaces;
using Dal.Repositories;
using Domain.Interfaces;
using Infastracted.Data;
using Infastracted.EF;
using Logic.DTO;
using Logic.MapperLogic;
using Logic.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Получение строк подключения
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
var projectConnection = builder.Configuration.GetConnectionString("ProjectConnection");

// Конфигурация AutoMapper
var mappingConfig = new MapperConfiguration(config => config.AddProfile(new MappingProfile()));
var mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

// Добавление сервисов
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Репозитории и сервисы для сущностей Task и Comment
builder.Services.AddTransient<IUnitOfWork, EFUnitOfWork>();
builder.Services.AddTransient<IRepository<TaskDal, int>, TaskRepository>();
builder.Services.AddTransient<IRepository<CommentDal, int>, CommentRepository>();
builder.Services.AddTransient<IDtoService<TaskDTO, int>, TaskService>();
builder.Services.AddTransient<IDtoService<CommentDTO, int>, CommentService>();

// Сервис фонового проектного подключения через RabbitMQ
builder.Services.AddHostedService<RabbitMQBackgroundProjectConnectionService>();

// Сервисы проектного подключения
builder.Services.AddTransient<IProjectConnectionService, ProjectConnectionService>();
builder.Services.AddHttpRequestService();

// Контексты базы данных
builder.Services.AddDbContext<TaskManagerDbContext>(options => options.UseSqlServer(connection));
builder.Services.AddDbContext<ProjectServiceDbContext>(options => options.UseSqlServer(projectConnection));

// Репозиторий и сервис для работы с проектами
builder.Services.AddTransient<IProjectRepository, ProjectRepository>();
builder.Services.AddTransient<IAddTaskIdToProjectIdList, AddTaskIdToProjectIdList>();

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