namespace BlazorPanzoom
{
    public interface IBlazorPanzoomEvent
    {
    }

    public delegate void BlazorPanzoomEventHandler<in T>(T args) where T : IBlazorPanzoomEvent;

    public delegate void BlazorPanzoomEventHandler();
}