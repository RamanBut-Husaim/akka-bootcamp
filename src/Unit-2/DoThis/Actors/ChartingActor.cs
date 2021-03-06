﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

using Akka.Actor;

using ChartApp.Actors.Messages;

namespace ChartApp.Actors
{
    public sealed class ChartingActor : ReceiveActor, IWithUnboundedStash
    {
        /// <summary>
        /// Maximum number of points we will allow in a series
        /// </summary>
        public const int MaxPoints = 250;

        /// <summary>
        /// Incrementing counter we use to plot along the X-axis
        /// </summary>
        private int xPosCounter;

        private readonly Chart chart;
        private readonly Button pauseButton;
        private Dictionary<string, Series> seriesIndex;

        public ChartingActor(Chart chart, Button pauseButton) :
            this(chart, new Dictionary<string, Series>(), pauseButton)
        {
        }

        public ChartingActor(Chart chart, Dictionary<string, Series> seriesIndex,
            Button pauseButton)
        {
            this.chart = chart;
            this.seriesIndex = seriesIndex;
            this.pauseButton = pauseButton;
            this.Charting();
        }

        public IStash Stash { get; set; }

        #region Individual Message Type Handlers

        private void Charting()
        {
            this.Receive<InitializeChart>(ic => this.HandleInitialize(ic));
            this.Receive<AddSeries>(addSeries => this.HandleAddSeries(addSeries));
            this.Receive<RemoveSeries>(removeSeries => this.HandleRemoveSeries(removeSeries));
            this.Receive<Metric>(metric => this.HandleMetrics(metric));

            //new receive handler for the TogglePause message type
            this.Receive<TogglePause>(pause =>
            {
                this.SetPauseButtonText(true);
                this.BecomeStacked(Paused);
            });
        }

        private void Paused()
        {
            this.Receive<AddSeries>(addSeries => this.Stash.Stash());
            this.Receive<RemoveSeries>(removeSeries => this.Stash.Stash());
            this.Receive<Metric>(metric => this.HandleMetricsPaused(metric));
            this.Receive<TogglePause>(pause =>
            {
                this.SetPauseButtonText(false);
                this.UnbecomeStacked();

                // ChartingActor is leaving the Paused state, put messages back
                // into mailbox for processing under new behavior
                this.Stash.UnstashAll();
            });
        }

        private void SetPauseButtonText(bool paused)
        {
            this.pauseButton.Text = $"{(!paused ? "PAUSE" : "RESUME")}";
        }

        private void HandleMetricsPaused(Metric metric)
        {
            if (!string.IsNullOrEmpty(metric.Series) && this.seriesIndex.ContainsKey(metric.Series))
            {
                var series = this.seriesIndex[metric.Series];
                // set the Y value to zero when we're paused
                series.Points.AddXY(this.xPosCounter++, 0.0d);
                while (series.Points.Count > MaxPoints)
                    series.Points.RemoveAt(0);
                this.SetChartBoundaries();
            }
        }

        private void HandleInitialize(InitializeChart ic)
        {
            if (ic.InitialSeries != null)
            {
                // swap the two series out
                this.seriesIndex = ic.InitialSeries;
            }

            // delete any existing series
            this.chart.Series.Clear();

            // set the axes up
            var area = this.chart.ChartAreas[0];
            area.AxisX.IntervalType = DateTimeIntervalType.Number;
            area.AxisY.IntervalType = DateTimeIntervalType.Number;

            this.SetChartBoundaries();

            // attempt to render the initial chart
            if (this.seriesIndex.Any())
            {
                foreach (var series in this.seriesIndex)
                {
                    // force both the chart and the internal index to use the same names
                    series.Value.Name = series.Key;
                    this.chart.Series.Add(series.Value);
                }
            }

            this.SetChartBoundaries();
        }

        private void HandleAddSeries(AddSeries series)
        {
            if (!string.IsNullOrEmpty(series.Series.Name) && !this.seriesIndex.ContainsKey(series.Series.Name))
            {
                this.seriesIndex.Add(series.Series.Name, series.Series);
                this.chart.Series.Add(series.Series);
                this.SetChartBoundaries();
            }
        }

        private void HandleRemoveSeries(RemoveSeries series)
        {
            if (!string.IsNullOrEmpty(series.SeriesName) && this.seriesIndex.ContainsKey(series.SeriesName))
            {
                var seriesToRemove = this.seriesIndex[series.SeriesName];
                this.seriesIndex.Remove(series.SeriesName);
                this.chart.Series.Remove(seriesToRemove);
                this.SetChartBoundaries();
            }
        }

        private void HandleMetrics(Metric metric)
        {
            if (!string.IsNullOrEmpty(metric.Series) && this.seriesIndex.ContainsKey(metric.Series))
            {
                var series = this.seriesIndex[metric.Series];
                series.Points.AddXY(this.xPosCounter++, metric.CounterValue);
                while (series.Points.Count > MaxPoints) series.Points.RemoveAt(0);
                this.SetChartBoundaries();
            }
        }

        private void SetChartBoundaries()
        {
            var allPoints = this.seriesIndex.Values.SelectMany(series => series.Points).ToList();
            var yValues = allPoints.SelectMany(point => point.YValues).ToList();
            double maxAxisX = this.xPosCounter;
            double minAxisX = this.xPosCounter - MaxPoints;
            var maxAxisY = yValues.Count > 0 ? Math.Ceiling(yValues.Max()) : 1.0d;
            var minAxisY = yValues.Count > 0 ? Math.Floor(yValues.Min()) : 0.0d;

            if (allPoints.Count > 2)
            {
                var area = this.chart.ChartAreas[0];
                area.AxisX.Minimum = minAxisX;
                area.AxisX.Maximum = maxAxisX;
                area.AxisY.Minimum = minAxisY;
                area.AxisY.Maximum = maxAxisY;
            }
        }

        #endregion
    }
}
