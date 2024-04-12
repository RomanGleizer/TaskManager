using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logic.TaskSaga;

public class CreateTaskSagaStateMap : SagaClassMap<CreateTaskSagaState>
{
    protected override void Configure(EntityTypeBuilder<CreateTaskSagaState> entity, ModelBuilder model)
    {
        base.Configure(entity, model);
        entity.Property(x => x.CurrentState);
    }
}
