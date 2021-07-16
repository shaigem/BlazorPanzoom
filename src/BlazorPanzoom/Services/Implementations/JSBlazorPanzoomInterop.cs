using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorPanzoom
{
    public class JSBlazorPanzoomInterop : IJSBlazorPanzoomInterop
    {
        private const string InteropIdentifier = "blazorPanzoom";

        private readonly IJSRuntime _jsRuntime;

        public JSBlazorPanzoomInterop(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async ValueTask<IJSObjectReference> CreatePanzoomAsync(ElementReference elementReference,
            PanzoomOptions? options = null)
        {
            return await Invoke<IJSObjectReference>("createPanzoomForReference", elementReference, options);
        }

        public async ValueTask<IJSObjectReference[]> CreatePanzoomAsync(string selector, PanzoomOptions? options = null)
        {
            return await Invoke<IJSObjectReference[]>("createPanzoomForSelector", selector, options);
        }

        public async ValueTask RegisterDefaultWheelZoom(ElementReference elementReference,
            IJSObjectReference jsPanzoomReference)
        {
            await InvokeVoid("registerDefaultWheelZoom", elementReference, jsPanzoomReference);
        }

        public async ValueTask RegisterWheelListenerAsync(
            DotNetObjectReference<IPanzoomWheelListener> dotNetObjectReference, ElementReference elementReference,
            IJSObjectReference jsPanzoomReference)
        {
            await InvokeVoid("registerWheelListener", dotNetObjectReference, elementReference, jsPanzoomReference);
        }

        public async ValueTask RemoveZoomWithWheelListenerAsync(
            ElementReference elementReference,
            IJSObjectReference jsPanzoomReference)
        {
            await InvokeVoid("removeWheelListeners", elementReference, jsPanzoomReference);
        }

        private async ValueTask
            InvokeVoid(string functionName, params object[] args)
        {
            await _jsRuntime.InvokeVoidAsync($"{InteropIdentifier}.{functionName}", args);
        }

        private async ValueTask<T> Invoke<T>(string functionName,
            params object?[] args)
        {
            return await _jsRuntime.InvokeAsync<T>($"{InteropIdentifier}.{functionName}", args);
        }
    }
}