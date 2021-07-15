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
        [Parameter] public RenderFragment<BlazorPanzoomCtrl>? ChildContent { get; set; }

        public async ValueTask DisposeAsync()
        {
            await _underlyingPanzoom.DisposeAsync();
        }

        public ValueTask ZoomInAsync()
        {
            return _underlyingPanzoom.ZoomInAsync();
        }

        public ValueTask ZoomOutAsync()
        {
            return _underlyingPanzoom.ZoomOutAsync();
        }

        public ValueTask ZoomAsync(double toScale)
        {
            return _underlyingPanzoom.ZoomAsync(toScale);
        }

        public ValueTask ZoomToPointAsync(double toScale, double clientX, double clientY,
            IZoomOnlyOptions? overridenZoomOptions)
        {
            return _underlyingPanzoom.ZoomToPointAsync(toScale, clientX, clientY, overridenZoomOptions);
        }

        public ValueTask ResetAsync(PanzoomOptions resetOptions)
        {
            return _underlyingPanzoom.ResetAsync(resetOptions);
        }

        public ValueTask ResetAsync()
        {
            return _underlyingPanzoom.ResetAsync();
        }

        public ValueTask SetOptionsAsync(PanzoomOptions options)
        {
            return _underlyingPanzoom.SetOptionsAsync(options);
        }

        public ValueTask<PanzoomOptions> GetOptionsAsync()
        {
            return _underlyingPanzoom.GetOptionsAsync();
        }

        public ValueTask<double> GetScaleAsync()
        {
            return _underlyingPanzoom.GetScaleAsync();
        }

        public ValueTask DestroyAsync()
        {
            return _underlyingPanzoom.DestroyAsync();
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