using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Akka.Actor;
using Akka.Util.Internal;
using ChartApp.Actors;
using ChartApp.Actors.Messages;

namespace ChartApp
{
    public partial class Main : Form
    {
        private IActorRef chartActor;
        private readonly AtomicCounter seriesCounter = new AtomicCounter(1);

        public Main()
        {
            this.InitializeComponent();
        }

        #region Initialization


        private void Main_Load(object sender, EventArgs e)
        {
            this.chartActor = Program.ChartActors.ActorOf(Props.Create(() => new ChartingActor(this.sysChart)), "charting");
            var series = ChartDataHelper.RandomSeries("FakeSeries" + this.seriesCounter.GetAndIncrement());

            this.chartActor.Tell(new InitializeChart(new Dictionary<string, Series>()
            {
                {series.Name, series}
            }));
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            //shut down the charting actor
            this.chartActor.Tell(PoisonPill.Instance);

            //shut down the ActorSystem
            Program.ChartActors.Terminate();
        }

        #endregion

        private void btnAddSeries_Click(object sender, EventArgs e)
        {
            var series = ChartDataHelper.RandomSeries("FakeSeries" + this.seriesCounter.GetAndIncrement());
            this.chartActor.Tell(new AddSeries(series));
        }
    }
}
