using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace Logic.TaskSaga;

public class SagasDbContext : SagaDbContext
{
    public SagasDbContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override IEnumerable<ISagaClassMap> Configurations =>
    [
        new CreateTaskSagaStateMap()
    ];
}
