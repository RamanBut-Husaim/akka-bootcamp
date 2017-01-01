using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

using Akka.Actor;

using ChartApp.Actors.Messages;
using ChartApp.Actors.PerformanceCounterCoordinator.Messages;

namespace ChartApp.Actors.PerformanceCounterCoordinator
{
    public sealed class PerformanceCounterCoordinatorActor : ReceiveActor
    {
        private static readonly Dictionary<CounterType, Func<PerformanceCounter>>
            CounterGenerators = new Dictionary<CounterType, Func<PerformanceCounter>>
            {
                {
                    CounterType.Cpu, () => new PerformanceCounter("Processor",
                        "% Processor Time", "_Total", true)
                },
                {
                    CounterType.Memory, () => new PerformanceCounter("Memory",
                        "% Committed Bytes In Use", true)
                },
                {
                    CounterType.Disk, () => new PerformanceCounter("LogicalDisk",
                        "% Disk Time", "_Total", true)
                },
            };

        private static readonly Dictionary<CounterType, Func<Series>> CounterSeries =
            new Dictionary<CounterType, Func<Series>>()
            {
                {
                    CounterType.Cpu, () =>
                        new Series(CounterType.Cpu.ToString())
                        {
                            ChartType = SeriesChartType.SplineArea,
                            Color = Color.DarkGreen
                        }
                },
                {
                    CounterType.Memory, () =>
                        new Series(CounterType.Memory.ToString())
                        {
                            ChartType = SeriesChartType.FastLine,
                            Color = Color.MediumBlue
                        }
                },
                {
                    CounterType.Disk, () =>
                        new Series(CounterType.Disk.ToString())
                        {
                            ChartType = SeriesChartType.SplineArea,
                            Color = Color.DarkRed
                        }
                },
            };

        private readonly Dictionary<CounterType, IActorRef> counterActors;

        private readonly IActorRef chartingActor;

        public PerformanceCounterCoordinatorActor(IActorRef chartingActor)
            : this(chartingActor, new Dictionary<CounterType, IActorRef>())
        {
        }

        public PerformanceCounterCoordinatorActor(
            IActorRef chartingActor,
            Dictionary<CounterType, IActorRef> counterActors)
        {
            this.chartingActor = chartingActor;
            this.counterActors = counterActors;

            this.Receive<Watch>(watch =>
            {
                if (!this.counterActors.ContainsKey(watch.Counter))
                {
                    // create a child actor to monitor this counter if
                    // one doesn't exist already
                    var performanceCounterProps = Props.Create(() =>
                        new PerformanceCounterActor(
                            watch.Counter.ToString(),
                            CounterGenerators[watch.Counter]));
                    var counterActor = Context.ActorOf(performanceCounterProps);

                    // add this counter actor to our index
                    this.counterActors[watch.Counter] = counterActor;
                }

                // register this series with the ChartingActor
                this.chartingActor.Tell(new AddSeries(CounterSeries[watch.Counter]()));

                // tell the counter actor to begin publishing its
                // statistics to the _chartingActor
                this.counterActors[watch.Counter].Tell(new SubscribeCounter(watch.Counter, this.chartingActor));
            });

            this.Receive<Unwatch>(unwatch =>
            {
                if (!this.counterActors.ContainsKey(unwatch.Counter))
                {
                    return; // noop
                }

                // unsubscribe the ChartingActor from receiving any more updates
                this.counterActors[unwatch.Counter].Tell(new UnsubscribeCounter(unwatch.Counter, this.chartingActor));

                // remove this series from the ChartingActor
                this.chartingActor.Tell(new RemoveSeries(unwatch.Counter.ToString()));
            });
        }
    }
}
