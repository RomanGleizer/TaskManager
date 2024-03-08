namespace ConnectionLib.ConnectionServices.DtoModels.AddTaskInProject;

public record AddTaskInProjectApiResponse
{
    public required int ProjectId { get; init; }
}
