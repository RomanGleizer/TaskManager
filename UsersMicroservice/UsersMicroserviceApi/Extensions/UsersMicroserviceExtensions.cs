using ConnectionLibrary.ConnectionServices;
using ConnectionLibrary.ConnectionServices.Interfaces;
using Core.Dal;
using Core.Dal.Base;
using Core.HttpLogic;
// using ProjectsMicroservice.ProjectsMicroserviceDomain.Entities;
using UsersMicroservice.UsersMicroserviceDal.Entities;
using UsersMicroservice.UsersMicroserviceDal.EntityFramework;
using UsersMicroservice.UsersMicroserviceDal.Interfaces;
using UsersMicroservice.UsersMicroserviceDal.Repositories;
using UsersMicroservice.UsersMicroserviceLogic.Dto.Role;
using UsersMicroservice.UsersMicroserviceLogic.Interfaces;
using UsersMicroservice.UsersMicroserviceLogic.Services;

namespace UsersMicroservice.UsersMicroserviceApi.Extensions;

public static class UsersMicroserviceExtensions
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
        services.AddTransient<IRepository<RoleDal, int>, RoleRepository>();
        services.AddTransient<IUserRepository<UserDal>, UserRepository>();
        services.AddTransient<IUnitOfWork, EntityFrameworkUnitOfWork>();
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IDtoService<RoleDto, int>, RoleService>();
    }

    public static void AddConnectionServices(this IServiceCollection services)
    {
        // services.AddTransient<IProjectConnectionService, ProjectConnectionService<Project>>();
    }

    public static void AddMicroserviceInteractionOperations(this IServiceCollection services)
    {
        services.AddTransient<IAddProjectIdToUserProjectIdList, AddProjectIdToUserProjectIdList<UserDal>>();
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
            .AddEntityFrameworkStores<UsersMicroserviceDbContext>();
    }
}