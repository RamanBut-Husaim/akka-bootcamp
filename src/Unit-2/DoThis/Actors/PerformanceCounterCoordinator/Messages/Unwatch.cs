using ChartApp.Actors.Messages;

namespace ChartApp.Actors.PerformanceCounterCoordinator.Messages
{
    public sealed class Unwatch
    {
        public Unwatch(CounterType counter)
        {
            this.Counter = counter;
        }

        public CounterType Counter { get; }
    }
}
