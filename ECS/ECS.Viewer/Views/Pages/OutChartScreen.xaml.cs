using ECS.Core.Util;
using ECS.Core.ViewModels.Viewer;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Forms.DataVisualization.Charting;

namespace ECS.Viewer.Views.Pages
{
    public partial class OutChartScreen : Page
    {
        private CountingViewModel ViewModel => (CountingViewModel)DataContext;
        private Chart chartView = new Chart();
        private ChartArea chartArea = new ChartArea();
        private SharpTimer midNightTimer = new SharpTimer(0, 0, 0);


        public OutChartScreen()
        {
            InitializeComponent();

            this.InitDatePicker();
            this.InitChart();

            this.midNightTimer.Elapsed += MidNightTimer_Elapsed;
            this.midNightTimer.Start();

            this.ViewModel.DataChanged += this.OnDataChanged;
            this.OnDataChanged();
        }

        private void MidNightTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.midNightTimer.SetSarpInterval();

            this.InitDatePicker();
        }

        private void InitDatePicker()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                this.datePickerSelectDay.SelectedDate = DateTime.Today;
                this.datePickerSelectDay.DisplayDateStart = DateTime.Today.AddDays(-30);
                this.datePickerSelectDay.DisplayDateEnd = DateTime.Today;
            }));
        }

        private void InitChart()
        {
            Title title = new Title();
            title.ForeColor = System.Drawing.Color.White;
            this.chartView.Titles.Add(title);

            this.chartArea = new ChartArea();
            this.chartArea.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
            this.chartArea.AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
            this.chartArea.AxisX.Interval = 1;
            this.chartArea.AxisX.TitleForeColor = System.Drawing.Color.LightGray;
            this.chartArea.AxisX.LineColor = System.Drawing.Color.LightGray;
            this.chartArea.AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            this.chartArea.AxisX.MajorGrid.Enabled = false;
            this.chartArea.AxisX.LabelStyle.ForeColor = System.Drawing.Color.LightGray;
            this.chartArea.AxisX.MajorTickMark.LineColor = System.Drawing.Color.LightGray;
            this.chartArea.AxisY.LineColor = System.Drawing.Color.LightGray;
            this.chartArea.AxisY.TitleForeColor = System.Drawing.Color.LightGray;
            this.chartArea.AxisY.Maximum = 1000;
            this.chartArea.AxisY.TitleForeColor = System.Drawing.Color.LightGray;
            this.chartArea.AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            this.chartArea.AxisY.LabelStyle.ForeColor = System.Drawing.Color.LightGray;
            this.chartArea.AxisY.MajorTickMark.LineColor = System.Drawing.Color.LightGray;
            this.chartView.ChartAreas.Add(this.chartArea);

            Series series = new Series();
            series.BackGradientStyle = GradientStyle.TopBottom;
            series.ChartType = SeriesChartType.Column;
            series.XValueType = ChartValueType.String;
            series.BackSecondaryColor = System.Drawing.Color.Aquamarine;
            series.LabelForeColor = System.Drawing.Color.Black;
            series.Color = System.Drawing.Color.SteelBlue;
            series.IsValueShownAsLabel = true;
            this.chartView.Series.Add(series);

            Legend legend = new Legend();
            legend.Enabled = false;
            this.chartView.Legends.Add(legend);

            this.chart.Child = this.chartView;

            for (int i = 1; i <= 24; i++)
                this.chartView.Series[0].Points.AddXY($"{(i + 6) % 24}시", 0);
        }

        private void OnDataChanged()
        {
            var data = this.ViewModel.OutChartData;
            for (int i = 0; i < 24; i++)
                this.SetChartPoint(i, data[(i + 7) % 24]);
            this.chartArea.AxisY.Maximum = (data.Max() / 100 + 1) * 100;
            this.countText.Text = data.Sum().ToString();
        }

        private void SetChartPoint(int point, int value)
        {
            this.chartView.Series[0].Points[point].YValues = new double[] { value };
            this.chartView.Series[0].Points[point].Label = value.ToString();
        }

        private void Page_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ViewModel.DataChanged -= this.OnDataChanged;
        }
    }
}
