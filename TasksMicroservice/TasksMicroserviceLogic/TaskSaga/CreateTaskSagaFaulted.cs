namespace TasksMicroservice.TasksMicroserviceLogic.TaskSaga;

public record CreateTaskSagaFaulted(Guid ProjectId, Guid TaskId, string Reason)
{
    public CreateTaskSagaFaulted() : this(default, default, string.Empty)
    {
    }
}