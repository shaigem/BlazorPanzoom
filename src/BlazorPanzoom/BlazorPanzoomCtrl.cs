using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorPanzoom
{
    public class BlazorPanzoomCtrl : ComponentBase, IPanzoom, IAsyncDisposable
    {
        private HashSet<ElementReference>? _excludedElements = null;
        private DefaultPanzoom _underlyingPanzoom = null!;
        [Inject] private IPanzoomProvider PanzoomProvider { get; set; } = null!;

        public ElementReference ElementReference { private get; set; }

        public ElementReference ExcludedElementReference
        {
            set => AddToExcludedElements(value);
        }

        [Parameter] public PanzoomOptions PanzoomOptions { private get; set; } = PanzoomOptions.DefaultOptions;
        [Parameter] public WheelHandler WheelHandler { private get; set; } = WheelHandler.None;
        [Parameter] public EventCallback<PanzoomWheelEventArgs> OnWheel { private get; set; }
        [Parameter] public RenderFragment<BlazorPanzoomCtrl>? ChildContent { get; set; }

        public async ValueTask DisposeAsync()
        {
            await _underlyingPanzoom.DisposeAsync();
        }

        public async ValueTask ZoomInAsync()
        {
            await _underlyingPanzoom.ZoomInAsync();
        }

        public async ValueTask ZoomOutAsync()
        {
            await _underlyingPanzoom.ZoomOutAsync();
        }

        public async ValueTask ZoomAsync(double toScale)
        {
            await _underlyingPanzoom.ZoomAsync(toScale);
        }

        public async ValueTask ZoomToPointAsync(double toScale, double clientX, double clientY,
            IZoomOnlyOptions? overridenZoomOptions)
        {
            await _underlyingPanzoom.ZoomToPointAsync(toScale, clientX, clientY, overridenZoomOptions);
        }

        public async ValueTask ResetAsync(PanzoomOptions resetOptions)
        {
            await _underlyingPanzoom.ResetAsync(resetOptions);
        }

        public async ValueTask ResetAsync()
        {
            await _underlyingPanzoom.ResetAsync();
        }

        public async ValueTask SetOptionsAsync(PanzoomOptions options)
        {
            await _underlyingPanzoom.SetOptionsAsync(options);
        }

        public async ValueTask<PanzoomOptions> GetOptionsAsync()
        {
            return await _underlyingPanzoom.GetOptionsAsync();
        }

        public async ValueTask<double> GetScaleAsync()
        {
            return await _underlyingPanzoom.GetScaleAsync();
        }

        public async ValueTask DestroyAsync()
        {
            await _underlyingPanzoom.DestroyAsync();
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
            Console.WriteLine(constrainedScale);
            await ZoomToPointAsync(constrainedScale, args.ClientX, args.ClientY, overridenOptions);
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
                _underlyingPanzoom =
                    (DefaultPanzoom) await PanzoomProvider.CreateForElementReference(ElementReference, PanzoomOptions);
                if (!WheelHandler.Equals(WheelHandler.None))
                {
                    await PanzoomProvider.RegisterZooming(ElementReference, _underlyingPanzoom, WheelHandler, OnWheel);
                }

                await UpdateExcludedElements();
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
    }
}