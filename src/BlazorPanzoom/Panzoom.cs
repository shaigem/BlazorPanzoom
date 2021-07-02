using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace BlazorPanzoom
{
    public class Panzoom : ComponentBase, IPanzoom, IAsyncDisposable
    {
        private IJSObjectReference _jsPanzoomReference;

        public ElementReference ElementReference;
        [Inject] private IJSRuntime JsRuntime { get; set; }
        [Parameter] public RenderFragment<Panzoom> ChildContent { get; set; }
        [Parameter] public PanzoomOptions PanzoomOptions { private get; set; } = PanzoomOptions.DefaultOptions;
        [Parameter] public string IdSelector { get; set; }
        [Parameter] public string CssSelector { get; set; }

        public async ValueTask ZoomInAsync() => await _jsPanzoomReference.InvokeVoidAsync("zoomIn");
        public async ValueTask ZoomOutAsync() => await _jsPanzoomReference.InvokeVoidAsync("zoomOut");

        public async ValueTask ZoomAsync(double toScale) =>
            await _jsPanzoomReference.InvokeVoidAsync("zoom", toScale);

        public async ValueTask ResetAsync(PanzoomOptions resetOptions) => await
            _jsPanzoomReference.InvokeVoidAsync("reset", resetOptions);

        public async ValueTask ResetAsync() => await _jsPanzoomReference.InvokeVoidAsync("reset");

        public async ValueTask SetOptionsAsync(PanzoomOptions options) =>
            await _jsPanzoomReference.InvokeVoidAsync("setOptions", options);

        public async ValueTask<PanzoomOptions> GetOptionsAsync() =>
            await _jsPanzoomReference.InvokeAsync<PanzoomOptions>("getOptions");

        public async ValueTask<double> GetScaleAsync() => await _jsPanzoomReference.InvokeAsync<double>("getScale");
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

        public override Task SetParametersAsync(ParameterView parameters)
        {
            var hasIdSelector = parameters.TryGetValue<string>(IdSelector, out _);
            var hasCssSelector = parameters.TryGetValue<string>(CssSelector, out _);
            if (hasIdSelector && hasCssSelector)
            {
                throw new ArgumentException("Cannot have both an id and a css selector set!");
            }

            return base.SetParametersAsync(parameters);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var hasElementReference = !ElementReference.Equals(default(ElementReference));
                if (hasElementReference)
                {
                    _jsPanzoomReference = await CreatePanzoomAsync(ElementReference, PanzoomOptions);
                }
                else
                {
                    var hasIdSelector = !string.IsNullOrEmpty(IdSelector);
                    var hasCssSelector = !string.IsNullOrEmpty(CssSelector);
                    var hasSelector = hasIdSelector || hasCssSelector;
                    if (!hasSelector)
                    {
                        throw new ArgumentException("Must have an element reference set or an id or css selector set!");
                    }

                    if (hasIdSelector)
                    {
                        _jsPanzoomReference = await CreatePanzoomForIdAsync(IdSelector);
                    }
                    else if (hasCssSelector)
                    {
                        _jsPanzoomReference = await CreatePanzoomForSelectorAsync(CssSelector);
                    }
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async ValueTask<IJSObjectReference>
            CreatePanzoomAsync(ElementReference element, PanzoomOptions options) =>
            await JsRuntime.InvokeAsync<IJSObjectReference>("blazorPanzoom.createPanzoomForReference", element,
                options);

        private async ValueTask<IJSObjectReference> CreatePanzoomForIdAsync(string id)
        {
            return await JsRuntime.InvokeAsync<IJSObjectReference>("blazorPanzoom.createPanzoomForId", id);
        }

        private async ValueTask<IJSObjectReference> CreatePanzoomForSelectorAsync(string cssSelector)
        {
            return await JsRuntime.InvokeAsync<IJSObjectReference>("blazorPanzoom.createPanzoomForSelector",
                cssSelector);
        }
    }
}