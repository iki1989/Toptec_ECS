using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ECS.Viewer.Views.Controls
{
    public class SimpleChart : Chart
    {

        private ObservableCollection<(DateTime x, double y)> m_DataList;
        public ObservableCollection<(DateTime x, double y)> DataList
        {
            get => this.m_DataList;
            set
            {
                value.CollectionChanged += OnDataListChanged;
                this.m_DataList = value;
                this.Init();
            }
        }

        private void OnDataListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    this.Series[0].Points.Clear();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    this.Series[0].Points.RemoveAt(e.OldStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Replace:
                    (var dt, var val) = ((DateTime, double))e.NewItems[0];
                    var pt = new DataPoint() { AxisLabel = dt.ToString("MM/dd") };
                    pt.SetValueY(val);
                    if (e.Action == NotifyCollectionChangedAction.Add)
                        this.Series[0].Points.Add(pt);
                    else
                        this.Series[0].Points[e.NewStartingIndex] = pt;
                    break;
                case NotifyCollectionChangedAction.Move: break;
                default: break;
            }
        }
        private void Init()
        {
            this.Series[0].Points.Clear();
            foreach ((var dt, var val) in this.DataList)
            {
                this.Series[0].Points.AddXY(dt.ToString("MM/dd"), val);
            }
        }

        public SimpleChart() : base()
        {
            Title title = new Title();
            title.ForeColor = Color.White;
            this.Titles.Add(title);

            ChartArea chartArea = new ChartArea();
            chartArea.BackColor = Color.FromArgb(0, 0, 0, 0);
            chartArea.AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.TitleForeColor = Color.LightGray;
            chartArea.AxisX.LineColor = Color.LightGray;
            chartArea.AxisX.MajorGrid.LineColor = Color.White;
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.LabelStyle.ForeColor = Color.LightGray;
            chartArea.AxisX.MajorTickMark.LineColor = Color.LightGray;
            chartArea.AxisY.LineColor = Color.LightGray;
            chartArea.AxisY.TitleForeColor = Color.LightGray;
            //chartArea.AxisY.Maximum = 1000;
            chartArea.AxisY.TitleForeColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.White;
            chartArea.AxisY.LabelStyle.ForeColor = Color.LightGray;
            chartArea.AxisY.MajorTickMark.LineColor = Color.LightGray;
            this.ChartAreas.Add(chartArea);

            Series series = new Series();
            series.BackGradientStyle = GradientStyle.TopBottom;
            series.ChartType = SeriesChartType.Column;
            series.XValueType = ChartValueType.String;
            series.BackSecondaryColor = Color.Aquamarine;
            series.LabelForeColor = Color.Black;
            series.Color = Color.SteelBlue;
            series.IsValueShownAsLabel = true;
            this.Series.Add(series);

            Legend legend = new Legend();
            legend.Enabled = false;
            this.Legends.Add(legend);

        }

    }
}
