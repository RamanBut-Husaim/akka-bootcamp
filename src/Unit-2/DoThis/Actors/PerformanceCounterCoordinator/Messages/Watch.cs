using ChartApp.Actors.Messages;

namespace ChartApp.Actors.PerformanceCounterCoordinator.Messages
{
    public sealed class Watch
    {
        public Watch(CounterType counter)
        {
            this.Counter = counter;
        }

        public CounterType Counter { get; }
    }
}
