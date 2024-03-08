namespace ProjectConnectionLib.ConnectionServices.DtoModels.PendingTaskNameLists;

public record ExistingTaskApiRequest
{
    public required int TaskId { get; init; }
}
