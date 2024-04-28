using AutoMapper;
using ConnectionLibrary.ConnectionServices.BackgroundConnectionServices;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;
using ProjectsMicroservice.ProjectsMicroserviceDomain.Entities;
using ProjectsMicroservice.ProjectsMicroserviceInfrastructure.EntityFramework;
using RabbitMQ.Client;
using SemaphoreSynchronizationPrimitiveLibrary.Interfaces;
using SemaphoreSynchronizationPrimitiveLibrary.Semaphores;
using StackExchange.Redis;
using TasksMicroservice.TasksMicroserviceApi.Extensions;
using TasksMicroservice.TasksMicroserviceDal.EntityFramework;
using TasksMicroservice.TasksMicroserviceLogic.MapperLogic;
using TasksMicroservice.TasksMicroserviceLogic.TaskSaga;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString("TaskMicroserviceConnection");
var projectConnection = builder.Configuration.GetConnectionString("ProjectMicroserviceConnection");
var sagaConnection = builder.Configuration.GetConnectionString("SagasDatabase");

var mappingConfig = new MapperConfiguration(mapperConfigurationExpression =>
    mapperConfigurationExpression.AddProfile(new MappingProfile()));

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

builder.Services.AddHostedService<RabbitMqBackgroundGetProjectService<Project>>();
builder.Services.AddHostedService<RabbitMqBackgroundAddTaskService>();

builder.Services.AddDbContext<TasksMicroserviceDbContext>(options => options.UseSqlServer(connection));
builder.Services.AddDbContext<ProjecstMicroserviceDbContext>(options => options.UseSqlServer(projectConnection));

builder.Services.AddSingleton<ObjectPool<IConnection>>(connectionPool);

builder.Services.AddSingleton<IDistributedSemaphore>(_ =>
{
    var connectionMultiplexer = ConnectionMultiplexer.Connect("localhost");
    return new RedisDistributedSemaphore(connectionMultiplexer, 10);
});

builder.Services.AddDbContext<SagasDbContext>(options => options.UseSqlServer(sagaConnection));

builder.Services.AddMassTransit(cfg =>
{
    cfg.SetKebabCaseEndpointNameFormatter();
    cfg.SetInMemorySagaRepositoryProvider();
    cfg.AddDelayedMessageScheduler();
    cfg.AddConsumer<CreateTaskSagaConsumer>();
    cfg.AddSagaStateMachine<CreateTaskSagaStateMachine, CreateTaskSagaState>()
        .EntityFrameworkRepository(entityFrameworkSagaRepositoryConfigurator =>
        {
            entityFrameworkSagaRepositoryConfigurator.ConcurrencyMode = ConcurrencyMode.Pessimistic;
            entityFrameworkSagaRepositoryConfigurator.ExistingDbContext<SagasDbContext>();
            entityFrameworkSagaRepositoryConfigurator.LockStatementProvider = new SqlServerLockStatementProvider();
        });

    cfg.UsingRabbitMq((busRegistrationContext, rabbitMqBusFactoryConfigurator) =>
    {
        rabbitMqBusFactoryConfigurator.UseInMemoryOutbox(busRegistrationContext);
        rabbitMqBusFactoryConfigurator.UseMessageRetry(retryConfigurator =>
        {
            retryConfigurator.Incremental(
                3,
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(1));
        });
        rabbitMqBusFactoryConfigurator.UseDelayedMessageScheduler();
        rabbitMqBusFactoryConfigurator.Host("localhost", rabbitMqHostConfigurator =>
        {
            rabbitMqHostConfigurator.Username("guest");
            rabbitMqHostConfigurator.Password("guest");
        });
        rabbitMqBusFactoryConfigurator.ConfigureEndpoints(busRegistrationContext);
    });
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