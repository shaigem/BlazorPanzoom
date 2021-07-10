using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace BlazorPanzoom
{
    internal readonly struct PointArgs
    {
        internal PointArgs(double clientX = 0.0, double clientY = 0.0)
        {
            ClientX = clientX;
            ClientY = clientY;
        }

        public double ClientX { get; }
        public double ClientY { get; }
    }

    public class Panzoom : ComponentBase, IPanzoom, IAsyncDisposable
    {
        private DotNetObjectReference<Panzoom>? _dotNetObjectReference = null;
        private IJSObjectReference _jsPanzoomReference;
        public ElementReference ElementReference;
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Parameter] public RenderFragment<Panzoom> ChildContent { get; set; }
        [Parameter] public PanzoomOptions PanzoomOptions { private get; set; } = PanzoomOptions.DefaultOptions;
        [Parameter] public bool AddWheelZoom { get; set; }
        [Parameter] public EventCallback<PanzoomWheelEventArgs> OnWheel { get; set; }

        public async ValueTask DisposeAsync()
        {
            await JSRuntime.DisposePanzoomAsync(ElementReference, _jsPanzoomReference);
            await DestroyAsync();
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

        public async ValueTask ResetAsync(PanzoomOptions resetOptions)
        {
            await
                _jsPanzoomReference.InvokeVoidAsync("reset", resetOptions);
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

        public async ValueTask ZoomWithWheel(PanzoomWheelEventArgs args, IZoomOnlyOptions? overridenOptions = default)
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

        [JSInvokable]
        public async ValueTask OnCustomWheelEvent(PanzoomWheelEventArgs args)
        {
            await OnWheel.InvokeAsync(args);
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            builder.AddContent(0, ChildContent(this));
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var hasElementReference = !ElementReference.Equals(default(ElementReference));
                if (hasElementReference)
                {
                    _jsPanzoomReference = await JSRuntime.CreatePanzoomAsync(ElementReference, PanzoomOptions);

                    if (AddWheelZoom)
                    {
                        if (OnWheel.HasDelegate)
                            await RegisterCustomWheelListener();
                        else
                            await JSRuntime.RegisterWheelZoomAsync(ElementReference, _jsPanzoomReference);
                    }
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async ValueTask RegisterCustomWheelListener()
        {
            _dotNetObjectReference ??= DotNetObjectReference.Create(this);
            await JSRuntime.RegisterWheelListenerAsync(_dotNetObjectReference, ElementReference);
        }
    }
}