using ConnectionLibrary.ConnectionServices;
using ConnectionLibrary.ConnectionServices.Interfaces;
using Core.Dal;
using Core.Dal.Base;
using Core.HttpLogic;
using ProjectsMicroservice.ProjectsMicroserviceDomain.Entities;
using ProjectsMicroservice.ProjectsMicroserviceInfrastructure.Data;
using TasksMicroservice.TasksMicroserviceDal.Entities;
using TasksMicroservice.TasksMicroserviceDal.Interfaces;
using TasksMicroservice.TasksMicroserviceDal.Repositories;
using TasksMicroservice.TasksMicroserviceLogic.Dto;
using TasksMicroservice.TasksMicroserviceLogic.Services;

namespace TasksMicroservice.TasksMicroserviceApi.Extensions;

public static class TaskServicesExtensions
{
    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddHttpRequestService();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IRepository<TaskDal, Guid>, TaskRepository>();
        services.AddTransient<IUnitOfWork, EntityFrameworkUnitOfWork>();
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IDtoService<TaskDto, Guid>, TaskService>();
    }

    public static void AddConnectionServices(this IServiceCollection services)
    {
        services.AddTransient<IProjectConnectionService, ProjectConnectionService<Project>>();
    }

    public static void AddMicroserviceInteractionOperations(this IServiceCollection services)
    {
        services.AddTransient<IProjectRepository<Project, Guid>, ProjectRepository>();
        services.AddTransient<IAddTaskIdToProjectTaskIdList, AddTaskIdToProjectTaskIdList<Project>>();
        services.AddTransient<IGetProjectById<Project>, GetProjectById<Project>>();
    }
}