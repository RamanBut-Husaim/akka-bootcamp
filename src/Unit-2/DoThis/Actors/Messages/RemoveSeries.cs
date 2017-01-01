namespace ChartApp.Actors.Messages
{
    public sealed class RemoveSeries
    {
        public RemoveSeries(string seriesName)
        {
            this.SeriesName = seriesName;
        }

        public string SeriesName { get; }
    }
}
