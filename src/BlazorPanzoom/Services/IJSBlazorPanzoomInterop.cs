using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorPanzoom
{
    public interface IJSBlazorPanzoomInterop
    {
        public ValueTask<IJSObjectReference>
            CreatePanzoomAsync(ElementReference elementReference, PanzoomOptions? options = null);

        public ValueTask<IJSObjectReference[]> CreatePanzoomAsync(string selector, PanzoomOptions? options = null);

        public ValueTask RegisterSetTransformAsync(DotNetObjectReference<IPanzoom> dotNetObjectReference,
            IJSObjectReference jsPanzoomReference);

        public ValueTask RegisterZoomWithWheelAsync(
            IJSObjectReference jsPanzoomReference, ElementReference? elementReference = null);

        public ValueTask RegisterWheelListenerAsync(
            DotNetObjectReference<PanzoomInterop> dotNetObjectReference,
            IJSObjectReference jsPanzoomReference, ElementReference? elementReference = null);

        public ValueTask RemoveZoomWithWheelAsync(IJSObjectReference jsPanzoomReference,
            ElementReference? elementReference = null);

        public ValueTask RemoveWheelListenerAsync(IJSObjectReference jsPanzoomReference,
            ElementReference? elementReference = null);

        public ValueTask DestroyPanzoomAsync(IJSObjectReference jsPanzoomReference);

        public ValueTask
            PerformForAllAsync(string functionName, IEnumerable<IJSObjectReference> jsPanzoomReferences,
                params object[] args);
    }
}