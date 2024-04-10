using Api.Extentions;
using AutoMapper;
using ConnectionLib.ConnectionServices.BackgroundConnectionService;
using ConnectionLib.ConnectionServices.BackgroundConnectionServices;
using Dal.EF;
using Domain.Entities;
using Infastracted.EF;
using Logic.MapperLogic;
using Logic.TaskSaga;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using MassTransit.EntityFrameworkCoreIntegration.Saga;
using MassTransit.Saga;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
var projectConnection = builder.Configuration.GetConnectionString("ProjectConnection");
var sagaConnection = builder.Configuration.GetConnectionString("SagaConnection");

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

builder.Services.AddDbContext<TaskManagerDbContext>(options => options.UseSqlServer(connection));
builder.Services.AddDbContext<ProjectServiceDbContext>(options => options.UseSqlServer(projectConnection));

builder.Services.AddSingleton<ObjectPool<IConnection>>(connectionPool);

builder.Services.AddDbContext<SagasDbContext>(options => options.UseSqlServer(sagaConnection));
builder.Services.AddMassTransit(cfg =>
{
    cfg.SetKebabCaseEndpointNameFormatter();
    cfg.SetInMemorySagaRepositoryProvider();
    cfg.AddDelayedMessageScheduler();
    cfg.AddConsumer<CreateTaskSagaConsumer>();
    cfg.AddSagaStateMachine<CreateTaskSagaStateMachine, CreateTaskSagaState>()
        .EntityFrameworkRepository(r =>
        {
            r.ConcurrencyMode = ConcurrencyMode.Pessimistic;
            r.ExistingDbContext<SagasDbContext>();
            r.LockStatementProvider = new SqlServerLockStatementProvider();
        });

    cfg.UsingRabbitMq((brc, rbfc) =>
    {
        rbfc.UseInMemoryOutbox(brc);
        rbfc.UseMessageRetry(r =>
        {
            r.Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        });
        rbfc.UseDelayedMessageScheduler();
        rbfc.Host("localhost", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        rbfc.ConfigureEndpoints(brc);
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