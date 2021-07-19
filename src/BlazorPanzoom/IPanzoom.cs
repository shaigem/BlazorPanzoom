using System.Threading.Tasks;

namespace BlazorPanzoom
{
    public interface IPanzoom
    {
        ValueTask PanAsync(double x, double y, IPanOnlyOptions? overridenOptions = default);
        ValueTask ZoomInAsync(IZoomOnlyOptions? options = default);
        ValueTask ZoomOutAsync(IZoomOnlyOptions? options = default);
        ValueTask ZoomAsync(double toScale, IZoomOnlyOptions? options = default);

        ValueTask ZoomToPointAsync(double toScale, double clientX, double clientY,
            IZoomOnlyOptions? overridenZoomOptions = default);

        ValueTask ZoomWithWheelAsync(CustomWheelEventArgs args, IZoomOnlyOptions? overridenOptions = default);
        ValueTask ResetAsync(PanzoomOptions? options = default);
        ValueTask SetOptionsAsync(PanzoomOptions options);
        ValueTask<PanzoomOptions> GetOptionsAsync();
        ValueTask<double> GetScaleAsync();
        ValueTask<ReadOnlyFocalPoint> GetPanAsync();
        ValueTask ResetStyleAsync();
        ValueTask SetStyleAsync(string name, string value);
        ValueTask DestroyAsync();
    }
}