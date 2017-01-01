using System.Windows.Forms.DataVisualization.Charting;

namespace ChartApp.Actors.Messages
{
    public sealed class AddSeries
    {
        public AddSeries(Series series)
        {
            this.Series = series;
        }

        public Series Series { get; }
    }
}
