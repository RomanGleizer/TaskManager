using MassTransit;

namespace Logic.TaskSaga;

public class CreateTaskSagaStateMachine : MassTransitStateMachine<CreateTaskSagaState>
{
    public CreateTaskSagaStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => CreateTaskRequested, x => x.CorrelateById(context => context.Message.ProjectId));
        Event(() => ProjectNotFound, x => x.CorrelateById(context => context.Message.ProjectId));
        Event(() => TaskAddedToProject, x => x.CorrelateById(context => context.Message.ProjectId));

        Initially(
            When(CreateTaskRequested)
                .Then(context =>
                {
                    context.Instance.ProjectId = context.Data.ProjectId;
                    context.Instance.TaskId = context.Data.TaskId;
                })
                .Publish(ctx => new CreateTaskSagaRequest(ctx.Instance.ProjectId, ctx.Instance.TaskId))
                .TransitionTo(CreatingTask)
        );

        During(CreatingTask,
            When(ProjectNotFound)
                .Then(context => throw new Exception("Проект не найден"))
                .TransitionTo(Failed)
        );

        During(CreatingTask,
            When(TaskAddedToProject)
                .TransitionTo(Success)
        );

        SetCompletedWhenFinalized();
    }

    public State CreatingTask { get; private set; }
    public State Success { get; private set; }
    public State Failed { get; private set; }

    public Event<CreateTaskSagaRequest> CreateTaskRequested { get; private set; }
    public Event<CreateTaskSagaResponse> TaskAddedToProject { get; private set; }
    public Event<CreateTaskSagaFaulted> ProjectNotFound { get; private set; }
}