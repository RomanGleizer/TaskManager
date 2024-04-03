using Api.Extentions;
using AutoMapper;
using ConnectionLib.ConnectionServices.BackgroundConnectionServices;
using Dal.Ef;
using Logic.Mapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString("DefaultConnection");

var mappingConfig = new MapperConfiguration(config => config.AddProfile(new MappingProfile()));
var mapper = mappingConfig.CreateMapper();

var connectionFactory = new ConnectionFactory { HostName = "localhost" };
var poolPolicy = new RabbitMqConnectionPoolPolicy(connectionFactory);
var connectionPool = new DefaultObjectPool<IConnection>(poolPolicy);

builder.Services.AddSingleton(mapper);

builder.Services.AddCustomServices();
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddConnectionServices();
builder.Services.AddMicroserviceInteractionOperations();
builder.Services.AddIdentity();

builder.Services.AddHostedService<RabbitMqBackgroundAddProjectService>();

builder.Services.AddDbContext<IdentityServiceDbContext>(options => options.UseSqlServer(connection));

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