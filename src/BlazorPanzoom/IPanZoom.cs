using System;
using System.Threading.Tasks;

namespace BlazorPanZoom
{
    public interface IPanZoom : IAsyncDisposable
    {
        ValueTask ZoomInAsync(); 
        ValueTask PanAsync(double toX, double toY);
        ValueTask ZoomAsync(double toScale);
        ValueTask ZoomToPointAsync(double toScale, double toX, double toY);
        ValueTask ResetAsync();
        ValueTask DestroyAsync();
    }
}