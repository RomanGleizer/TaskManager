using Core.TraceLogic.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Core.TraceIdLogic;

public static class StartUpTraceId
{
    public static IServiceCollection TryAddTraceId(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<TraceIdAccessor>();
        serviceCollection
            .TryAddScoped<ITraceWriter>(provider => ServiceProviderServiceExtensions.GetRequiredService<TraceIdAccessor>(provider));
        serviceCollection
            .TryAddScoped<ITraceReader>(provider => provider.GetRequiredService<TraceIdAccessor>());
        serviceCollection
            .TryAddScoped<ITraceIdAccessor>(provider => provider.GetRequiredService<TraceIdAccessor>());

        return serviceCollection;
    }
}