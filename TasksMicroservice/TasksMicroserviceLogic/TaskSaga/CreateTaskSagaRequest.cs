namespace TasksMicroservice.TasksMicroserviceLogic.TaskSaga;

public record CreateTaskSagaRequest(Guid ProjectId, Guid TaskId)
{
    public CreateTaskSagaRequest() : this(default, default)
    {
    }
}