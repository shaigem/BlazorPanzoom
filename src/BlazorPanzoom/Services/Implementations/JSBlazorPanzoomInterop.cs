using System.Collections.Generic;
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

        public async ValueTask RegisterSetTransformAsync(DotNetObjectReference<IPanzoom> dotNetObjectReference,
            IJSObjectReference jsPanzoomReference)
        {
            await InvokeVoid("registerSetTransform", dotNetObjectReference, jsPanzoomReference);
        }

        public async ValueTask RegisterZoomWithWheelAsync(IJSObjectReference jsPanzoomReference,
            ElementReference? elementReference = null)
        {
            await InvokeVoid("registerZoomWithWheel", jsPanzoomReference, elementReference);
        }

        public async ValueTask RegisterWheelListenerAsync(
            DotNetObjectReference<PanzoomInterop> dotNetObjectReference, IJSObjectReference jsPanzoomReference,
            ElementReference? elementReference = null)
        {
            await InvokeVoid("registerWheelListener", dotNetObjectReference, jsPanzoomReference, elementReference);
        }

        public async ValueTask RemoveZoomWithWheelAsync(IJSObjectReference jsPanzoomReference,
            ElementReference? elementReference = null)
        {
            await InvokeVoid("removeZoomWithWheel", jsPanzoomReference, elementReference);
        }

        public async ValueTask RemoveWheelListenerAsync(IJSObjectReference jsPanzoomReference,
            ElementReference? elementReference = null)
        {
            await InvokeVoid("removeWheelListener", jsPanzoomReference, elementReference);
        }

        public async ValueTask DestroyPanzoomAsync(IJSObjectReference jsPanzoomReference)
        {
            await InvokeVoid("destroyPanzoom", jsPanzoomReference);
        }

        public async ValueTask PerformForAllAsync(string functionName,
            IEnumerable<IJSObjectReference> jsPanzoomReferences,
            params object[] args)

        {
            await InvokeVoid("performForAllPanzoom", functionName, jsPanzoomReferences, args);
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