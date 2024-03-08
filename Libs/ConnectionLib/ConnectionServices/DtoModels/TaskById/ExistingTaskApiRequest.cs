namespace ConnectionLib.ConnectionServices.DtoModels.TaskById;

public record ExistingTaskApiRequest
{
    public required int TaskId { get; init; }
}
