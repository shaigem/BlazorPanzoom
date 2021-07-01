using System;
using System.Threading.Tasks;

namespace BlazorPanzoom
{
    public interface IPanzoom : IAsyncDisposable
    {
        ValueTask ZoomInAsync(); 
        ValueTask PanAsync(double toX, double toY);
        ValueTask ZoomAsync(double toScale);
        ValueTask ZoomToPointAsync(double toScale, double toX, double toY);
        ValueTask<double> GetScaleAsync();
        ValueTask ResetAsync();
        ValueTask DestroyAsync();
    }
}