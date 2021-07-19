namespace BlazorPanzoom
{
    public interface IPanOnlyOptions
    {
        public Contain GetContainOrDefault(Contain contain = Contain.None);
        public Cursor GetCursorOrDefault(Cursor cursor = Cursor.Move);
        public bool GetDisablePanOrDefault(bool disablePan = default);
        public bool GetDisableXAxisOrDefault(bool disableXAxis = default);
        public bool GetDisableYAxisOrDefault(bool disableYAxis = default);
        public bool GetPanOnlyWhenZoomedOrDefault(bool panOnlyWhenZoomed = default);
        public bool GetRelativeOrDefault(bool relative = default);
    }
}