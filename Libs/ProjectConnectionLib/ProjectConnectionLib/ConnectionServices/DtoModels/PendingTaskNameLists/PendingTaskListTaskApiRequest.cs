namespace ProjectConnectionLib.ConnectionServices.DtoModels.PendingTaskNameLists;

public record PendingTaskListTaskApiRequest
{
    public required int[] TaskIdList { get; init; }
}
