namespace BlazorPanZoom.Options
{
    public enum Contain
    {
        Inside,
        Outside
    }

    public interface IZoomOnlyOptions
    {
        public bool DisableZoom { get; }
        public (double x, double y) Focal { get; }
        public double MinScale { get; }
        public double MaxScale { get; }
    }

    public interface IPanOnlyOptions
    {
    }

    public interface IMiscOptions
    {
        public bool Animate { get; }
        public bool Canvas { get; }
        public double Duration { get; }
    }

    public readonly struct ZoomOptions
    {
        //  private static ZoomOnlyOptions _defaultOptions = new();

        //  public static ref readonly ZoomOnlyOptions DefaultOptions => ref _defaultOptions;
    }
}