using AutoMapper;
using ConnectionLibrary.ConnectionServices.BackgroundConnectionServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;
using ProjectsMicroservice.ProjectsMicroserviceApi.Extensions;
using ProjectsMicroservice.ProjectsMicroserviceApplication.Mappers;
using ProjectsMicroservice.ProjectsMicroserviceInfrastructure.EntityFramework;
using RabbitMQ.Client;
using UsersMicroservice.UsersMicroserviceDal.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString("ProjectMicroserviceConnection");
var identityDbConnection = builder.Configuration.GetConnectionString("UsersMicroserviceConnection");

var connectionFactory = new ConnectionFactory { HostName = "localhost" };
var poolPolicy = new RabbitMqConnectionPoolPolicy(connectionFactory);
var connectionPool = new DefaultObjectPool<IConnection>(poolPolicy);

var mapperProfile = new MapperConfiguration(mapperConfigurationExpression
    => mapperConfigurationExpression.AddProfile(new MapperProfile()));

var mapper = mapperProfile.CreateMapper();

builder.Services.AddCustomServices();
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddIdentity();
builder.Services.AddConnectionServices();
builder.Services.AddMicroserviceInteractionOperations();

builder.Services.AddSingleton(mapper);

builder.Services.AddSwaggerGen(c => c.EnableAnnotations());

builder.Services.AddHostedService<RabbitMqBackgroundAddProjectService>();

builder.Services.AddSingleton<ObjectPool<IConnection>>(connectionPool);

builder.Services.AddDbContext<ProjecstMicroserviceDbContext>(
    options => options.UseSqlServer(connection));

builder.Services.AddDbContext<UsersMicroserviceDbContext>(
    options => options.UseSqlServer(identityDbConnection));

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