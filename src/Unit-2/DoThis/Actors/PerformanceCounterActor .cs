using System;
using System.Collections.Generic;
using System.Diagnostics;

using Akka.Actor;

using ChartApp.Actors.Messages;

namespace ChartApp.Actors
{
    public sealed class PerformanceCounterActor : UntypedActor
    {
        private readonly string seriesName;
        private readonly Func<PerformanceCounter> performanceCounterGenerator;
        private PerformanceCounter counter;

        private readonly HashSet<IActorRef> subscriptions;
        private readonly ICancelable cancelPublishing;

        public PerformanceCounterActor(
            string seriesName,
            Func<PerformanceCounter> performanceCounterGenerator)
        {
            this.seriesName = seriesName;
            this.performanceCounterGenerator = performanceCounterGenerator;

            this.subscriptions = new HashSet<IActorRef>();
            this.cancelPublishing = new Cancelable(Context.System.Scheduler);
        }

        protected override void PreStart()
        {
            //create a new instance of the performance counter
            this.counter = this.performanceCounterGenerator();
            Context.System.Scheduler.ScheduleTellRepeatedly(
                TimeSpan.FromMilliseconds(250),
                TimeSpan.FromMilliseconds(250),
                this.Self,
                new GatherMetrics(),
                this.Self,
                this.cancelPublishing);
        }

        protected override void PostStop()
        {
            try
            {
                //terminate the scheduled task
                this.cancelPublishing.Cancel(false);
                this.counter.Dispose();
            }
            catch
            {
                //don't care about additional "ObjectDisposed" exceptions
            }
            finally
            {
                base.PostStop();
            }
        }

        protected override void OnReceive(object message)
        {
            if (message is GatherMetrics)
            {
                //publish latest counter value to all subscribers
                var metric = new Metric(this.seriesName, this.counter.NextValue());
                foreach (var sub in this.subscriptions)
                    sub.Tell(metric);
            }
            else if (message is SubscribeCounter)
            {
                // add a subscription for this counter
                // (it's parent's job to filter by counter types)
                var sc = message as SubscribeCounter;
                this.subscriptions.Add(sc.Subscriber);
            }
            else if (message is UnsubscribeCounter)
            {
                // remove a subscription from this counter
                var uc = message as UnsubscribeCounter;
                this.subscriptions.Remove(uc.Subscriber);
            }
        }
    }
}
