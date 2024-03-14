using AutoMapper;
using ConnectionLib.ConnectionServices;
using ConnectionLib.ConnectionServices.Interfaces;
using Core.Dal.Base;
using Core.HttpLogic;
using Dal.EF;
using Dal.Entities;
using Dal.Interfaces;
using Dal.Repositories;
using Logic.DTO;
using Logic.MapperLogic;
using Logic.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
var mappingConfig = new MapperConfiguration(config => config.AddProfile(new MappingProfile()));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IUnitOfWork, EFUnitOfWork>();
builder.Services.AddTransient<IRepository<TaskDal, int>, TaskRepository>();
builder.Services.AddTransient<IRepository<CommentDal, int>, CommentRepository>();
builder.Services.AddTransient<IDtoService<TaskDTO, int>, TaskService>();
builder.Services.AddTransient<IDtoService<CommentDTO, int>, CommentService>();

builder.Services.AddTransient<IProjectConnectionService, ProjectConnectionService>();
builder.Services.AddHttpRequestService();

builder.Services.AddSingleton(mappingConfig.CreateMapper());
builder.Services
    .AddDbContext<TaskManagerDbContext>(options => options.UseSqlServer(connection));

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
