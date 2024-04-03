using ConnectionLib.ConnectionServices.Interfaces;
using ConnectionLib.ConnectionServices;
using Core.Dal.Base;
using Core.HttpLogic;
using Dal.Ef;
using Dal.Entities;
using Dal.Repositories;
using Domain.Entities;
using Infastracted.Data;
using Logic.Interfaces;
using Logic.Services;
using Services.Interfaces;
using Services.Services;
using Core.Dal;

namespace Api.Extentions;

public static class ProjectServicesExtentions
{
    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddHttpRequestService();
        services.AddSwaggerGen();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IProjectRepository<Project, Guid>, ProjectRepository>();
        services.AddTransient<IUserRepository<UserDal>, UserRepository>();
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IProjectService, ProjectService>();
        services.AddTransient<IUserService, UserService>();
    }

    public static void AddConnectionServices(this IServiceCollection services)
    {
        services.AddTransient<ITaskConnectionService, TaskConnectionService>();
        services.AddTransient<IUserConnectionService, UserConnectionService>();
    }

    public static void AddMicroserviceInteractionOperations(this IServiceCollection services)
    {
        services.AddTransient<IAddProjectIdToUserProjectIdList, AddProjectIdToUserProjectIdList<UserDal>>();
        services.AddTransient<IAddTaskIdToProjectTaskIdList, AddTaskIdToProjectTaskIdList<Project>>();
        services.AddTransient<IGetProjectById<Project>, GetProjectById<Project>>();
    }

    public static void AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<UserDal, RoleDal>(options =>
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
    }
}
