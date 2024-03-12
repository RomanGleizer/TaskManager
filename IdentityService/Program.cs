using AutoMapper;
using Core.Dal.Base;
using Dal.Ef;
using Dal.Entities;
using Dal.Interfaces;
using Dal.Repositories;
using Logic.Dto.Role;
using Logic.Interfaces;
using Logic.Mapper;
using Logic.Services;
using Microsoft.EntityFrameworkCore;
using Core.HttpLogic;
using ConnectionLib.ConnectionServices.Interfaces;
using ConnectionLib.ConnectionServices;

var builder = WebApplication.CreateBuilder(args);
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
var mappingConfig = new MapperConfiguration(config => config.AddProfile(new MappingProfile()));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IUnitOfWork, EFUnitOfWork>();

builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddTransient<IDtoService<RoleDto, int>, RoleService>();
builder.Services.AddTransient<IRepository<RoleDal, int>, RoleRepository>();

builder.Services.AddTransient<IProjectConnectionService, ProjectConnectionService>();

builder.Services.AddSingleton(mappingConfig.CreateMapper());

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
