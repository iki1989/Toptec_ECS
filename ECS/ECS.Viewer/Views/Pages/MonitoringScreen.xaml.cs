using ECS.Core.ViewModels.Viewer;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Media;

namespace ECS.Viewer.Views.Pages
{
    public partial class MonitoringScreen : Page
    {
        private CountingViewModel ViewModel => (CountingViewModel)DataContext;//public bool m_IsClosed { get; private set; }

        private Chart todayOrderChartView = new Chart();
        private Chart nonOutChartView = new Chart();
        private Chart todayOutChartView = new Chart();
        private Chart totalOutChartView = new Chart();

        private System.Drawing.Color cjYellow;
        private System.Drawing.Color cjBlue;

        public MonitoringScreen()
        {
            InitializeComponent();


            var mCjRed = (this.FindResource("Brush.CjYellow") as SolidColorBrush).Color;
            var mCjBlue = (this.FindResource("Brush.CjBlue") as SolidColorBrush).Color;
            this.cjYellow = System.Drawing.Color.FromArgb(mCjRed.A, mCjRed.R, mCjRed.G, mCjRed.B);
            this.cjBlue = System.Drawing.Color.FromArgb(mCjBlue.A, mCjBlue.R, mCjBlue.G, mCjBlue.B);

            this.InitChart();

            this.ViewModel.DataChanged += this.OnDataChanged;
            this.OnDataChanged();
        }
        private void InitChart()
        {
            this.InitDailyOrderChart();
            this.InitYesterdayNotOutChart();
            this.InitTodayOutChart();
            this.InitTotalOutChart();
        }
        private void InitDailyOrderChart()
        {
            Title title = new Title();
            title.ForeColor = System.Drawing.Color.White;
            this.todayOrderChartView.Titles.Add(title);

            ChartArea chartArea = new ChartArea();
            chartArea.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
            chartArea.AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.TitleForeColor = System.Drawing.Color.LightGray;
            chartArea.AxisX.LineColor = System.Drawing.Color.LightGray;
            chartArea.AxisX.MajorGrid.LineColor = System.Drawing.Color.White;
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.LabelStyle.ForeColor = System.Drawing.Color.LightGray;
            chartArea.AxisX.MajorTickMark.LineColor = System.Drawing.Color.LightGray;
            chartArea.AxisY.LineColor = System.Drawing.Color.LightGray;
            chartArea.AxisY.TitleForeColor = System.Drawing.Color.LightGray;
            //chartArea.AxisY.Maximum = 1000;
            chartArea.AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chartArea.AxisY.TitleForeColor = System.Drawing.Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = System.Drawing.Color.White;
            chartArea.AxisY.LabelStyle.ForeColor = System.Drawing.Color.LightGray;
            chartArea.AxisY.MajorTickMark.LineColor = System.Drawing.Color.LightGray;
            this.todayOrderChartView.ChartAreas.Add(chartArea);

            Series series = new Series();
            series.BackGradientStyle = GradientStyle.TopBottom;
            series.ChartType = SeriesChartType.Column;
            series.XValueType = ChartValueType.String;
            series.BackSecondaryColor = System.Drawing.Color.Aquamarine;
            series.LabelForeColor = System.Drawing.Color.Black;
            series.Color = System.Drawing.Color.SteelBlue;
            series.IsValueShownAsLabel = true;
            this.todayOrderChartView.Series.Add(series);

            Legend legend = new Legend();
            legend.Enabled = false;
            this.todayOrderChartView.Legends.Add(legend);

            this.dailyOrderChart.Child = this.todayOrderChartView;
        }
        private void InitYesterdayNotOutChart()
        {
            Title title = new Title();
            title.ForeColor = System.Drawing.Color.White;
            this.nonOutChartView.Titles.Add(title);

            ChartArea chartArea = new ChartArea();
            chartArea.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
            chartArea.AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.TitleForeColor = System.Drawing.Color.LightGray;
            chartArea.AxisX.LineColor = System.Drawing.Color.LightGray;
            chartArea.AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.LabelStyle.ForeColor = System.Drawing.Color.LightGray;
            chartArea.AxisX.MajorTickMark.LineColor = System.Drawing.Color.LightGray;
            chartArea.AxisY.LineColor = System.Drawing.Color.LightGray;
            chartArea.AxisY.TitleForeColor = System.Drawing.Color.LightGray;
            //chartArea.AxisY.Maximum = 1000;
            chartArea.AxisY.TitleForeColor = System.Drawing.Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea.AxisY.LabelStyle.ForeColor = System.Drawing.Color.LightGray;
            chartArea.AxisY.MajorTickMark.LineColor = System.Drawing.Color.LightGray;
            this.nonOutChartView.ChartAreas.Add(chartArea);

            Series series = new Series();
            series.BackGradientStyle = GradientStyle.TopBottom;
            series.ChartType = SeriesChartType.Column;
            series.XValueType = ChartValueType.String;
            series.BackSecondaryColor = System.Drawing.Color.Aquamarine;
            series.LabelForeColor = System.Drawing.Color.Black;
            series.Color = System.Drawing.Color.SteelBlue;
            series.IsValueShownAsLabel = true;
            this.nonOutChartView.Series.Add(series);

            Legend legend = new Legend();
            legend.Enabled = false;
            this.nonOutChartView.Legends.Add(legend);

            this.yesterdayNotOutChart.Child = this.nonOutChartView;
        }
        private void InitTodayOutChart()
        {
            this.todayOutChartView.ChartAreas.Add(new ChartArea());

            Series series = new Series();
            series.BackGradientStyle = GradientStyle.TopBottom;
            series.ChartType = SeriesChartType.Pie;
            series.XValueType = ChartValueType.String;
            series.BackSecondaryColor = System.Drawing.Color.Aquamarine;
            series.LabelForeColor = System.Drawing.Color.Black;
            series.Color = System.Drawing.Color.SteelBlue;
            series.IsValueShownAsLabel = true;
            series.Label = "#PERCENT{P1}";

            series.Points.Add(0);
            series.Points.Add(0);

            series.Points[0].LegendText = "출고 예정";
            series.Points[1].LegendText = "출고 완료";
            this.todayOutChartView.Series.Add(series);

            Legend legend = new Legend();
            this.todayOutChartView.Legends.Add(legend);

            this.todayOutChart.Child = this.todayOutChartView;
        }
        private void InitTotalOutChart()
        {
            Title title = new Title();
            title.ForeColor = System.Drawing.Color.White;
            this.totalOutChartView.Titles.Add(title);

            ChartArea chartArea = new ChartArea();
            chartArea.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
            chartArea.AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.TitleForeColor = System.Drawing.Color.LightGray;
            chartArea.AxisX.LineColor = System.Drawing.Color.LightGray;
            chartArea.AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.LabelStyle.ForeColor = System.Drawing.Color.LightGray;
            chartArea.AxisX.MajorTickMark.LineColor = System.Drawing.Color.LightGray;
            chartArea.AxisY.LineColor = System.Drawing.Color.LightGray;
            chartArea.AxisY.TitleForeColor = System.Drawing.Color.LightGray;
            //chartArea.AxisY.Maximum = 1000;
            chartArea.AxisY.TitleForeColor = System.Drawing.Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea.AxisY.LabelStyle.ForeColor = System.Drawing.Color.LightGray;
            chartArea.AxisY.MajorTickMark.LineColor = System.Drawing.Color.LightGray;
            this.totalOutChartView.ChartAreas.Add(chartArea);

            Series series = new Series();
            series.BackGradientStyle = GradientStyle.TopBottom;
            series.ChartType = SeriesChartType.Line;
            series.XValueType = ChartValueType.String;
            series.BackSecondaryColor = System.Drawing.Color.Aquamarine;
            series.LabelForeColor = System.Drawing.Color.Black;
            series.Color = System.Drawing.Color.SteelBlue;
            series.IsValueShownAsLabel = true;
            this.totalOutChartView.Series.Add(series);

            Legend legend = new Legend();
            legend.Enabled = false;
            this.totalOutChartView.Legends.Add(legend);

            this.totalOutChart.Child = this.totalOutChartView;
        }

        private void OnDataChanged()
        {
            this.Dispatcher.Invoke(() =>
            {
                this.TodayDate.Text = DateTime.Now.AddHours(-7).ToString("yyyy-MM-dd");
                this.HourlyOutCount.Text = this.ViewModel.HourlyOutCount.ToString("N0");
                this.TodayCaseErectRejectCount.Text = this.ViewModel.TodayCaseErectRejectCount.ToString("N0");
                this.TodayWeightRejectCount.Text = this.ViewModel.TodayWeightRejectCount.ToString("N0");
                this.TodaySmartPackingRejectCount.Text = this.ViewModel.TodaySmartPackingRejectCount.ToString("N0");
                this.TodayTopRejectCount.Text = this.ViewModel.TodayTopRejectCount.ToString("N0");
                this.OnOrderDataChanged();
                this.OnNonOutDataChanged();
                this.OnTodayOutDataChanged();
                this.OnTotalOutDataChanged();
            });
        }
        private void OnOrderDataChanged()
        {
            this.todayOrderChartView.Series[0].Points.Clear();
            var data = this.ViewModel.OrderData;
            foreach (var d in data)
                this.todayOrderChartView.Series[0].Points.AddXY(d.key, d.count);
            this.TodayOrderCount.Text = data.Last().count.ToString("N0");

        }

        private void OnNonOutDataChanged()
        {
            this.nonOutChartView.Series[0].Points.Clear();
            var data = this.ViewModel.NonOutData;
            foreach (var d in data)
                this.nonOutChartView.Series[0].Points.AddXY(d.key, d.count);
            this.NonOutCount.Text = data.Last().count.ToString("N0");
        }

        private void OnTodayOutDataChanged()
        {
            var data = this.ViewModel.TodayOutData;
            if (data == (0, 0))
                data = (1, 0);
            this.todayOutChartView.Series[0].Points[0].YValues = new double[] { data.orderData };
            this.todayOutChartView.Series[0].Points[1].YValues = new double[] { data.outData };
            this.TodayOutCount.Text = data.outData.ToString("N0");
        }

        private void OnTotalOutDataChanged()
        {

            this.totalOutChartView.Series[0].Points.Clear();
            var data = this.ViewModel.TotalOutData;
            foreach (var d in data)
                this.totalOutChartView.Series[0].Points.AddXY(d.key, d.count);
            this.TotalOutCount.Text = this.ViewModel.TotalOutCount.ToString("N0");
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            this.ViewModel.DataChanged -= this.OnDataChanged;
        }
    }
}
