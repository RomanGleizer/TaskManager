namespace ConnectionLib.ConnectionServices.DtoModels.TaskById;

public record ExistingTaskApiResponse
{
    public required bool IsExists { get; init; }
}
