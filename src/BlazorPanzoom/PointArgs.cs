namespace BlazorPanzoom
{
    internal readonly struct PointArgs
    {
        internal PointArgs(double clientX = 0.0, double clientY = 0.0)
        {
            ClientX = clientX;
            ClientY = clientY;
        }

        public double ClientX { get; }
        public double ClientY { get; }
    }
}