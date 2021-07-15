using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorPanzoom
{
    public interface IPanzoomWheelListener
    {
        public ValueTask OnCustomWheelEvent(PanzoomWheelEventArgs args);
        public ValueTask RemoveWheelListener();
    }


    public interface IJSPanzoomInterop
    {
        public ValueTask<IJSObjectReference>
            CreatePanzoomAsync(ElementReference elementReference, PanzoomOptions? options = null);

        public ValueTask<IJSObjectReference[]> CreatePanzoomAsync(string selector, PanzoomOptions? options = null);

        public ValueTask RegisterDefaultWheelZoom(ElementReference elementReference,
            IJSObjectReference jsPanzoomReference);

        public ValueTask RegisterWheelListenerAsync(
            DotNetObjectReference<IPanzoomWheelListener> dotNetObjectReference,
            ElementReference elementReference, IJSObjectReference jsPanzoomReference);

        public ValueTask RemoveZoomWithWheelListenerAsync(
            ElementReference elementReference,
            IJSObjectReference jsPanzoomReference);
    }

    public class DefaultPanzoom : IPanzoom, IPanzoomWheelListener, IAsyncDisposable
    {
        private readonly IJSObjectReference _jsPanzoomReference;

        public Func<ValueTask>? RemoveListenersTask { get; set; }

        public EventCallback<PanzoomWheelEventArgs> OnWheel { get; set; } // TODO could probably just use Func

        internal DefaultPanzoom(IJSObjectReference jsPanzoomReference)
        {
            _jsPanzoomReference = jsPanzoomReference;
        }

        public async ValueTask ZoomInAsync()
        {
            await _jsPanzoomReference.InvokeVoidAsync("zoomIn");
        }

        public async ValueTask ZoomOutAsync()
        {
            await _jsPanzoomReference.InvokeVoidAsync("zoomOut");
        }

        public async ValueTask ZoomAsync(double toScale)
        {
            await _jsPanzoomReference.InvokeVoidAsync("zoom", toScale);
        }

        public async ValueTask ZoomToPointAsync(double toScale, double clientX, double clientY,
            IZoomOnlyOptions? overridenZoomOptions)
        {
            await _jsPanzoomReference.InvokeVoidAsync("zoomToPoint", toScale, clientX, clientY, overridenZoomOptions);
        }

        public async ValueTask ResetAsync(PanzoomOptions resetOptions)
        {
            await _jsPanzoomReference.InvokeVoidAsync("reset");
        }

        public async ValueTask ResetAsync()
        {
            await _jsPanzoomReference.InvokeVoidAsync("reset");
        }

        public async ValueTask SetOptionsAsync(PanzoomOptions options)
        {
            await _jsPanzoomReference.InvokeVoidAsync("setOptions", options);
        }

        public async ValueTask<PanzoomOptions> GetOptionsAsync()
        {
            return await _jsPanzoomReference.InvokeAsync<PanzoomOptions>("getOptions");
        }

        public async ValueTask<double> GetScaleAsync()
        {
            return await _jsPanzoomReference.InvokeAsync<double>("getScale");
        }

        public async ValueTask DestroyAsync()
        {
            await _jsPanzoomReference.InvokeVoidAsync("destroy");
        }

        [JSInvokable]
        public async ValueTask OnCustomWheelEvent(PanzoomWheelEventArgs args)
        {
            await OnWheel.InvokeAsync();
        }

        public async ValueTask RemoveWheelListener()
        {
            if (RemoveListenersTask is not null)
            {
                await RemoveListenersTask.Invoke();
            }
        }

        public async ValueTask DisposeAsync()
        {
            await RemoveWheelListener();
            await DestroyAsync();
            await _jsPanzoomReference.DisposeAsync();
        }
    }


    public interface IPanzoomProvider
    {
        public ValueTask<IPanzoom> CreateForElementReference(ElementReference elementReference,
            PanzoomOptions? panzoomOptions = default);

        public ValueTask<IPanzoom[]> CreateForSelector(string selector, PanzoomOptions? panzoomOptions = default);
    }

    public class PanzoomProvider : IPanzoomProvider
    {
        private readonly IJSPanzoomInterop _jsPanzoomInterop;

        public PanzoomProvider(IJSPanzoomInterop jsPanzoomInterop)
        {
            _jsPanzoomInterop = jsPanzoomInterop;
        }

        public async ValueTask<IPanzoom> CreateForElementReference(ElementReference elementReference,
            PanzoomOptions? panzoomOptions = default)
        {
            var panzoomRef = await _jsPanzoomInterop.CreatePanzoomAsync(elementReference, panzoomOptions);
            var panzoom = new DefaultPanzoom(panzoomRef);
            return panzoom;
        }

        public async ValueTask<IPanzoom[]> CreateForSelector(string selector, PanzoomOptions? panzoomOptions = default)
        {
            var jsPanzoomReferences = await _jsPanzoomInterop.CreatePanzoomAsync(selector, panzoomOptions);
            var referencesLength = jsPanzoomReferences.Length;
            var panzoomControls = new IPanzoom[referencesLength];
            for (var i = 0; i < referencesLength; i++)
            {
                var jsPanzoomRef = jsPanzoomReferences[i];
                panzoomControls[i] = new DefaultPanzoom(jsPanzoomRef);
            }

            return panzoomControls;
        }
    }


    public class JSPanzoomInterop : IJSPanzoomInterop
    {
        private const string InteropIdentifier = "blazorPanzoom";

        private readonly IJSRuntime _jsRuntime;

        public JSPanzoomInterop(IJSRuntime jsRuntime)
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

        public ValueTask RegisterDefaultWheelZoom(ElementReference elementReference,
            IJSObjectReference jsPanzoomReference)
        {
            throw new NotImplementedException();
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