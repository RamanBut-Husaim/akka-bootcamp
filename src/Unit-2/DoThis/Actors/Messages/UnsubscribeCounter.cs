using Akka.Actor;

namespace ChartApp.Actors.Messages
{
    public sealed class UnsubscribeCounter
    {
        public UnsubscribeCounter(CounterType counter, IActorRef subscriber)
        {
            this.Subscriber = subscriber;
            this.Counter = counter;
        }

        public CounterType Counter { get; }

        public IActorRef Subscriber { get; }
    }
}
