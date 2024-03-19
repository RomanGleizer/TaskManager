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

var builder = WebApplication.CreateBuilder(args);

// Получение строки подключения к базе данных
var connection = builder.Configuration.GetConnectionString("DefaultConnection");

// Конфигурация AutoMapper
var mappingConfig = new MapperConfiguration(config => config.AddProfile(new MappingProfile()));
var mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

// Добавление сервисов
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Репозиторий для работы с Unit of Work
builder.Services.AddTransient<IUnitOfWork, EFUnitOfWork>();

// Сервисы для работы с пользователями и ролями
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IDtoService<RoleDto, int>, RoleService>();
builder.Services.AddTransient<IRepository<RoleDal, int>, RoleRepository>();
builder.Services.AddTransient<IUserRepository<UserDal>, UserRepository>();

// Сервис проектного подключения
builder.Services.AddTransient<IProjectConnectionService, ProjectConnectionService<Project>>();

// Сервис для добавления идентификатора проекта в список идентификаторов проекта пользователя
builder.Services.AddTransient<IAddProjectIdToUserProjectIdList, AddProjectIdToUserProjectIdList<UserDal>>();

// Фоновый сервис для подключения к пользовательским службам через RabbitMQ
builder.Services.AddHostedService<RabbitMQBackgroundAddProjectService>();

// Конфигурация Identity и добавление сервиса HTTP-запросов
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