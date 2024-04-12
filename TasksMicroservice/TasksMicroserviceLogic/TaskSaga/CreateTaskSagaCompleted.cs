namespace TasksMicroservice.TasksMicroserviceLogic.TaskSaga;

public record CreateTaskSagaCompleted(Guid ProjectId, Guid TaskId)
{
    public CreateTaskSagaCompleted() : this(default, default)
    {
    }
}