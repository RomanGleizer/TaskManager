using ConnectionLibrary.ConnectionServices.DtoModels.AddTaskInProject;
using ConnectionLibrary.ConnectionServices.DtoModels.ProjectById;
using ConnectionLibrary.ConnectionServices.Interfaces;
using MassTransit;

namespace TasksMicroservice.TasksMicroserviceLogic.TaskSaga;

public class CreateTaskSagaConsumer(IProjectConnectionService projectConnectionService)
    : IConsumer<CreateTaskSagaRequest>
{
    public async Task Consume(ConsumeContext<CreateTaskSagaRequest> context)
    {
        try
        {
            var project = await projectConnectionService.GetProjectByIdAsync(new IsProjectExistsRequest
            {
                ProjectId = context.Message.ProjectId
            });

            await projectConnectionService.AddTaskIdInProjectTaskIdsAsync(new AddTaskIdInProjectTaskIdsRequest
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