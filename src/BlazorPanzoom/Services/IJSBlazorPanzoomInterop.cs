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

        public ValueTask
            PerformForAll(string functionName, IEnumerable<IPanzoom> panzoomEnumerable, params object[] args);
    }
}