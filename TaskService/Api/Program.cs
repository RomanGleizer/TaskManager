using Api.Extentions;
using AutoMapper;
using ConnectionLib.ConnectionServices.BackgroundConnectionService;
using ConnectionLib.ConnectionServices.BackgroundConnectionServices;
using ConnectionLib.ConnectionServices.DtoModels.AddTaskInProject;
using ConnectionLib.ConnectionServices.DtoModels.ProjectById;
using Dal.EF;
using Domain.Entities;
using Infastracted.EF;
using Logic.MapperLogic;
using Logic.SagaLogic;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
var projectConnection = builder.Configuration.GetConnectionString("ProjectConnection");

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

builder.Services.AddMassTransit(cfg =>
{
    cfg.AddRequestClient<IsProjectExistsRequest>();
    cfg.AddRequestClient<AddTaskIdInProjectTaskIdsRequest>();

    cfg.UsingRabbitMq((brc, rbfc) =>
    {
        rbfc.UseInMemoryOutbox(brc);

        rbfc.UseMessageRetry(r =>
        {
            r.Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        });

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