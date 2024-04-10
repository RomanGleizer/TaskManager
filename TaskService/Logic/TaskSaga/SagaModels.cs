namespace Logic.TaskSaga;

public record CreateTaskSagaRequest(Guid ProjectId, Guid TaskId)
{
    public CreateTaskSagaRequest() : this(default, default) { }
}

public record CreateTaskSagaResponse(Guid ProjectId, Guid TaskId)
{
    public CreateTaskSagaResponse() : this(default, default) { }
}

public record CreateTaskSagaCompleted(Guid ProjectId, Guid TaskId)
{
    public CreateTaskSagaCompleted() : this(default, default) { }
}

public record CreateTaskSagaFaulted(Guid ProjectId, Guid TaskId, string Reason)
{
    public CreateTaskSagaFaulted() : this(default, default, string.Empty) { }
}
