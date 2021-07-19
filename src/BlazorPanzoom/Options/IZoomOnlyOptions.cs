namespace BlazorPanzoom
{
    public interface IZoomOnlyOptions
    {
        public bool GetDisableZoomOrDefault(bool disableZoom = default);
        public ReadOnlyFocalPoint GetFocalOrDefault(ReadOnlyFocalPoint focal = default);
        public double GetMaxScaleOrDefault(double maxScale = default);
        public double GetMinScaleOrDefault(double minScale = default);
        public double GetStepOrDefault(double step = default);
    }
}