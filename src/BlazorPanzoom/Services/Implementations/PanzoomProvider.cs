using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorPanzoom
{
    public class PanzoomProvider : IPanzoomProvider
    {
        private readonly IJSBlazorPanzoomInterop _jsPanzoomInterop;

        public PanzoomProvider(IJSBlazorPanzoomInterop jsPanzoomInterop)
        {
            _jsPanzoomInterop = jsPanzoomInterop;
        }

        public async ValueTask RegisterZooming(ElementReference elementReference,
            PanzoomInterop panzoomInterop, WheelHandler wheelHandler,
            EventCallback<PanzoomWheelEventArgs> handler)
        {
            switch (wheelHandler)
            {
                case WheelHandler.Custom when handler.HasDelegate:
                    var dotNetListenerReference = DotNetObjectReference.Create<IPanzoomWheelListener>(panzoomInterop);
                    panzoomInterop.OnWheel = handler;
                    await _jsPanzoomInterop.RegisterWheelListenerAsync(dotNetListenerReference, elementReference,
                        panzoomInterop.JSPanzoomReference);
                    break;
                case WheelHandler.ZoomOnScroll:
                    await _jsPanzoomInterop.RegisterDefaultWheelZoom(elementReference,
                        panzoomInterop.JSPanzoomReference);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(wheelHandler));
            }

            ValueTask RemoveListener() =>
                _jsPanzoomInterop.RemoveZoomWithWheelListenerAsync(elementReference, panzoomInterop.JSPanzoomReference);

            panzoomInterop.RemoveListenersTask = RemoveListener;
        }

        public async ValueTask<IPanzoom> CreateForElementReference(ElementReference elementReference,
            PanzoomOptions? panzoomOptions = default)
        {
            var panzoomRef = await _jsPanzoomInterop.CreatePanzoomAsync(elementReference, panzoomOptions);
            var panzoom = new PanzoomInterop(panzoomRef);
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
                panzoomControls[i] = new PanzoomInterop(jsPanzoomRef);
            }

            return panzoomControls;
        }

        public async ValueTask ResetAllFor(IEnumerable<IPanzoom> panzoomEnumerable)
        {
            await _jsPanzoomInterop.PerformForAll("reset", panzoomEnumerable);
        }
    }
}