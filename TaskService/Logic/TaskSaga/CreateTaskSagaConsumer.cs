using ConnectionLib.ConnectionServices.DtoModels.AddTaskInProject;
using ConnectionLib.ConnectionServices.DtoModels.ProjectById;
using MassTransit;
using ConnectionLib.ConnectionServices.Interfaces;

namespace Logic.TaskSaga;

public class CreateTaskSagaConsumer(IProjectConnectionService projectConnectionService) : IConsumer<CreateTaskSagaRequest>
{
    private readonly IProjectConnectionService _projectConnectionService = projectConnectionService;

    public async Task Consume(ConsumeContext<CreateTaskSagaRequest> context)
    {
        try
        {
            var project = await _projectConnectionService.GetProjectByIdAsync(new IsProjectExistsRequest
            {
                ProjectId = context.Message.ProjectId
            });

            if (project == null)
            {
                await context.Publish<CreateTaskSagaFaulted>(new
                {
                    context.Message.ProjectId,
                    context.Message.TaskId,
                    Reason = "Проект не найден"
                });
            }
            else
            {
                await _projectConnectionService.AddTaskIdInProjectTaskIdsAsync(new AddTaskIdInProjectTaskIdsRequest
                {
                    ProjectId = context.Message.ProjectId,
                    TaskId = context.Message.TaskId
                });

                await context.Publish<CreateTaskSagaResponse>(new
                {
                    context.Message.ProjectId,
                    context.Message.TaskId
                });
            }
        }
        catch (Exception ex)
        {
            await context.Publish<CreateTaskSagaFaulted>(new
            {
                context.Message.ProjectId,
                context.Message.TaskId,
                Reason = ex.Message
            });
        }
    }
}