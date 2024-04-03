using ConnectionLib.ConnectionServices.Interfaces;
using ConnectionLib.ConnectionServices;
using Core.Dal.Base;
using Core.Dal;
using Dal.Interfaces;
using Dal.Repositories;
using Domain.Entities;
using Logic.DTO;
using Logic.Services;
using Dal.Entities;
using Core.HttpLogic;
using Infastracted.Data;

namespace Api.Extentions;

public static class TaskServicesExtentions
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
        services.AddTransient<IUnitOfWork, EFUnitOfWork>();
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IDtoService<TaskDTO, Guid>, TaskService>();
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
