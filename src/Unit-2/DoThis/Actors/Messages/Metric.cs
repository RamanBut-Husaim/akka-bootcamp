namespace ChartApp.Actors.Messages
{
    public sealed class Metric
    {
        public Metric(string series, float counterValue)
        {
            this.CounterValue = counterValue;
            this.Series = series;
        }

        public string Series { get; }

        public float CounterValue { get; }
    }
}
