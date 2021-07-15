using System;
using System.Collections.Generic;
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
        private HashSet<ElementReference>? _excludedElements = null;
        private IJSObjectReference? _jsPanzoomReference;
        public ElementReference ElementReference { private get; set; }

        public ElementReference ExcludedElementReference
        {
            set => AddToExcludedElements(value);
        }

        [Inject] private IJSRuntime JsRuntime { get; set; } = null!;
        [Parameter] public RenderFragment<Panzoom>? ChildContent { get; set; }
        [Parameter] public PanzoomOptions PanzoomOptions { private get; set; } = PanzoomOptions.DefaultOptions;
        [Parameter] public WheelHandler WheelHandler { private get; set; } = WheelHandler.None;
        [Parameter] public EventCallback<PanzoomWheelEventArgs> OnWheel { private get; set; }

        public async ValueTask DisposeAsync()
        {
            if (WheelHandler.Equals(WheelHandler.Custom))
            {
                await JsRuntime.RemoveWheelListenerAsync(ElementReference);
            }

            if (_jsPanzoomReference is not null)
            {
                if (WheelHandler.Equals(WheelHandler.ZoomOnScroll))
                {
                    await JsRuntime.RemoveZoomWithWheelListenerAsync(ElementReference, _jsPanzoomReference);
                }

                await DestroyAsync();
                await _jsPanzoomReference.DisposeAsync();
                _jsPanzoomReference = null;
            }

            _dotNetObjectReference?.Dispose();
        }

        public async ValueTask ZoomInAsync()
        {
            await JsPanzoomInvokeVoidAsync("zoomIn");
        }

        public async ValueTask ZoomOutAsync()
        {
            await JsPanzoomInvokeVoidAsync("zoomOut");
        }

        public async ValueTask ZoomAsync(double toScale)
        {
            await JsPanzoomInvokeVoidAsync("zoom", toScale);
        }

        public async ValueTask ZoomToPointAsync(double toScale, double clientX, double clientY,
            IZoomOnlyOptions? overridenZoomOptions = default)
        {
            await JsPanzoomInvokeVoidAsync("zoomToPoint", toScale, new PointArgs(clientX, clientY),
                overridenZoomOptions);
        }

        public async ValueTask ResetAsync(PanzoomOptions resetOptions)
        {
            await
                JsPanzoomInvokeVoidAsync("reset", resetOptions);
        }

        public async ValueTask ResetAsync()
        {
            await JsPanzoomInvokeVoidAsync("reset");
        }

        public async ValueTask SetOptionsAsync(PanzoomOptions options)
        {
            await JsPanzoomInvokeVoidAsync("setOptions", options);
        }

        public async ValueTask<PanzoomOptions> GetOptionsAsync()
        {
            return await JsPanzoomInvokeAsync<PanzoomOptions>("getOptions");
        }

        public async ValueTask<double> GetScaleAsync()
        {
            return await JsPanzoomInvokeAsync<double>("getScale");
        }

        public async ValueTask DestroyAsync()
        {
            await JsPanzoomInvokeVoidAsync("destroy");
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
            if (ChildContent is null)
            {
                return;
            }

            base.BuildRenderTree(builder);
            builder.AddContent(0, ChildContent(this));
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await InitializeJsPanzoom();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task UpdateExcludedElements()
        {
            if (_excludedElements is null)
            {
                return;
            }

            if (_excludedElements.Count == 0)
            {
                return;
            }

            // 1. Get the current excluded elements from JS
            // 2. Get the new excluded elements set by the user
            // 3. Combine the current and new excluded elements
            // 4. Send the combined array to JS
            var currentOptions = await GetOptionsAsync();
            var excludedElements = currentOptions.GetExcludeOrDefault();
            var newExcludedElements = new ElementReference[excludedElements.Length + _excludedElements.Count];
            excludedElements.CopyTo(newExcludedElements, 0);
            _excludedElements.CopyTo(newExcludedElements, excludedElements.Length);
            await SetOptionsAsync(new PanzoomOptions {Exclude = newExcludedElements});
            // TODO is this the best way?
        }

        private void AddToExcludedElements(ElementReference reference)
        {
            _excludedElements ??= new HashSet<ElementReference>();
            _excludedElements.Add(reference);
        }

        private async Task InitializeJsPanzoom()
        {
            // TODO a JS panzoom object MUST be created and shouldn't be null!
            var hasElementReference = !ElementReference.IsDefault();
            if (hasElementReference)
            {
                _jsPanzoomReference = await JsRuntime.CreatePanzoomAsync(ElementReference, PanzoomOptions);
                switch (WheelHandler)
                {
                    case WheelHandler.Custom when OnWheel.HasDelegate:
                        await RegisterCustomWheelListener();
                        break;
                    case WheelHandler.ZoomOnScroll:
                        await JsRuntime.RegisterDefaultWheelZoom(ElementReference, _jsPanzoomReference);
                        break;
                    case WheelHandler.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                await UpdateExcludedElements();
            }
        }

        private async ValueTask RegisterCustomWheelListener()
        {
            _dotNetObjectReference ??= DotNetObjectReference.Create(this);
            await JsRuntime.RegisterWheelListenerAsync(_dotNetObjectReference, ElementReference);
        }

        private async ValueTask JsPanzoomInvokeVoidAsync(string identifier, params object?[] args)
        {
            if (_jsPanzoomReference is null)
            {
                throw new NullReferenceException("JS Panzoom reference is null!");
            }

            await _jsPanzoomReference.InvokeVoidAsync(identifier, args);
        }

        private async ValueTask<T> JsPanzoomInvokeAsync<T>(string identifier, params object?[] args)
        {
            if (_jsPanzoomReference is null)
            {
                throw new NullReferenceException("JS Panzoom reference is null!");
            }

            return await _jsPanzoomReference.InvokeAsync<T>(identifier, args);
        }
    }
}