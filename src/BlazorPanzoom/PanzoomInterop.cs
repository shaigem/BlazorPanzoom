using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace BlazorPanzoom
{
    public class PanzoomInterop : IPanzoom, IPanzoomWheelListener, IAsyncDisposable
    {
        private readonly IJSObjectReference _jsPanzoomReference;

        internal PanzoomInterop(IJSObjectReference jsPanzoomReference)
        {
            _jsPanzoomReference = jsPanzoomReference;
        }

        public Func<ValueTask>? OnDispose { private get; init; }

        public IJSObjectReference JSPanzoomReference => _jsPanzoomReference;

        public async ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);
            if (OnRemoveListener is not null)
            {
                await OnRemoveListener.Invoke();
            }

            await DestroyAsync();
            if (OnDispose is not null)
            {
                await OnDispose();
            }

            await _jsPanzoomReference.DisposeAsync();
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

        public async ValueTask ZoomWithWheel(WheelEventArgs args, IZoomOnlyOptions? overridenOptions = default)
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

        public async ValueTask DestroyAsync()
        {
            await _jsPanzoomReference.InvokeVoidAsync("destroy");
        }

        public EventCallback<WheelEventArgs> OnWheel { get; set; }
        public Func<ValueTask>? OnRemoveListener { get; set; }

        [JSInvokable]
        public async ValueTask OnCustomWheelEvent(WheelEventArgs args)
        {
            await OnWheel.InvokeAsync(args);
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