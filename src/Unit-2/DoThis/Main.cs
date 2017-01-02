using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Akka.Actor;
using Akka.Util.Internal;
using ChartApp.Actors;
using ChartApp.Actors.ButtonToggle;
using ChartApp.Actors.ButtonToggle.Messages;
using ChartApp.Actors.Messages;
using ChartApp.Actors.PerformanceCounterCoordinator;

namespace ChartApp
{
    public partial class Main : Form
    {
        private IActorRef coordinatorActor;

        private Dictionary<CounterType, IActorRef> toggleActors = new Dictionary<CounterType, IActorRef>();

        private IActorRef chartActor;
        private readonly AtomicCounter seriesCounter = new AtomicCounter(1);

        public Main()
        {
            this.InitializeComponent();
        }

        #region Initialization


        private void Main_Load(object sender, EventArgs e)
        {
            this.chartActor = Program.ChartActors.ActorOf(Props.Create(() =>
                new ChartingActor(this.sysChart, this.PauseBtn)), "charting");
            this.chartActor.Tell(new InitializeChart(null)); //no initial series

            this.coordinatorActor = Program.ChartActors.ActorOf(Props.Create(() =>
                    new PerformanceCounterCoordinatorActor(this.chartActor)), "counters");

            // CPU button toggle actor
            this.toggleActors[CounterType.Cpu] = Program.ChartActors.ActorOf(
                Props.Create(() => new ButtonToggleActor(this.coordinatorActor, this.CpuBtn, CounterType.Cpu, false))
                    .WithDispatcher("akka.actor.synchronized-dispatcher"));

            // MEMORY button toggle actor
            this.toggleActors[CounterType.Memory] = Program.ChartActors.ActorOf(
                Props.Create(() => new ButtonToggleActor(this.coordinatorActor, this.MemoryBtn, CounterType.Memory, false))
                    .WithDispatcher("akka.actor.synchronized-dispatcher"));

            // DISK button toggle actor
            this.toggleActors[CounterType.Disk] = Program.ChartActors.ActorOf(
                Props.Create(() => new ButtonToggleActor(this.coordinatorActor, this.DiskBtn, CounterType.Disk, false))
                    .WithDispatcher("akka.actor.synchronized-dispatcher"));

            // Set the CPU toggle to ON so we start getting some data
            this.toggleActors[CounterType.Cpu].Tell(new Toggle());
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            //shut down the charting actor
            this.chartActor.Tell(PoisonPill.Instance);

            //shut down the ActorSystem
            Program.ChartActors.Terminate();
        }

        #endregion

        private void CpuBtn_Click(object sender, EventArgs e)
        {
            this.toggleActors[CounterType.Cpu].Tell(new Toggle());
        }

        private void MemoryBtn_Click(object sender, EventArgs e)
        {
            this.toggleActors[CounterType.Memory].Tell(new Toggle());
        }

        private void DiskBtn_Click(object sender, EventArgs e)
        {
            this.toggleActors[CounterType.Disk].Tell(new Toggle());
        }

        private void PauseBtn_Click(object sender, EventArgs e)
        {
            this.chartActor.Tell(new TogglePause());
        }
    }
}
