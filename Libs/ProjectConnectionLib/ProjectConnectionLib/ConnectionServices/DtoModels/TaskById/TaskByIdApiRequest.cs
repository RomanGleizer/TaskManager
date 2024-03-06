namespace ProjectConnectionLib.ConnectionServices.DtoModels.PendingTaskNameLists;

public record TaskByIdApiRequest
{
    public required int TaskId { get; init; }
}
