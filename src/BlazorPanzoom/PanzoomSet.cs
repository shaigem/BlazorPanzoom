using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorPanzoom
{
    public class PanzoomSet : IAsyncDisposable, IEnumerable<PanzoomInterop>
    {
        private readonly HashSet<PanzoomInterop> _panzoomSet;

        public PanzoomSet(HashSet<PanzoomInterop> panzoomSet)
        {
            _panzoomSet = panzoomSet;
        }

        public PanzoomSet()
        {
            _panzoomSet = new HashSet<PanzoomInterop>();
        }

        public bool Add(PanzoomInterop panzoom)
        {
            return _panzoomSet.Add(panzoom);
        }

        public async ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);
            foreach (var panzoom in _panzoomSet)
            {
                await panzoom.DisposeAsync();
            }
        }

        public IEnumerator<PanzoomInterop> GetEnumerator()
        {
            return _panzoomSet.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}