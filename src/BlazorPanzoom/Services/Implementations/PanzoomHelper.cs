using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorPanzoom
{
    public class PanzoomHelper : IPanzoomHelper
    {
        private readonly IJSBlazorPanzoomInterop _jsPanzoomInterop;

        public PanzoomHelper(IJSBlazorPanzoomInterop jsPanzoomInterop)
        {
            _jsPanzoomInterop = jsPanzoomInterop;
        }

        public async ValueTask SetTransformAsync(PanzoomInterop panzoom,
            EventCallback<SetTransformEventArgs> onSetTransform)
        {
            var dotNetRef = DotNetObjectReference.Create<IPanzoom>(panzoom);
            await _jsPanzoomInterop.RegisterSetTransformAsync(dotNetRef, panzoom.JSPanzoomReference);
            panzoom.OnSetTransform += args => onSetTransform.InvokeAsync(args);
            panzoom.OnDispose += dotNetRef.Dispose;
        }

        public async ValueTask RegisterZoomWithWheelAsync(PanzoomInterop panzoom,
            ElementReference? elementReference = null)
        {
            await _jsPanzoomInterop.RegisterZoomWithWheelAsync(panzoom.JSPanzoomReference, elementReference);

            async void RemoveTask() =>
                await _jsPanzoomInterop.RemoveZoomWithWheelAsync(panzoom.JSPanzoomReference, elementReference);

            panzoom.OnDispose += RemoveTask;
        }

        public async ValueTask RegisterWheelListenerAsync(PanzoomInterop panzoom,
            EventCallback<CustomWheelEventArgs> onWheel,
            ElementReference? elementReference = null)
        {
            var dotNetRef = DotNetObjectReference.Create(panzoom);
            await _jsPanzoomInterop.RegisterWheelListenerAsync(dotNetRef, panzoom.JSPanzoomReference, elementReference);

            async void RemoveTask()
            {
                await _jsPanzoomInterop.RemoveWheelListenerAsync(panzoom.JSPanzoomReference, elementReference);
                dotNetRef.Dispose();
            }

            panzoom.OnCustomWheel += args => onWheel.InvokeAsync(args);
            panzoom.OnDispose += RemoveTask;
        }

        public async ValueTask RegisterWheelListenerAsync(PanzoomInterop panzoom, object receiver,
            Func<CustomWheelEventArgs, Task> onWheel, ElementReference? elementReference = null)
        {
            await RegisterWheelListenerAsync(panzoom, EventCallback.Factory.Create(receiver, onWheel),
                elementReference);
        }

        public async ValueTask<PanzoomInterop> CreateForElementReferenceAsync(ElementReference elementReference,
            PanzoomOptions? panzoomOptions = default)
        {
            var panzoomRef = await _jsPanzoomInterop.CreatePanzoomAsync(elementReference, panzoomOptions);
            var panzoom = Create(panzoomRef);

            return panzoom;
        }

        public async ValueTask<PanzoomInterop[]> CreateForSelectorAsync(string selector,
            PanzoomOptions? panzoomOptions = default)
        {
            var jsPanzoomReferences = await _jsPanzoomInterop.CreatePanzoomAsync(selector, panzoomOptions);
            var referencesLength = jsPanzoomReferences.Length;
            var panzoomControls = new PanzoomInterop[referencesLength];

            for (var i = 0; i < referencesLength; i++)
            {
                var jsPanzoomRef = jsPanzoomReferences[i];
                panzoomControls[i] = Create(jsPanzoomRef);
            }

            return panzoomControls;
        }

        public async ValueTask ResetAllForAsync(IEnumerable<PanzoomInterop> panzoomInterops)
        {
            var references = panzoomInterops.Select(p => p.JSPanzoomReference).ToArray();
            await _jsPanzoomInterop.PerformForAllAsync("reset", references);
        }

        private PanzoomInterop Create(IJSObjectReference jsPanzoomReference)
        {
            async void DisposeTask() => await _jsPanzoomInterop.DestroyPanzoomAsync(jsPanzoomReference);
            var panzoom = new PanzoomInterop(jsPanzoomReference);
            panzoom.OnDispose += DisposeTask;
            return panzoom;
        }
    }
}