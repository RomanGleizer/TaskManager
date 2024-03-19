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

// ��������� ����� �����������
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
var projectConnection = builder.Configuration.GetConnectionString("ProjectConnection");

// ������������ AutoMapper
var mappingConfig = new MapperConfiguration(config => config.AddProfile(new MappingProfile()));
var mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

// ���������� ��������
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ����������� � ������� ��� ��������� Task � Comment
builder.Services.AddTransient<IUnitOfWork, EFUnitOfWork>();
builder.Services.AddTransient<IRepository<TaskDal, int>, TaskRepository>();
builder.Services.AddTransient<IRepository<CommentDal, int>, CommentRepository>();
builder.Services.AddTransient<IDtoService<TaskDTO, int>, TaskService>();
builder.Services.AddTransient<IDtoService<CommentDTO, int>, CommentService>();

// ������ �������� ���������� ����������� ����� RabbitMQ
builder.Services.AddHostedService<RabbitMQBackgroundProjectConnectionService>();

// ������� ���������� �����������
builder.Services.AddTransient<IProjectConnectionService, ProjectConnectionService>();
builder.Services.AddHttpRequestService();

// ��������� ���� ������
builder.Services.AddDbContext<TaskManagerDbContext>(options => options.UseSqlServer(connection));
builder.Services.AddDbContext<ProjectServiceDbContext>(options => options.UseSqlServer(projectConnection));

// ����������� � ������ ��� ������ � ���������
builder.Services.AddTransient<IProjectRepository, ProjectRepository>();
builder.Services.AddTransient<IAddTaskIdToProjectIdList, AddTaskIdToProjectIdList>();

var app = builder.Build();

// Middleware ��� ����������
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();