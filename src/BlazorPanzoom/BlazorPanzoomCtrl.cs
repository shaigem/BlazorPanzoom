using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorPanzoom
{
    public class BlazorPanzoomCtrl : ComponentBase, IPanzoom, IAsyncDisposable
    {
        private DefaultPanzoom? _underlyingPanzoom;

        [Inject] private IPanzoomProvider PanzoomProvider { get; set; } = null!;

        public ElementReference ElementReference { private get; set; }
        [Parameter] public PanzoomOptions PanzoomOptions { private get; set; } = PanzoomOptions.DefaultOptions;
        [Parameter] public RenderFragment<BlazorPanzoomCtrl>? ChildContent { get; set; }

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
                    await PanzoomProvider.CreateForElementReference(ElementReference, PanzoomOptions) as DefaultPanzoom;
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        public async ValueTask DisposeAsync()
        {
            if (_underlyingPanzoom is not null)
            {
                await _underlyingPanzoom.DisposeAsync();
            }
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
    }
}