using AutoMapper;
using ConnectionLib.ConnectionServices.BackgroundConnectionServices;
using Dal.Ef;
using Infastracted.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using Services.Mappers;
using Api.Extentions;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
var identityDbConnection = builder.Configuration.GetConnectionString("IdentityDbConnection");

var connectionFactory = new ConnectionFactory { HostName = "localhost" };
var poolPolicy = new RabbitMQConnectionPoolPolicy(connectionFactory);
var connectionPool = new DefaultObjectPool<IConnection>(poolPolicy);

var mapperProfile = new MapperConfiguration(config => config.AddProfile(new MapperProfile()));
var mapper = mapperProfile.CreateMapper();

builder.Services.AddCustomServices();
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddIdentity();
builder.Services.AddConnectionServices();
builder.Services.AddMicroserviceInteractionOperations();

builder.Services.AddSingleton(mapper);

builder.Services.AddSwaggerGen(c => c.EnableAnnotations());

builder.Services.AddHostedService<RabbitMQBackgroundAddProjectService>();

builder.Services.AddSingleton<ObjectPool<IConnection>>(connectionPool);

builder.Services.AddDbContext<ProjectServiceDbContext>(options => options.UseSqlServer(connection));
builder.Services.AddDbContext<IdentityServiceDbContext>(options => options.UseSqlServer(identityDbConnection));

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