using MassTransit;

namespace TasksMicroservice.TasksMicroserviceLogic.TaskSaga;

public class CreateTaskSagaStateMachine : MassTransitStateMachine<CreateTaskSagaState>
{
    public CreateTaskSagaStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(
            () => CreateTaskRequested,
            eventCorrelationConfigurator => eventCorrelationConfigurator.CorrelateById(
                context => context.Message.ProjectId));

        Event(
            () => ProjectNotFound,
            eventCorrelationConfigurator => eventCorrelationConfigurator.CorrelateById(
                context => context.Message.ProjectId));

        Event(
            () => TaskAddedToProject,
            eventCorrelationConfigurator => eventCorrelationConfigurator.CorrelateById(
                context => context.Message.ProjectId));

        Initially(
            When(CreateTaskRequested)
                .Then(context =>
                {
                    context.Saga.ProjectId = context.Message.ProjectId;
                    context.Saga.TaskId = context.Message.TaskId;
                })
                .Publish(behaviorContext
                    => new CreateTaskSagaRequest(behaviorContext.Saga.ProjectId, behaviorContext.Saga.TaskId))
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

    public State CreatingTask { get; }
    public State Success { get; }
    public State Failed { get; }

    public Event<CreateTaskSagaRequest> CreateTaskRequested { get; }
    public Event<CreateTaskSagaResponse> TaskAddedToProject { get; }
    public Event<CreateTaskSagaFaulted> ProjectNotFound { get; }
}