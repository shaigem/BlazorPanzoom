using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorPanzoom
{
    public class BlazorPanzoomCtrl : ComponentBase, IAsyncDisposable
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
    }
}