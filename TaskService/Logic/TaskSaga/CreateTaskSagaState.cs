using MassTransit;

namespace Logic.TaskSaga;

public class CreateTaskSagaState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }

    public Guid ProjectId { get; set; }

    public Guid TaskId { get; set; }

    public State CurrentState { get; set; }
}