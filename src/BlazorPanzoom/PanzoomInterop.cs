using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace BlazorPanzoom
{
    public delegate void BlazorPanzoomEventHandler<in T>(T args) where T : IBlazorPanzoomEvent;

    public delegate void BlazorPanzoomEventHandler();

    public interface IBlazorPanzoomEvent
    {
    }

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


    public record CustomWheelEventArgs
        (double DeltaX, double DeltaY, double ClientX, double ClientY, bool ShiftKey) : IBlazorPanzoomEvent;

    public class PanzoomInterop : IPanzoom, IAsyncDisposable
    {
        private readonly IJSObjectReference _jsPanzoomReference;

        internal PanzoomInterop(IJSObjectReference jsPanzoomReference)
        {
            _jsPanzoomReference = jsPanzoomReference;
        }

        public IJSObjectReference JSPanzoomReference => _jsPanzoomReference;


        public async ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);
            await OnDispose.InvokeAsync();
            await DestroyAsync();
            await _jsPanzoomReference.DisposeAsync();
            DisposeAllEventHandlers();
        }

        public async ValueTask PanAsync(double x, double y, IPanOnlyOptions? overridenOptions = default)
        {
            await _jsPanzoomReference.InvokeVoidAsync("pan", x, y, overridenOptions);
        }

        public async ValueTask ZoomInAsync()
        {
            await _jsPanzoomReference.InvokeVoidAsync("zoomIn");
        }

        public async ValueTask ZoomOutAsync()
        {
            await _jsPanzoomReference.InvokeVoidAsync("zoomOut");
        }

        public async ValueTask ZoomAsync(double toScale)
        {
            await _jsPanzoomReference.InvokeVoidAsync("zoom", toScale);
        }

        public async ValueTask ZoomToPointAsync(double toScale, double clientX, double clientY,
            IZoomOnlyOptions? overridenZoomOptions = default)
        {
            await _jsPanzoomReference.InvokeVoidAsync("zoomToPoint", toScale, new PointArgs(clientX, clientY),
                overridenZoomOptions);
        }

        public async ValueTask ZoomWithWheelAsync(CustomWheelEventArgs args,
            IZoomOnlyOptions? overridenOptions = default)
        {
            var currentOptions = await GetOptionsAsync();
            var currentScale = await GetScaleAsync();
            var minScale = currentOptions.GetMinScaleOrDefault();
            var maxScale = currentOptions.GetMaxScaleOrDefault();
            var step = currentOptions.GetStepOrDefault();
            if (overridenOptions is not null)
            {
                minScale = overridenOptions.GetMinScaleOrDefault(minScale);
                maxScale = overridenOptions.GetMaxScaleOrDefault(maxScale);
                step = overridenOptions.GetStepOrDefault(step);
            }

            var delta = args.DeltaY == 0 && args.DeltaX != 0 ? args.DeltaX : args.DeltaY;
            var direction = delta < 0 ? 1 : -1;
            var calculatedScale = currentScale * Math.Exp(direction * step / 3);
            var constrainedScale = Math.Min(Math.Max(calculatedScale, minScale), maxScale);
            await ZoomToPointAsync(constrainedScale, args.ClientX, args.ClientY, overridenOptions);
        }

        public async ValueTask ResetAsync(PanzoomOptions resetOptions)
        {
            await _jsPanzoomReference.InvokeVoidAsync("reset");
        }

        public async ValueTask ResetAsync()
        {
            await _jsPanzoomReference.InvokeVoidAsync("reset");
        }

        public async ValueTask SetOptionsAsync(PanzoomOptions options)
        {
            await _jsPanzoomReference.InvokeVoidAsync("setOptions", options);
        }

        public async ValueTask<PanzoomOptions> GetOptionsAsync()
        {
            return await _jsPanzoomReference.InvokeAsync<PanzoomOptions>("getOptions");
        }

        public async ValueTask<double> GetScaleAsync()
        {
            return await _jsPanzoomReference.InvokeAsync<double>("getScale");
        }

        public async ValueTask<FocalPoint2> GetPanAsync()
        {
            return await _jsPanzoomReference.InvokeAsync<FocalPoint2>("getPan");
        }

        public async ValueTask SetStyleAsync(string name, string value)
        {
            await _jsPanzoomReference.InvokeVoidAsync("setStyle", name, value);
        }

        public async ValueTask DestroyAsync()
        {
            await _jsPanzoomReference.InvokeVoidAsync("destroy");
        }

        public event BlazorPanzoomEventHandler<CustomWheelEventArgs>? OnCustomWheel;
        public event BlazorPanzoomEventHandler<SetTransformEventArgs>? OnSetTransform;
        public event BlazorPanzoomEventHandler? OnDispose;

        [JSInvokable]
        public async ValueTask OnCustomWheelEvent(CustomWheelEventArgs args)
        {
            await OnCustomWheel.InvokeAsync(args);
        }

        [JSInvokable]
        public async ValueTask OnSetTransformEvent(SetTransformEventArgs args)
        {
            await OnSetTransform.InvokeAsync(args);
        }

        private void DisposeAllEventHandlers()
        {
            OnCustomWheel = null;
            OnSetTransform = null;
            OnDispose = null;
        }


        protected bool Equals(PanzoomInterop other)
        {
            return _jsPanzoomReference.Equals(other._jsPanzoomReference);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PanzoomInterop) obj);
        }

        public override int GetHashCode()
        {
            return _jsPanzoomReference.GetHashCode();
        }

        public static bool operator ==(PanzoomInterop? left, PanzoomInterop? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PanzoomInterop? left, PanzoomInterop? right)
        {
            return !Equals(left, right);
        }
    }
}