using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace BlazorPanZoom
{
    public class BlazorPanZoom : ComponentBase, IPanZoom
    {
        private IJSObjectReference _jsPanzoomReference;

        public ElementReference ElementReference;
        [Inject] private IJSRuntime JsRuntime { get; set; }
        [Parameter] public RenderFragment<BlazorPanZoom> ChildContent { get; set; }

        public async ValueTask ZoomInAsync() => await _jsPanzoomReference.InvokeVoidAsync("zoomIn");

        public async ValueTask PanAsync(double toX, double toY) =>
            await _jsPanzoomReference.InvokeVoidAsync("pan", toX, toY);

        public async ValueTask ZoomAsync(double toScale) =>
            await _jsPanzoomReference.InvokeVoidAsync("zoom", toScale);

        public async ValueTask ZoomToPointAsync(double toScale, double toX, double toY) =>
            await _jsPanzoomReference.InvokeVoidAsync("zoomToPoint", toScale, toX, toY);

        public async ValueTask ResetAsync() => await _jsPanzoomReference.InvokeVoidAsync("reset");
        public async ValueTask DestroyAsync() => await _jsPanzoomReference.InvokeVoidAsync("destroy");
        
        public async ValueTask DisposeAsync()
        {
            await DestroyAsync();
            await _jsPanzoomReference.DisposeAsync();
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
                _jsPanzoomReference = await CreatePanzoomAsync(ElementReference);
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async ValueTask<IJSInProcessObjectReference> CreatePanzoomAsync(ElementReference element) =>
            await JsRuntime.InvokeAsync<IJSInProcessObjectReference>("blazorPanzoom.createPanzoom", element);
    }
}