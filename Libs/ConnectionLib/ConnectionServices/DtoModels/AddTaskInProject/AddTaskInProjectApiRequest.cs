namespace ConnectionLib.ConnectionServices.DtoModels.AddTaskInProject;

public record AddTaskInProjectApiRequest
{
    public required int ProjectId { get; init; }
}
