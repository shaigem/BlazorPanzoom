using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorPanzoom
{
    public class Panzoom : ComponentBase, IPanzoom, IAsyncDisposable
    {
        private HashSet<ElementReference>? _excludedElements = null;
        private PanzoomInterop _underlyingPanzoomInterop = null!;
        [Inject] private IPanzoomHelper PanzoomHelper { get; set; } = null!;

        public ElementReference ElementReference { private get; set; }

        public ElementReference ExcludedElementReference
        {
            set => AddToExcludedElements(value);
        }

        [Parameter] public WheelMode WheelMode { get; set; } = WheelMode.None;
        [Parameter] public EventCallback<CustomWheelEventArgs> OnWheel { get; set; }
        [Parameter] public PanzoomOptions PanzoomOptions { private get; set; } = PanzoomOptions.DefaultOptions;
        [Parameter] public RenderFragment<Panzoom>? ChildContent { get; set; }
        [Parameter] public EventCallback<SetTransformEventArgs> SetTransform { get; set; }

        public async ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);
            if (_underlyingPanzoomInterop != null)
            {
                await _underlyingPanzoomInterop.DisposeAsync();
            }
        }

        public async ValueTask PanAsync(double x, double y, IPanOnlyOptions? overridenOptions = default)
        {
            await _underlyingPanzoomInterop.PanAsync(x, y, overridenOptions);
        }

        public async ValueTask ZoomInAsync(IZoomOnlyOptions? options = default)
        {
            await _underlyingPanzoomInterop.ZoomInAsync();
        }

        public async ValueTask ZoomOutAsync(IZoomOnlyOptions? options = default)
        {
            await _underlyingPanzoomInterop.ZoomOutAsync();
        }

        public async ValueTask ZoomAsync(double toScale, IZoomOnlyOptions? options = default)
        {
            await _underlyingPanzoomInterop.ZoomAsync(toScale);
        }

        public async ValueTask ZoomToPointAsync(double toScale, double clientX, double clientY,
            IZoomOnlyOptions? overridenZoomOptions = default)
        {
            await _underlyingPanzoomInterop.ZoomToPointAsync(toScale, clientX, clientY, overridenZoomOptions);
        }

        public async ValueTask ResetAsync(PanzoomOptions? options = default)
        {
            await _underlyingPanzoomInterop.ResetAsync();
        }

        public async ValueTask SetOptionsAsync(PanzoomOptions options)
        {
            await _underlyingPanzoomInterop.SetOptionsAsync(options);
        }

        public async ValueTask<PanzoomOptions> GetOptionsAsync()
        {
            return await _underlyingPanzoomInterop.GetOptionsAsync();
        }

        public async ValueTask<double> GetScaleAsync()
        {
            return await _underlyingPanzoomInterop.GetScaleAsync();
        }

        public async ValueTask<ReadOnlyFocalPoint> GetPanAsync()
        {
            return await _underlyingPanzoomInterop.GetPanAsync();
        }

        public async ValueTask ResetStyleAsync()
        {
            await _underlyingPanzoomInterop.ResetStyleAsync();
        }

        public async ValueTask SetStyleAsync(string name, string value)
        {
            await _underlyingPanzoomInterop.SetStyleAsync(name, value);
        }

        public async ValueTask DestroyAsync()
        {
            await _underlyingPanzoomInterop.DestroyAsync();
        }

        public async ValueTask ZoomWithWheelAsync(CustomWheelEventArgs args,
            IZoomOnlyOptions? overridenOptions = default)
        {
            await _underlyingPanzoomInterop.ZoomWithWheelAsync(args, overridenOptions);
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
                if (ElementReference.IsDefault())
                {
                    throw new ArgumentException("ElementReference must be set for Panzoom to work");
                }

                _underlyingPanzoomInterop =
                    await PanzoomHelper.CreateForElementReferenceAsync(ElementReference,
                        PanzoomOptions);

                if (WheelMode.Equals(WheelMode.Custom))
                {
                    if (!OnWheel.HasDelegate)
                    {
                        throw new ArgumentException("OnWheel must be set when using WheelMode.Custom!");
                    }

                    await PanzoomHelper.RegisterWheelListenerAsync(_underlyingPanzoomInterop, OnWheel,
                        ElementReference);
                }
                else if (WheelMode.Equals(WheelMode.ZoomWithWheel))
                {
                    await PanzoomHelper.RegisterZoomWithWheelAsync(_underlyingPanzoomInterop, ElementReference);
                }

                if (SetTransform.HasDelegate)
                {
                    await PanzoomHelper.SetTransformAsync(_underlyingPanzoomInterop, SetTransform);
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

            var currentOptions = await GetOptionsAsync();
            var excludedElements = currentOptions.GetExcludeOrDefault();
            _excludedElements.UnionWith(excludedElements);
            await SetOptionsAsync(new PanzoomOptions {Exclude = _excludedElements});
        }

        private void AddToExcludedElements(ElementReference reference)
        {
            if (reference.IsDefault())
            {
                return;
            }

            _excludedElements ??= new HashSet<ElementReference>();
            _excludedElements.Add(reference);
        }
    }
}
