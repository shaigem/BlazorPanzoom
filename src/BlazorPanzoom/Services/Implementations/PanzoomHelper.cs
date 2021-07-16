using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace BlazorPanzoom
{
    public class PanzoomHelper : IPanzoomHelper
    {
        private readonly IJSBlazorPanzoomInterop _jsPanzoomInterop;

        public PanzoomHelper(IJSBlazorPanzoomInterop jsPanzoomInterop)
        {
            _jsPanzoomInterop = jsPanzoomInterop;
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