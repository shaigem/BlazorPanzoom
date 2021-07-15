using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorPanzoom
{
    internal static class PanzoomInterop
    {
        private const string InteropIdentifier = "blazorPanzoom";

        public static async ValueTask<IJSObjectReference>
            CreatePanzoomAsync(this IJSRuntime jsRuntime, ElementReference element, PanzoomOptions options)
        {
            return await
                jsRuntime.Invoke<IJSObjectReference>("createPanzoomForReference", element, options);
        }

        public static async ValueTask RegisterDefaultWheelZoom(this IJSRuntime jsRuntime,
            ElementReference elementReference,
            IJSObjectReference jsPanzoomReference)
        {
            await jsRuntime.InvokeVoid("registerDefaultWheelZoom", elementReference, jsPanzoomReference);
        }

        public static async ValueTask RegisterWheelListenerAsync(this IJSRuntime jsRuntime,
            DotNetObjectReference<Panzoom> dotNetObjectReference,
            ElementReference elementReference)
        {
            await jsRuntime.InvokeVoid("registerWheelListener", dotNetObjectReference, elementReference);
        }

        public static async ValueTask RemoveZoomWithWheelListenerAsync(this IJSRuntime jsRuntime,
            ElementReference elementReference,
            IJSObjectReference jsPanzoomReference)
        {
            await jsRuntime.InvokeVoid("removeZoomWithWheelListener", elementReference, jsPanzoomReference);
        }

        public static async ValueTask RemoveWheelListenerAsync(this IJSRuntime jsRuntime,
            ElementReference elementReference)
        {
            await jsRuntime.InvokeVoid("removeWheelListener", elementReference);
        }

        private static async ValueTask
            InvokeVoid(this IJSRuntime jsRuntime, string functionName, params object[] args)
        {
            await jsRuntime.InvokeVoidAsync($"{InteropIdentifier}.{functionName}", args);
        }

        private static async ValueTask<T> Invoke<T>(this IJSRuntime jsRuntime, string functionName,
            params object[] args)
        {
            return await jsRuntime.InvokeAsync<T>($"{InteropIdentifier}.{functionName}", args);
        }
    }
}