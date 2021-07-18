using System.Threading.Tasks;

namespace BlazorPanzoom
{
    public static class BlazorPanzoomEventExtensions
    {
        public static Task InvokeAsync(this BlazorPanzoomEventHandler? handler)
        {
            handler?.Invoke();
            return Task.CompletedTask;
        }

        public static Task InvokeAsync<T>(this BlazorPanzoomEventHandler<T>? handler, IBlazorPanzoomEvent @event)
            where T : IBlazorPanzoomEvent
        {
            handler?.Invoke((T) @event);
            return Task.CompletedTask;
        }
    }
}