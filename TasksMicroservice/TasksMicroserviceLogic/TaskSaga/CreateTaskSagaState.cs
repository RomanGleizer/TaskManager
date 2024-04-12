using MassTransit;

namespace TasksMicroservice.TasksMicroserviceLogic.TaskSaga;

public class CreateTaskSagaState : SagaStateMachineInstance
{
    public Guid ProjectId { get; set; }

    public Guid TaskId { get; set; }

    public State CurrentState { get; set; }
    public Guid CorrelationId { get; set; }
}