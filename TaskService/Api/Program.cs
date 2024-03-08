using AutoMapper;
using Dal.EF;
using Dal.Entities;
using Dal.Interfaces;
using Dal.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Logic.MapperLogic;
using Logic.Interfaces;
using Logic.Services;
using Logic.DTO;
using ConnectionLib.ConnectionServices.Interfaces;
using ConnectionLib.ConnectionServices;
using Core.HttpLogic.Services.Interfaces;
using Core.HttpLogic;

var builder = WebApplication.CreateBuilder(args);
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
var mappingConfig = new MapperConfiguration(config => config.AddProfile(new MappingProfile()));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IUnitOfWork, EFUnitOfWork>();
builder.Services.AddTransient<IRepository<TaskDal, int>, TaskRepository>();
builder.Services.AddTransient<IRepository<CommentDal, int>, CommentRepository>();
builder.Services.AddTransient<IRepository<RoleDal, int>, RoleRepository>();
builder.Services.AddTransient<IRepository<UserDal, string>, UserRepository>();
builder.Services.AddTransient<IDtoService<TaskDTO, int>, TaskService>();
builder.Services.AddTransient<IDtoService<CommentDTO, int>, CommentService>();
builder.Services.AddTransient<IDtoService<RoleDTO, int>, RoleService>();
builder.Services.AddTransient<IDtoService<UserDTO, string>, UserService>();

builder.Services.AddTransient<IProjectConnectionService, ProjectConnectionService>();
builder.Services.AddHttpRequestService();

builder.Services.AddSingleton(mappingConfig.CreateMapper());
builder.Services
    .AddDbContext<TaskManagerDbContext>(options => options.UseSqlServer(connection))
    .AddIdentity<UserDal, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireDigit = true;
        options.User.RequireUniqueEmail = false;
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<TaskManagerDbContext>();

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
