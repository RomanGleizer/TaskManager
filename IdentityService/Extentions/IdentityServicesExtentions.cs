using ConnectionLib.ConnectionServices.Interfaces;
using ConnectionLib.ConnectionServices;
using Core.Dal.Base;
using Core.Dal;
using Dal.Ef;
using Dal.Entities;
using Dal.Repositories;
using Domain.Entities;
using Logic.Interfaces;
using Logic.Services;
using Logic.Dto.Role;
using Dal.Interfaces;
using Core.HttpLogic;

namespace Api.Extentions;

public static class IdentityServicesExtentions
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
        services.AddTransient<IUnitOfWork, EFUnitOfWork>();
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IDtoService<RoleDto, int>, RoleService>();
    }

    public static void AddConnectionServices(this IServiceCollection services)
    {
        services.AddTransient<IProjectConnectionService, ProjectConnectionService<Project>>();
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
        .AddEntityFrameworkStores<IdentityServiceDbContext>();
    }
}
