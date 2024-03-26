using AutoMapper;
using ConnectionLib.ConnectionServices;
using ConnectionLib.ConnectionServices.BackgroundConnectionServices;
using ConnectionLib.ConnectionServices.Interfaces;
using Core.Dal;
using Core.Dal.Base;
using Core.HttpLogic;
using Dal.Ef;
using Dal.Entities;
using Dal.Interfaces;
using Dal.Repositories;
using Domain.Entities;
using Logic.Dto.Role;
using Logic.Interfaces;
using Logic.Mapper;
using Logic.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
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

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IDtoService<RoleDto, int>, RoleService>();
builder.Services.AddTransient<IRepository<RoleDal, int>, RoleRepository>();
builder.Services.AddTransient<IUserRepository<UserDal>, UserRepository>();

builder.Services.AddTransient<IProjectConnectionService, ProjectConnectionService<Project>>();

builder.Services.AddTransient<IAddProjectIdToUserProjectIdList, AddProjectIdToUserProjectIdList<UserDal>>();

builder.Services.AddHostedService<RabbitMQBackgroundAddProjectService>();

builder.Services
    .AddDbContext<IdentityServiceDbContext>(options => options.UseSqlServer(connection))
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
builder.Services.AddHttpRequestService();

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