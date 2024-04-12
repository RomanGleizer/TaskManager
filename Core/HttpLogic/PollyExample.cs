using Polly;

namespace Core.HttpLogic;

public class PollyExample
{
    public static async Task<string> ActionAsync()
    {
#pragma warning disable CS1998 // В асинхронном методе отсутствуют операторы await, будет выполнен синхронный метод
        var res = await Policy
            .Handle<Exception>()
            .WaitAndRetryForeverAsync(
                i => TimeSpan.FromSeconds(5 + i), (result, retryCount, _) =>
                {
                    Console.WriteLine($"Начало {retryCount} Попытки повтора");
                    return Task.CompletedTask;
                })
            .ExecuteAsync(async () => { return "hello world"; });
#pragma warning restore CS1998 // В асинхронном методе отсутствуют операторы await, будет выполнен синхронный метод

        return res;
    }
}