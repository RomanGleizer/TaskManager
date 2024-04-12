namespace TasksMicroservice.TasksMicroserviceLogic.TaskSaga;

public record CreateTaskSagaResponse(Guid ProjectId, Guid TaskId)
{
    public CreateTaskSagaResponse() : this(default, default)
    {
    }
}