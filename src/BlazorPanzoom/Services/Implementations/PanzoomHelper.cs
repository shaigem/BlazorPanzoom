using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace BlazorPanzoom
{
    public interface IPanzoomWheelListener
    {
        public EventCallback<WheelEventArgs> OnWheel { get; set; }
        public Func<ValueTask>? OnRemoveListener { get; set; }
        public ValueTask OnCustomWheelEvent(WheelEventArgs args);
    }

    public class PanzoomHelper : IPanzoomHelper
    {
        private readonly IJSBlazorPanzoomInterop _jsPanzoomInterop;

        public PanzoomHelper(IJSBlazorPanzoomInterop jsPanzoomInterop)
        {
            _jsPanzoomInterop = jsPanzoomInterop;
        }

        public async ValueTask RegisterZoomWithWheelAsync(PanzoomInterop panzoom,
            ElementReference? elementReference = null)
        {
            await _jsPanzoomInterop.RegisterZoomWithWheelAsync(panzoom.JSPanzoomReference, elementReference);

            ValueTask RemoveTask() =>
                _jsPanzoomInterop.RemoveZoomWithWheelAsync(panzoom.JSPanzoomReference, elementReference);

            panzoom.OnRemoveListener = RemoveTask;
        }

        public async ValueTask RegisterWheelListenerAsync(PanzoomInterop panzoom, EventCallback<WheelEventArgs> onWheel,
            ElementReference? elementReference = null)
        {
            var dotNetRef = DotNetObjectReference.Create<IPanzoomWheelListener>(panzoom);
            await _jsPanzoomInterop.RegisterWheelListenerAsync(dotNetRef, panzoom.JSPanzoomReference, elementReference);

            ValueTask RemoveTask() =>
                _jsPanzoomInterop.RemoveWheelListenerAsync(panzoom.JSPanzoomReference, elementReference);

            panzoom.OnWheel = onWheel;
            panzoom.OnRemoveListener = RemoveTask;
        }

        public async ValueTask RegisterWheelListenerAsync(PanzoomInterop panzoom, object receiver,
            Func<WheelEventArgs, Task> onWheel, ElementReference? elementReference = null)
        {
            await RegisterWheelListenerAsync(panzoom, EventCallback.Factory.Create(receiver, onWheel),
                elementReference);
        }

        public async ValueTask<PanzoomInterop> CreateForElementReferenceAsync(ElementReference elementReference,
            PanzoomOptions? panzoomOptions = default)
        {
            var panzoomRef = await _jsPanzoomInterop.CreatePanzoomAsync(elementReference, panzoomOptions);
            return Create(panzoomRef);
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
            ValueTask DisposeTask() => _jsPanzoomInterop.DestroyPanzoomAsync(jsPanzoomReference);
            return new PanzoomInterop(jsPanzoomReference)
            {
                OnDispose = DisposeTask
            };
        }
    }
}