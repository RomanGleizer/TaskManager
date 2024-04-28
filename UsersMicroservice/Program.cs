using AutoMapper;
using ConnectionLibrary.ConnectionServices.BackgroundConnectionServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using SemaphoreSynchronizationPrimitiveLibrary.Interfaces;
using SemaphoreSynchronizationPrimitiveLibrary.Semaphores;
using StackExchange.Redis;
using UsersMicroservice.UsersMicroserviceApi.Extensions;
using UsersMicroservice.UsersMicroserviceDal.EntityFramework;
using UsersMicroservice.UsersMicroserviceLogic.Mappers;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString("UsersMicroserviceConnection");
var redisDistributedSemaphoreConfigurationTimeout = builder.Configuration.GetValue<string>("RedisDistributedSemaphoreTimeoutInSeconds");

var mappingConfig = new MapperConfiguration(mapperConfigurationExpression
    => mapperConfigurationExpression.AddProfile(new MappingProfile()));

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
builder.Services.AddDbContext<UsersMicroserviceDbContext>(options => options.UseSqlServer(connection));
builder.Services.AddSingleton<ObjectPool<IConnection>>(connectionPool);

builder.Services.AddSingleton<IDistributedSemaphore>(provider =>
{
    if (!int.TryParse(redisDistributedSemaphoreConfigurationTimeout, out var redisDistributedSemaphoreTimeout))
        throw new Exception("Параметр TimeOut в конфигурации не целое число");

    var connectionMultiplexer = ConnectionMultiplexer.Connect("localhost");
    return new RedisDistributedSemaphore(connectionMultiplexer, redisDistributedSemaphoreTimeout);
});

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